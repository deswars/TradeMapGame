using System.Collections.Generic;

namespace TradeMapGame
{
    public class CollectorType
    {
        public string Id;
        public List<TerrainType> RequiredTerrain { get; }
        public List<TerrainFeautre> RequiredFeautre { get; }
        public List<ResourceType> Collected { get; }


        public CollectorType(string id, List<TerrainType> requiredTerrain, List<TerrainFeautre> requiredFeautre, List<ResourceType> collected)
        {
            Id = id;
            RequiredTerrain = requiredTerrain;
            RequiredFeautre = requiredFeautre;
            Collected = collected;
        }
    }
}
