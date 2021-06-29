using System;

namespace TradeMap.Di.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TypeAttribute : Attribute
    {
        public string Name { get; }
        
        public TypeAttribute(string name)
        {
            Name = name;
        }
    }
}
