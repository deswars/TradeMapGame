using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TradeMap.Core;
using TradeMap.Di.Attributes;
using TradeMap.GameLog;

namespace TradeMap.Di
{
    public class Manager
    {
        private const string _errorReadOnlyProperty = "ReadOnlyPropertyError";
        private const string _errorParserCreation = "ParserCreationError";
        private const string _errorValidatorCreation = "ValidatorCreationError";
        private const string _errorValidatorSupportedType = "ValidatorSupportedTypeError";
        private const string _errorParserSupportedType = "ParserSupportedTypeError";
        private const string _errorFeauturelessService = "FeauturelessServiceError";
        private const string _errorSubstepTooLarge = "SubstepTooLargeError";
        private const string _errorIncorrectSubturnType = "IncorrectSubturnTypeError";
        private const string _errorServiceNotFound = "ServiceNotFoundError";
        private const string _errorFeautreNotFound = "FeautreNotFoundError";
        private const string _errorIncorrectFeautreRegisterMethod = "IncorrectFeautreRegisterMethodError";


        private readonly IGameLog _log;
        private readonly ITypeRepository _repository;
        private readonly ITurnManager _eng;

        private readonly Dictionary<string, Service> _availableServices = new();
        private readonly Dictionary<string, Service> _registeredServices = new();
        private readonly Queue<Tuple<Service, Feautre, SubturnTypes, int>> _subscribeOrder = new();
        private readonly Dictionary<SubturnTypes, MethodInfo> _featureRegister = new();


        public Manager(IGameLog log, ITypeRepository gameTypesRepository, ITurnManager eng)
        {
            _log = log;
            _repository = gameTypesRepository;
            _eng = eng;

            var registerMethods = _eng.GetType().GetMethods().Where(m => m.GetCustomAttribute<SubturnTypeAttribute>() != null);
            foreach (var method in registerMethods)
            {
                if (!ValidateRegisterMethod(method))
                {
                    _log.AddEntry(InfoLevels.Error, () => new BaseLogEntry(_errorIncorrectFeautreRegisterMethod, new Dictionary<string, object> { { "method", method.Name } }));
                }
                else
                {
                    var attr = method.GetCustomAttribute<SubturnTypeAttribute>()!;
                    _featureRegister.Add(attr.Name, method);
                }
            }
        }

        public IReadOnlyDictionary<string, Service> FindAllAvailableServices()
        {
            ClearLists();
            var assemblyList = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblyList)
            {
                var typeList = assembly.GetTypes().Where(type => type.GetCustomAttribute<ServiceAttribute>() != null);
                foreach (var type in typeList)
                {
                    var service = GetServiceInfo(type);
                    if (service != null)
                    {
                        _availableServices[service.Name] = service;
                    }
                }
            }
            return _availableServices;
        }

        public bool TryRegisterTurnAction(SubturnTypes subturnType, string serviceName, string feautreName, int substep)
        {
            if (substep >= _eng.SubstepCount)
            {
                _log.AddEntry(InfoLevels.Error, () => new BaseLogEntry(_errorSubstepTooLarge, new Dictionary<string, object> { { "service", serviceName }, { "feautre", feautreName }, { "substep", substep } }));
                return false;
            }

            if (!_availableServices.ContainsKey(serviceName))
            {
                _log.AddEntry(InfoLevels.Error, () => new BaseLogEntry(_errorServiceNotFound, new Dictionary<string, object> { { "service", serviceName } }));
                return false;
            }
            var targetService = _availableServices[serviceName];

            if (!targetService.Feautres.ContainsKey(feautreName))
            {
                _log.AddEntry(InfoLevels.Error, () => new BaseLogEntry(_errorFeautreNotFound, new Dictionary<string, object> { { "service", serviceName }, { "feautre", feautreName } }));
                return false;
            }
            var targetFeautre = _availableServices[serviceName].Feautres[feautreName];

            if (!ValidateFeautre(targetFeautre.Method, subturnType))
            {
                _log.AddEntry(InfoLevels.Error, () => new BaseLogEntry(_errorIncorrectSubturnType, new Dictionary<string, object> { { "service", serviceName }, { "feautre", feautreName }, { "type", subturnType } }));
                return false;
            }

            _subscribeOrder.Enqueue(new Tuple<Service, Feautre, SubturnTypes, int>(targetService, targetFeautre, subturnType, substep));
            _registeredServices[targetService.Name] = targetService;
            return true;
        }

        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, Constant>> CollectConstantDemands()
        {
            Dictionary<string, IReadOnlyDictionary<string, Constant>> result = new();
            foreach (var serviceKV in _registeredServices)
            {
                result[serviceKV.Key] = serviceKV.Value.Constants;
            }
            return result;
        }

