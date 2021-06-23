using System;

namespace TradeMap.Di.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConstantAttribute : Attribute
    {
        //TODO replace with constraints
        public ValueTypes ValueType { get; private set; } = ValueTypes.None;
        public string? Name { get; private set; }
        public bool IsFullName { get; private set; } = false;
        public string DefaultValue { get; private set; }

        public ConstantAttribute(string defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public ConstantAttribute(string defaultValue, string name)
        {
            Name = name;
            DefaultValue = defaultValue;
        }

        public ConstantAttribute(string defaultValue, string name, bool isFullName)
        {
            Name = name;
            IsFullName = isFullName;
            DefaultValue = defaultValue;
        }

        public ConstantAttribute(string defaultValue, string name, ValueTypes valueType)
        {
            Name = name;
            ValueType = valueType;
            DefaultValue = defaultValue;
        }

        public ConstantAttribute(string defaultValue, string name, bool isFullName, ValueTypes valueType)
        {
            Name = name;
            ValueType = valueType;
            IsFullName = isFullName;
            DefaultValue = defaultValue;
        }
    }
}
