using System;
using System.Collections.Generic;

namespace TradeMap.Di
{
    public class Service
    {
        public string Name { get; }
        public Type ServiceType { get; }
        public IReadOnlyDictionary<string, Constant> Constants { get; }
        public IReadOnlyDictionary<string, Feautre> Feautres { get; }

        public Service(string name, Type serviceType, IReadOnlyDictionary<string, Constant> constants, IReadOnlyDictionary<string, Feautre> feautres)
        {
            Name = name;
            ServiceType = serviceType;
            Constants = constants;
            Feautres = feautres;
        }
    }
}
