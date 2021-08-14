using System;

namespace TradeMap.Di.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FeautreAttribute : Attribute
    {
        public string Name { get; }


        public FeautreAttribute(string name)
        {
            Name = name;
        }
    }
}
