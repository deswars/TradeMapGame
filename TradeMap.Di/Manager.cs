using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using TradeMap.Di.Attributes;
using TradeMap.Di.GameLog;
using TradeMap.GameLog;

namespace TradeMap.Di
{
    public class Manager
    {
        public const string NameDivisor = "-";

        public Manager(IGameLog log)
        {
            _log = log;
        }

        public IReadOnlyDictionary<string, Type> GetServiceList()
        {
            Dictionary<string, Type> result = new();
            var assemblyList = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblyList)
            {
                var typeList = assembly.GetTypes().Where(type => type.GetCustomAttribute<ServiceAttribute>() != null);
                foreach (var type in typeList)
                {
                    ServiceAttribute attribute = type.GetCustomAttribute<ServiceAttribute>()!;
                    var serviceName = GetServiceName(type, attribute);
                    if (result.ContainsKey(serviceName))
                    {
                        _log.AddEntry(InfoLevels.Error, () => new LogEntryServiceOverride(serviceName, result[serviceName], type));
                    }
                    else
                    {
                        result.Add(serviceName, type);
                    }
                }
            }
            return result;
        }

        public IReadOnlyDictionary<string, Tuple<Type, string>> GetServicConstantList()
        {
            Dictionary<string, Tuple<Type, string>> result = new();
            var assemblyList = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblyList)
            {
                var typeList = assembly.GetTypes().Where(type => type.GetCustomAttribute<ServiceAttribute>() != null);
                foreach (var type in typeList)
                {
                    ServiceAttribute attribute = type.GetCustomAttribute<ServiceAttribute>()!;
                    var constantList = GetConstantList(type, attribute);
                    foreach (var constantPair in constantList)
                    {
                        if (result.ContainsKey(constantPair.Key))
                        {
                            var oldConstant = result[constantPair.Key];
                            if (constantPair.Value.Item1 != oldConstant.Item1)
                            {
                                _log.AddEntry(InfoLevels.Error, () => new LogEntryConstantOverride(constantPair.Key, true));
                            }
                            else
                            {
                                _log.AddEntry(InfoLevels.Warning, () => new LogEntryConstantOverride(constantPair.Key, false));
                            }

                        }
                        result[constantPair.Key] = constantPair.Value;
                    }
                }
            }
            return result;
        }

        public void GetServiceActionList()
        {
            //TODO DI service methods
            throw new NotImplementedException();
        }

        public void AssignConstants()
        {
            //TODO DI constant assign
            throw new NotImplementedException();
        }

        public void BuildEngineTurn()
        {
            //TODO DI service build
            throw new NotImplementedException();
        }

        private readonly IGameLog _log;

        private IReadOnlyDictionary<string, Tuple<Type, string>> GetConstantList(Type serviceType, ServiceAttribute serviceAttribute)
        {
            Dictionary<string, Tuple<Type, string>> result = new();

            var constantList = serviceType.GetProperties().Where(property => property.GetCustomAttribute<ConstantAttribute>() != null);
            foreach (var constant in constantList)
            {
                ConstantAttribute attribute = constant.GetCustomAttribute<ConstantAttribute>()!;

                string name = GetConstantName(constant, attribute, serviceType, serviceAttribute);
                Type? constantType = GetConstantType(constant, attribute);
                if (constantType != null)
                {
                    result[name] = new Tuple<Type, string>(constantType, attribute.DefaultValue);
                }
                else
                {
                    _log.AddEntry(InfoLevels.Error, () => new LogEntryConstantTypeError(serviceType.Name, GetServiceName(serviceType, serviceAttribute), name));
                }
            }
            return result;
        }

        private static string GetConstantName(PropertyInfo constant, ConstantAttribute attribute, Type serviceType, ServiceAttribute serviceAttribute)
        {
            string name;
            if (attribute.Name == null)
            {
                name = constant.Name;
            }
            else
            {
                name = attribute.Name;
            }
            if (!attribute.IsFullName)
            {
                name = GetServiceName(serviceType, serviceAttribute) + NameDivisor + name;
            }
            return name;
        }

        private static string GetServiceName(Type serviceType, ServiceAttribute serviceAttribute)
        {
            return serviceAttribute.Name ?? serviceType.Name;
        }

        private static Type? GetConstantType(PropertyInfo constant, ConstantAttribute attribute)
        {
            Type? constantType = constant.PropertyType;
            if (attribute.ValueType != ValueTypes.None)
            {
                constantType = ConvertToType(attribute.ValueType);
            }
            bool isTypeValid = ValidateConstantType(constant, constantType);
            bool isValueValid = ValidateConstantDefault(attribute.DefaultValue, constantType);
            return isTypeValid && isValueValid ? constantType : null;
        }

        private static Type? ConvertToType(ValueTypes constantType)
        {
            return constantType switch
            {
                ValueTypes.Int => typeof(int),
                ValueTypes.Double => typeof(double),
                ValueTypes.String => typeof(string),
                _ => null,
            };
        }

        private static bool ValidateConstantType(PropertyInfo constant, Type? constantType)
        {
            return constant.PropertyType.IsAssignableFrom(constantType);
        }

        private static bool ValidateConstantDefault(string value, Type? constantType)
        {
            if (constantType == null)
            {
                return false;
            }
            try
            {
                var res = Convert.ChangeType(value, constantType, CultureInfo.InvariantCulture);
                return res != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
