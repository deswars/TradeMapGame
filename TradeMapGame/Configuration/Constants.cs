using TradeMapGame.Map;

namespace TradeMapGame.Configuration
{
    public class Constants
    {
        public string MoneyResourceId { get; internal set; }
        public ResourceType MoneyResource { get; internal set; }
        public string DefaultTerrainId { get; internal set; }
        public TerrainType DefaultTerrain { get; internal set; }
        public double MinPrice { get; internal set; } = 1;
        public double MaxPrice { get; internal set; } = 1;
        public double TaxPerPop { get; internal set; } = 0;
        public int MaxPopTier { get; internal set; } = 0;
        public double TierLevelLimit { get; internal set; } = 100;
        public double TierLevelUpStep { get; internal set; } = 10;
        public double TierLevelUpLimit { get; internal set; } = 0.9;
        public double TierLevelDownStep { get; internal set; } = 10;
        public double TierLevelDownLimit { get; internal set; } = 0.5;


        internal Constants(ResourceType moneyResource, TerrainType baseTerrain)
        {
            DefaultTerrainId = baseTerrain.Id;
            DefaultTerrain = baseTerrain;
            MoneyResourceId = moneyResource.Id;
            MoneyResource = moneyResource;
        }
    }
}
