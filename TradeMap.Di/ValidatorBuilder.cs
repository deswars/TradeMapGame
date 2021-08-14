using System;
using TradeMap.Core;

namespace TradeMap.Di
{
    public static class ValidatorBuilder
    {
        public static IValidator? Build(Type type, string[] args, ITypeRepository repo)
        {
            try
            {
                var constructor = type.GetConstructor(new Type[] { typeof(string[]) });
                if (constructor != null)
                {
                    return (IValidator)constructor.Invoke(new object[] { args });
                }
                constructor = type.GetConstructor(Array.Empty<Type>());
                if (constructor != null)
                {
                    return (IValidator)constructor.Invoke(null);
                }
            }
            catch { }
            return null;
        }
    }
}
