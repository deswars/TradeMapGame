using System.Collections.Generic;

namespace TradeMap.Core
{
    public interface IReadOnlyKeyedVectorPartial<TIndex> : IEnumerable<KeyValuePair<TIndex, double>> where TIndex : notnull
    {
        double this[TIndex index] { get; }
        KeyedVectorPartial<TIndex> Clone();
        KeyedVectorPartial<TIndex> FilterSmaller(IEnumerable<TIndex> keys);
    }
}