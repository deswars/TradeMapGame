using System.Collections.Generic;
using TradeMap.Core.Map.ReadOnly;

namespace TradeMap.Engine.Map
{
    public class TerrainFeautre : ITerrainFeautre
    {
        public string Id { get; }
        public IReadOnlyList<ITerrainType> RequiredTerrain { get; }
        public IReadOnlyList<IResourceDeposit> Deposits { get; }


        public TerrainFeautre(string id, IReadOnlyList<ITerrainType> requiredTerrain, IReadOnlyList<IResourceDeposit> deposits)
        {
            Id = id;
            RequiredTerrain = requiredTerrain;
            Deposits = deposits;
        }


        public override string ToString()
        {
            var result = $"({Id} [ ";
            foreach (var terrain in RequiredTerrain)
            {
                result += terrain.Id + " ";
            }
            result += "] [";
            foreach (var deposit in Deposits)
            {
                result += deposit + " ";
            }
            result += "])";
            return result;
        }
    }
}
