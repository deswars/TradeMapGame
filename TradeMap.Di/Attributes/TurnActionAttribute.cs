using System;

namespace TradeMap.Di.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TurnActionAttribute : Attribute
    {
        public string Name { get; }

        public TurnActionAttribute(string name)
        {
            Name = name;
        }
    }
}
