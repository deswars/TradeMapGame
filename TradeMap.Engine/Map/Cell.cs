using System.Collections.Generic;
using TradeMap.Core;
using TradeMap.Core.Map.Mutable;
using TradeMap.Core.Map.ReadOnly;

namespace TradeMap.Engine.Map
{
    public class Cell : ICellMutable
    {
        public ITerrainType Terrain { get; private set; }
        public IReadOnlyList<ITerrainFeautre> Feautres { get; private set; }
        public Point Position { get; }
        public ICollector? Collector { get => CollectorMut; }
        public ISettlement? Settlement { get => SettlementMut; }
        public IReadOnlyFlagRepository Flags { get => FlagsMut; }

        public ICollectorMutable? CollectorMut { get; set; }
        public ISettlementMutable? SettlementMut { get; set; }
        public FlagRepository FlagsMut { get; }


        public Cell(
            ITerrainType terrain,
            IReadOnlyList<ITerrainFeautre> feautres,
            Point position)
        {
            Terrain = terrain;
            Feautres = feautres;
            Position = position;
            FlagsMut = new();
        }


        public void Terraform(ITerrainType terrain, IReadOnlyList<ITerrainFeautre> feautres)
        {
            Terrain = terrain;
            Feautres = feautres;
        }

        public override string ToString()
        {
            return $"Cell({Position})";
        }
    }
}
