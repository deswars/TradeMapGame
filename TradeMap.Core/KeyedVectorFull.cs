using System.Collections.Generic;

namespace TradeMap.Core
{
    public class KeyedVectorFull<TIndex> : KeyedVectorPartial<TIndex>, IReadOnlyKeyedVectorFull<TIndex> where TIndex : notnull
    {
        private static Dictionary<TIndex, double> ZeroDictionary { get; set; } = new();


        public KeyedVectorFull() : base(ZeroDictionary)
        { }

        private KeyedVectorFull(Dictionary<TIndex, double> dict) : base(dict)
        { }


        public static void SetAvailableKeys(IEnumerable<TIndex> keyList)
        {
            foreach (var key in keyList)
            {
                ZeroDictionary[key] = 0;
            }
        }

        public KeyedVectorFull<TIndex> CloneFull()
        {
            KeyedVectorFull<TIndex> result = new(Dict);
            return result;
        }

        public void Add(IReadOnlyKeyedVectorPartial<TIndex> vec)
        {
            foreach (var pair in vec)
            {
                Dict[pair.Key] += pair.Value;
            }
        }

        public void Sub(IReadOnlyKeyedVectorPartial<TIndex> vec)
        {
            foreach (var pair in vec)
            {
                Dict[pair.Key] -= pair.Value;
            }
        }

        public KeyedVectorFull<TIndex> Filter(IEnumerable<TIndex> keys)
        {
            KeyedVectorFull<TIndex> result = new();
            foreach (var key in keys)
            {
                result.Dict[key] = Dict[key];
            }
            return result;
        }

        public bool IsSmaller(IReadOnlyKeyedVectorPartial<TIndex> bigger)
        {
            foreach (var pair in bigger)
            {
                if (pair.Value < this[pair.Key])
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsBigger(IReadOnlyKeyedVectorPartial<TIndex> smaller)
        {
            foreach (var pair in smaller)
            {
                if (pair.Value > this[pair.Key])
                {
                    return false;
                }
            }
            return true;
        }
    }
}