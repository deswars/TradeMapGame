using System.Collections.Generic;
using TradeMap.Core.Map.ReadOnly;

namespace TradeMap.Engine.Map
{
    public class TerrainType : ITerrainType
    {
        public string Id { get; }
        public IReadOnlyList<IResourceDeposit> Deposits { get; }


        public TerrainType(string id, IReadOnlyList<IResourceDeposit> deposits)
        {
            Id = id;
            Deposits = deposits;
        }


        public override string ToString()
        {
            var result = "(" + Id + " [ ";
            foreach (var deposit in Deposits)
            {
                result += deposit + " ";
            }
            result += "])";
            return result;
        }
    }
}
