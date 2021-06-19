using System;

namespace TradeMap.Di.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SettlementActionAttribute : Attribute
    {
        public string Name { get; private set; }

        public SettlementActionAttribute(string name)
        {
            Name = name;
        }
    }
}
