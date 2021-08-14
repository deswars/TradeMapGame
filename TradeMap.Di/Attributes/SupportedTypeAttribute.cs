using System;

namespace TradeMap.Di.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SupportedTypeAttribute : Attribute
    {
        public Type SupportedType { get; }


        public SupportedTypeAttribute(Type supportedType)
        {
            SupportedType = supportedType;
        }
    }
}
