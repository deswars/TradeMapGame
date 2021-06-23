using System;
using TradeMap.Di.Constraints;

namespace TradeMap.Di.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConstantAttribute : Attribute
    {
        public string Name { get; }
        public string DefaultValue { get; }
        public IConstraint Constraint { get; }

        public ConstantAttribute(string defaultValue, string name)
        {
            Name = name;
            DefaultValue = defaultValue;
            Constraint = new ConstraintAll();
        }

        public ConstantAttribute(string defaultValue, string name, Type constraint, string constraintParam)
        {
            Name = name;
            DefaultValue = defaultValue;
            Constraint = ConstraintFactory.CreateConstraint(constraint, constraintParam);
        }
    }
}
