using System.Collections.Generic;

namespace TradeMapGame.Map
{
    public class TerrainFeautre
    {
        public string Id { get; }
        public List<ResourceDeposit> Resources { get; }


        public TerrainFeautre(string id, IEnumerable<ResourceDeposit> resources)
        {
            Id = id;

            Resources = new();
            Resources.AddRange(resources);
        }
    }
}
