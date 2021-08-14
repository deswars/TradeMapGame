using System;

namespace TradeMap.Di.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TypeCategoryAttribute : Attribute
    {
        public string Name { get; }


        public TypeCategoryAttribute(string name)
        {
            Name = name;
        }
    }
}
