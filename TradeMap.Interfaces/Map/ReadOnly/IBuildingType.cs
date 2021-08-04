﻿using TradeMap.Core;

namespace TradeMap.Interfaces.Map.ReadOnly
{
    public interface IBuildingType
    {
        public string Id { get; }
        public int BaseLevel { get; }
        public IReadOnlyKeyedVectorPartial<IResourceType> Input { get; }
        public IReadOnlyKeyedVectorPartial<IResourceType> Output { get; }
    }
}
