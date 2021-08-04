using System.Collections.Generic;
using TradeMap.Core;

namespace TradeMap.Interfaces.Map.ReadOnly
{
    public interface ISettlement
    {
        public string Name { get; }
        public Point Position { get; }
        public IReadOnlyKeyedVectorFull<IResourceType> Resources { get; }
        public IReadOnlyKeyedVectorFull<IResourceType> Prices { get; }
        public int Population { get; }
        public IReadOnlyList<ICollector> Collectors { get; }
        public IReadOnlyList<IBuilding> Buildings { get; }
        public int Level { get; }
        public double LevelProgress { get; }
        public IReadOnlyFlagRepository Flags { get; }
    }
}
