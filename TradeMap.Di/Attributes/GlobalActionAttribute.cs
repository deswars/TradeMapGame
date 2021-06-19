using System;

namespace TradeMap.Di.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GlobalActionAttribute : Attribute
    {
        public string Name { get; private set; }

        public GlobalActionAttribute(string name)
        {
            Name = name;
        }
    }
}