        public void CreateServicesAndSubscribeActions(IReadOnlyDictionary<string, IReadOnlyDictionary<string, Constant>> constants)
        {
            Dictionary<string, object> createdServices = new();
            foreach (var subscribeElement in _subscribeOrder)
            {
                Service service = subscribeElement.Item1;
                Feautre action = subscribeElement.Item2;
                SubturnTypes type = subscribeElement.Item3;
                int substep = subscribeElement.Item4;

                if (!createdServices.ContainsKey(service.Name))
                {
                    object newService = CreateService(service);
                    InitializeConstants(newService, constants[service.Name]);
                    createdServices[service.Name] = newService;
                }

                var delegateType = _featureRegister[type].GetParameters()[0].ParameterType;
                var feautreDelegate = Delegate.CreateDelegate(delegateType, createdServices[service.Name], action.Method);
                object[] param;
                if (_featureRegister[type].GetParameters().Length == 1)
                {
                    param = new object[] { feautreDelegate };
                }
                else
                {
                    param = new object[] { feautreDelegate, substep };
                }
                _featureRegister[type].Invoke(_eng, param);
            }
        }


        private static bool ValidateRegisterMethod(MethodInfo method)
        {
            var methodParam = method.GetParameters();
            if ((methodParam.Length > 2) || (methodParam.Length < 1))
            {
                return false;
            }
            if (!methodParam[0].ParameterType.IsSubclassOf(typeof(MulticastDelegate)))
            {
                return false;
            }
            if ((methodParam.Length == 2) && (methodParam[1].ParameterType != typeof(int)))
            {
                return false;
            }
            return true;
        }

        private void ClearLists()
        {
            _availableServices.Clear();
            _registeredServices.Clear();
            _subscribeOrder.Clear();
        }

        private Service? GetServiceInfo(Type serviceType)
        {
            if (serviceType.GetConstructor(new Type[] { typeof(IGameLog) }) == null)
            {
                return null;
            }

            string serviceName = serviceType.GetCustomAttribute<ServiceAttribute>()!.Name;

            var constantList = GetConstantList(serviceType, serviceName);
            if (constantList == null)
            {
                return null;
            }

            var feautreList = GetFeautreList(serviceType, serviceName);
            if (feautreList == null)
            {
                return null;
            }

            return new Service(serviceName, serviceType, constantList, feautreList);
        }

