using System.Collections.Generic;

namespace TradeMapGame
{
    public class TerrainType
    {
        public string Id { get; }
        public List<ResourceDeposit> Resources { get; }


        public TerrainType(string id, IEnumerable<ResourceDeposit> resources)
        {
            Id = id;

            Resources = new();
            Resources.AddRange(resources);
        }
    }
}
