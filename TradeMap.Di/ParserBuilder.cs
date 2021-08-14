using System;

namespace TradeMap.Di
{
    public static class ParserBuilder
    {
        public static IParserJson? Build(Type type, string[] args)
        {
            try
            {
                var constructor = type.GetConstructor(new Type[] { typeof(string[]) });
                if (constructor != null)
                {
                    return (IParserJson)constructor.Invoke(new object[] { args });
                }
                constructor = type.GetConstructor(Array.Empty<Type>());
                if (constructor != null)
                {
                    return (IParserJson)constructor.Invoke(null);
                }
            }
            catch { }
            return null;
        }
    }
}
