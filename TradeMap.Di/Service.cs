using System;
using System.Collections.Generic;

namespace TradeMap.Di
{
    public class Service
    {
        public string Name { get; }
        public Type ServiceType { get; }
        public IReadOnlyDictionary<string, Constant> Constants { get; }
        public IReadOnlyDictionary<string, TurnAction> Actions { get; }

        public Service(string name, Type serviceType, IReadOnlyDictionary<string, Constant> constants, IReadOnlyDictionary<string, TurnAction> actions)
        {
            Name = name;
            ServiceType = serviceType;
            Constants = constants;
            Actions = actions;
        }
    }
}
