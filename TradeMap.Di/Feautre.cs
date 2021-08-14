using System.Reflection;

namespace TradeMap.Di
{
    public class Feautre
    {
        public string Name { get; }
        public MethodInfo Method { get; }

        public Feautre(string name, MethodInfo action)
        {
            Name = name;
            Method = action;
        }
    }
}
