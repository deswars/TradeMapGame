using System;

namespace TradeMap.Di.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public string? Name { get; private set; }
        public ServiceAttribute()
        {
        }

        public ServiceAttribute(string name)
        {
            Name = name;
        }
    }
}
