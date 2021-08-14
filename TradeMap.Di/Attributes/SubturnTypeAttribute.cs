using System;
using TradeMap.Core;

namespace TradeMap.Di.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SubturnTypeAttribute : Attribute
    {
        public SubturnTypes Name { get; }


        public SubturnTypeAttribute(SubturnTypes name)
        {
            Name = name;
        }
    }
}
