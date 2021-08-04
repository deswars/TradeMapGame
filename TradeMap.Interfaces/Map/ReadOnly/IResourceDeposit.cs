﻿namespace TradeMap.Interfaces.Map.ReadOnly
{
    public interface IResourceDeposit
    {
        public IResourceType Type { get; }
        public double Richness { get; }
        public double Hardness { get; }
    }
}
