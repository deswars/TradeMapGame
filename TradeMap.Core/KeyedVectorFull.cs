using System.Collections.Generic;

namespace TradeMap.Core
{
    public class KeyedVectorFull<TIndex> : KeyedVectorPartial<TIndex> where TIndex : notnull
    {
        public KeyedVectorFull() : base(ZeroDictionary)
        { }

        private KeyedVectorFull(Dictionary<TIndex, double> dict) : base(dict)
        { }

        public static void InitializeKeys(IEnumerable<TIndex> keyList)
        {
            foreach (var key in keyList)
            {
                ZeroDictionary[key] = 0;
            }
        }

        public override KeyedVectorFull<TIndex> Clone()
        {
            KeyedVectorFull<TIndex> result = new(Dict);
            return result;
        }

        public void Add(KeyedVectorPartial<TIndex> vec)
        {
            foreach (var pair in vec)
            {
                Dict[pair.Key] += pair.Value;
            }
        }

        public void Sub(KeyedVectorPartial<TIndex> vec)
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

        public bool IsSmaller(KeyedVectorPartial<TIndex> bigger)
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

        public bool IsBigger(KeyedVectorPartial<TIndex> smaller)
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

        private static Dictionary<TIndex, double> ZeroDictionary { get; set; } = new();
    }
}