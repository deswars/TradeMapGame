using System.Collections.Generic;
using TradeMap.Core.Map.ReadOnly;

namespace TradeMap.Engine.Map
{
    public class CollectorType : ICollectorType
    {
        public string Id { get; }
        public double BasePower { get; }
        public int BaseLevel { get; }
        public IReadOnlyList<ITerrainType> RequiredTerrain { get; }
        public IReadOnlyList<ITerrainFeautre> RequiredFeautre { get; }
        public IReadOnlyList<IResourceType> Collected { get; }


        public CollectorType(
            string id,
            double basePower,
            int baseLevel,
            IReadOnlyList<ITerrainType> requiredTerrain,
            IReadOnlyList<ITerrainFeautre> requiredFeautre,
            IReadOnlyList<IResourceType> collected)
        {
            Id = id;
            BasePower = basePower;
            BaseLevel = baseLevel;
            RequiredTerrain = requiredTerrain;
            RequiredFeautre = requiredFeautre;
            Collected = collected;
        }

        public override string ToString()
        {
            var result = $"({Id} {BasePower} {BaseLevel} [ ";
            foreach (var terrain in RequiredTerrain)
            {
                result += terrain.Id + " ";
            }
            result += "] [";
            foreach (var feautre in RequiredFeautre)
            {
                result += feautre.Id + " ";
            }
            result += "] [";
            foreach (var resource in Collected)
            {
                result += resource.Id + " ";
            }
            result += "])";
            return result;
        }
    }
}