        private Dictionary<string, Constant>? GetConstantList(Type serviceType, string serviceName)
        {
            Dictionary<string, Constant> result = new();
            var propertyList = serviceType.GetProperties().Where(property => property.GetCustomAttribute<ConstantAttribute>() != null);

            foreach (var property in propertyList)
            {
                ConstantAttribute attribute = property.GetCustomAttribute<ConstantAttribute>()!;
                string name = attribute.Name;
                if (!property.CanWrite)
                {
                    _log.AddEntry(InfoLevels.Error, () => new BaseLogEntry(_errorReadOnlyProperty, new Dictionary<string, object> { { "service", serviceName }, { "constant", name } }));
                    return null;
                }

                var parserAttr = attribute.Parser.GetCustomAttribute<SupportedTypeAttribute>();
                if ((parserAttr == null) || (parserAttr.SupportedType != property.PropertyType))
                {
                    _log.AddEntry(InfoLevels.Error, () => new BaseLogEntry(_errorParserSupportedType, new Dictionary<string, object> { { "service", serviceName }, { "constant", name } }));
                    return null;
                }
                IParserJson? parser = ParserBuilder.Build(attribute.Parser, attribute.ParserArgs);
                if (parser == null)
                {
                    _log.AddEntry(InfoLevels.Error, () => new BaseLogEntry(_errorParserCreation, new Dictionary<string, object> { { "service", serviceName }, { "constant", name } }));
                    return null;
                }

                IValidator? validator = null;
                if (attribute.Validator != null)
                {
                    var validatorAttr = attribute.Validator.GetCustomAttribute<SupportedTypeAttribute>();
                    if ((validatorAttr == null) || (validatorAttr.SupportedType != property.PropertyType))
                    {
                        _log.AddEntry(InfoLevels.Error, () => new BaseLogEntry(_errorValidatorSupportedType, new Dictionary<string, object> { { "service", serviceName }, { "constant", name } }));
                        return null;
                    }
                    validator = ValidatorBuilder.Build(attribute.Validator, attribute.ValidatorArgs!, _repository);
                    if (validator == null)
                    {
                        _log.AddEntry(InfoLevels.Error, () => new BaseLogEntry(_errorValidatorCreation, new Dictionary<string, object> { { "service", serviceName }, { "constant", name } }));
                        return null;
                    }
                }

                result[name] = new Constant(serviceName, name, parser, validator);
            }

            return result;
        }

        private Dictionary<string, Feautre>? GetFeautreList(Type serviceType, string serviceName)
        {
            Dictionary<string, Feautre> result = new();
            var methodList = serviceType.GetMethods().Where(property => property.GetCustomAttribute<FeautreAttribute>() != null);
            foreach (var method in methodList)
            {
                var attribute = method.GetCustomAttribute<FeautreAttribute>()!;
                result[attribute.Name] = new Feautre(attribute.Name, method);
            }

            if (!result.Any())
            {
                _log.AddEntry(InfoLevels.Error, () => new BaseLogEntry(_errorFeauturelessService, new Dictionary<string, object> { { "service", serviceName } }));
                return null;
            }

            return result;
        }

        private bool ValidateFeautre(MethodInfo feautre, SubturnTypes type)
        {
            if (!_featureRegister.ContainsKey(type))
            {
                return false;
            }
            var delegateType = _featureRegister[type].GetParameters()[0].ParameterType;
            var delegateMethod = delegateType.GetMethod("Invoke")!;
            if (delegateMethod.ReturnType != feautre.ReturnType)
            {
                return false;
            }
            var delegateParam = delegateMethod.GetParameters();
            var feautreParam = feautre.GetParameters();
            if (delegateParam.Length != feautreParam.Length)
            {
                return false;
            }
            for (int i = 0; i < delegateParam.Length; i++)
            {
                if (delegateParam[i].ParameterType != feautreParam[i].ParameterType)
                {
                    return false;
                }
            }
            return true;
        }

        private object CreateService(Service service)
        {
            Type type = service.ServiceType;
            return type.GetConstructor(new Type[] { typeof(IGameLog) })!.Invoke(new object[] { _log });
        }

        private static void InitializeConstants(object service, IReadOnlyDictionary<string, Constant> constantList)
        {
            var type = service.GetType();
            foreach (var property in type.GetProperties().Where(prop => prop.GetCustomAttribute<ConstantAttribute>() != null))
            {
                ConstantAttribute attribute = property.GetCustomAttribute<ConstantAttribute>()!;
                property.SetValue(service, constantList[attribute.Name].Value);
            }
        }
    }
}
