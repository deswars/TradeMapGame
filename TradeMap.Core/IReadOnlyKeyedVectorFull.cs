using System.Collections.Generic;

namespace TradeMap.Core
{
    public interface IReadOnlyKeyedVectorFull<TIndex> : IReadOnlyKeyedVectorPartial<TIndex> where TIndex : notnull
    {
        KeyedVectorFull<TIndex> CloneFull();
        KeyedVectorFull<TIndex> Filter(IEnumerable<TIndex> keys);
        bool IsBigger(IReadOnlyKeyedVectorPartial<TIndex> smaller);
        bool IsSmaller(IReadOnlyKeyedVectorPartial<TIndex> bigger);
    }
}