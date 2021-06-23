using System.Reflection;

namespace TradeMap.Di
{
    public class TurnAction
    {
        public string Name { get; }
        public MethodInfo Action { get; }

        public TurnAction(string name, MethodInfo action)
        {
            Name = name;
            Action = action;
        }
    }
}
