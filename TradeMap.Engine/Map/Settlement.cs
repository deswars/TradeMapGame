using System.Collections.Generic;
using TradeMap.Core;
using TradeMap.Core.Map.Mutable;
using TradeMap.Core.Map.ReadOnly;

namespace TradeMap.Engine.Map
{
    public class Settlement : ISettlementMutable
    {
        public string Name { get; }
        public Point Position { get; }
        public IReadOnlyKeyedVectorFull<IResourceType> Resources { get => ResourcesMut; }
        public IReadOnlyKeyedVectorFull<IResourceType> Prices { get => PricesMut; }
        public int Population { get => PopulationMut; }
        public int Level { get; private set; }
        public double LevelProgress { get; private set; }
        public IReadOnlyList<ICollector> Collectors { get => CollectorsMut; }
        public IReadOnlyList<IBuilding> Buildings { get => BuildingsMut; }
        public IReadOnlyFlagRepository Flags { get => FlagsMut; }

        public KeyedVectorFull<IResourceType> ResourcesMut { get; set; }
        public KeyedVectorFull<IResourceType> PricesMut { get; set; }
        public int PopulationMut { get; set; }
        public List<ICollectorMutable> CollectorsMut { get; }
        public List<IBuildingMutable> BuildingsMut { get; }
        public FlagRepository FlagsMut { get; }


        public Settlement(
            string name,
            Point position,
            KeyedVectorFull<IResourceType> resources,
            KeyedVectorFull<IResourceType> prices,
            int population,
            int level,
            double levelProgress)
        {
            Name = name;
            Position = position;
            ResourcesMut = resources;
            PricesMut = prices;
            PopulationMut = population;
            Level = level;
            LevelProgress = levelProgress;
            CollectorsMut = new();
            BuildingsMut = new();
            FlagsMut = new();
        }


        public void ChangeLevel(int level, double levelProgress)
        {
            Level = level;
            LevelProgress = levelProgress;
        }
    }
}
