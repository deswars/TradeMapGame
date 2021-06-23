using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using TradeMap.Di.Attributes;
using TradeMap.Di.GameLogEntry;
using TradeMap.GameLog;

namespace TradeMap.Di
{
    public class Manager<Tdest, TRepository>
    {
        public const string NameDivisor = "-";

        public Manager(IGameLog managerLog, IGameLog gameLog, TRepository gameTypesRepository)
        {
            _log = managerLog;
            _gameLog = gameLog;
            _repository = gameTypesRepository;

            EventInfo[] events = typeof(Tdest).GetEvents();
            foreach (var ev in events)
            {
                _possibleEvents[ev.Name] = ev;
            }
        }

        public IReadOnlyDictionary<string, EventInfo> GetPossibleActions()
        {
            return _possibleEvents;
        }

        public IReadOnlyDictionary<string, Service> CollectAllAvailableServices()
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
                        if (_availableServices.ContainsKey(service.Name))
                        {
                            _log.AddEntry(InfoLevels.Warning, () => new LogEntryServiceOverride(service.Name, _availableServices[service.Name].ServiceType, service.ServiceType));
                        }
                        _availableServices[service.Name] = service;
                    }
                }
            }
            return _availableServices;
        }

        public bool TryRegisterTurnAction(string eventName, string serviceName, string actionName)
        {
            if (!_possibleEvents.ContainsKey(eventName))
            {
                return false;
            }

            EventInfo targetEvent = _possibleEvents[eventName];
            if (!_availableServices.ContainsKey(serviceName))
            {
                return false;
            }

            Service targetService = _availableServices[serviceName];
            if (!_availableServices[serviceName].Actions.ContainsKey(actionName))
            {
                return false;
            }

            TurnAction targetAction = targetService.Actions[actionName];
            if (!IsActionTypeValid(targetEvent, targetAction))
            {
                return false;
            }

            _subscribeOrder.Enqueue(new Tuple<TurnAction, Service, string>(targetAction, targetService, eventName));
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

        public void CreateServicesAndSubscribeActions(Tdest dest, IReadOnlyDictionary<string, IReadOnlyDictionary<string, Constant>> constants)
        {
            Dictionary<string, object> createdServices = new();
            foreach (var subscribeElement in _subscribeOrder)
            {
                TurnAction action = subscribeElement.Item1;
                Service service = subscribeElement.Item2;
                string eventName = subscribeElement.Item3;

                if (!createdServices.ContainsKey(service.Name))
                {
                    object newService = CreateService(service);
                    Manager<Tdest, TRepository>.InitializeConstants(newService, constants[service.Name]);
                    createdServices[service.Name] = newService;
                }

                EventInfo eventInfo = _possibleEvents[eventName];
                var actionDelegate = Delegate.CreateDelegate(eventInfo.EventHandlerType!, createdServices[service.Name], action.Action);
                eventInfo.AddEventHandler(dest, actionDelegate);
            }
        }

        private readonly IGameLog _log;
        private readonly IGameLog? _gameLog;
        private readonly Dictionary<string, EventInfo> _possibleEvents = new();
        private readonly TRepository _repository;
        private readonly Dictionary<string, Service> _availableServices = new();
        private readonly Dictionary<string, Service> _registeredServices = new();
        private readonly Queue<Tuple<TurnAction, Service, string>> _subscribeOrder = new();

        private void ClearLists()
        {
            _availableServices.Clear();
            _registeredServices.Clear();
            _subscribeOrder.Clear();
        }

        private Service? GetServiceInfo(Type serviceType)
        {
            if (serviceType.GetConstructor(new Type[] { typeof(IGameLog), typeof(TRepository) }) == null)
            {
                return null;
            }
            string serviceName = serviceType.GetCustomAttribute<ServiceAttribute>()!.Name;
            var constantList = GetConstantList(serviceType, serviceName);
            if (constantList == null)
            {
                return null;
            }
            var actionList = Manager<Tdest, TRepository>.GetTurnActionList(serviceType);
            if ((actionList == null) || !actionList.Any())
            {
                _log.AddEntry(InfoLevels.Warning, () => new LogEntryActionlessService(serviceName, serviceType));
                return null;
            }
            return new Service(serviceName, serviceType, constantList, actionList);
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
                    _log.AddEntry(InfoLevels.Error, () => new LogEntryReadOnlyProperty(serviceType.Name, serviceName, name));
                    return null;
                }
                if (!attribute.Constraint.Check(attribute.DefaultValue))
                {
                    _log.AddEntry(InfoLevels.Error, () => new LogEntryDefaultValueConstraintError(serviceType.Name, serviceName, name, attribute.DefaultValue, attribute.Constraint));
                    return null;
                }
                result[name] = new Constant(name, attribute.DefaultValue, attribute.Constraint);
            }
            return result;
        }

        private static Dictionary<string, TurnAction>? GetTurnActionList(Type serviceType)
        {
            Dictionary<string, TurnAction> result = new();
            var methodList = serviceType.GetMethods().Where(property => property.GetCustomAttribute<TurnActionAttribute>() != null);
            foreach (var method in methodList)
            {
                TurnActionAttribute attribute = method.GetCustomAttribute<TurnActionAttribute>()!;
                result[attribute.Name] = new TurnAction(attribute.Name, method);
            }
            return result;
        }

        private static bool IsActionTypeValid(EventInfo targetEvent, TurnAction targetAction)
        {
            var handlerType = targetEvent.EventHandlerType;
            if (handlerType == null)
            {
                return false;
            }
            var delegateMethod = handlerType.GetMethod("Invoke")!;
            var actionMethod = targetAction.Action;
            if (delegateMethod.ReturnType != actionMethod.ReturnType)
            {
                return false;
            }
            var delegateArgs = delegateMethod.GetParameters();
            var actionArgs = actionMethod.GetParameters();
            if (delegateArgs.Length != actionArgs.Length)
            {
                return false;
            }
            for (int i = 0; i < delegateArgs.Length; i++)
            {
                if (delegateArgs[i].ParameterType != actionArgs[i].ParameterType)
                {
                    return false;
                }
            }
            return true;
        }

        private object CreateService(Service service)
        {
            Type type = service.ServiceType;
            return type.GetConstructor(new Type[] { typeof(IGameLog), typeof(TRepository) })!.Invoke(new object?[] { _gameLog, _repository });
        }

        private static void InitializeConstants(object service, IReadOnlyDictionary<string, Constant> constantList)
        {
            var type = service.GetType();
            foreach (var property in type.GetProperties().Where(prop => prop.GetCustomAttribute<ConstantAttribute>() != null))
            {
                ConstantAttribute attribute = property.GetCustomAttribute<ConstantAttribute>()!;
                var value = Convert.ChangeType(constantList[attribute.Name].Value, property.PropertyType, CultureInfo.InvariantCulture);
                property.SetValue(service, value);
            }
        }
    }
}
