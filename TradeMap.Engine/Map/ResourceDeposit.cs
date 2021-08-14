using TradeMap.Core.Map.ReadOnly;

namespace TradeMap.Engine.Map
{
    public class ResourceDeposit : IResourceDeposit
    {
        public IResourceType Type { get; }
        public double Richness { get; }
        public double Hardness { get; }


        public ResourceDeposit(IResourceType type, double richness, double hardness)
        {
            Type = type;
            Richness = richness;
            Hardness = hardness;
        }


        public override string ToString()
        {
            return $"({Type.Id} {Richness} {Hardness})";
        }
    }
}
