using System.Collections;
using System.Collections.Generic;

namespace TradeMapGame
{
    public class KeyedVector<TIndex> : IEnumerable<KeyValuePair<TIndex, double>> where TIndex : notnull
    {
        public Dictionary<TIndex, double> Dict { get; set; }

        public double this[TIndex index]
        {
            get
            {
                return Dict[index];
            }
            set
            {
                Dict[index] = value;
            }
        }

        public KeyedVector(ICollection<TIndex> indexes)
        {
            Dict = new();
            foreach (var index in indexes)
            {
                Dict.Add(index, 0);
            }
        }

        private KeyedVector()
        {
            Dict = new();
        }

        public IEnumerator<KeyValuePair<TIndex, double>> GetEnumerator()
        {
            return Dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {

            return Dict.GetEnumerator();
        }

        public KeyedVector<TIndex> Clone()
        {
            KeyedVector<TIndex> result = new();
            foreach (var pair in this)
            {
                result.Dict.Add(pair.Key, pair.Value);
            }
            return result;
        }

        public KeyedVector<TIndex> Zeroed()
        {
            KeyedVector<TIndex> result = new();
            foreach (var pair in this)
            {
                result.Dict.Add(pair.Key, 0);
            }
            return result;
        }

        public void Add(KeyedVector<TIndex> vec)
        {
            foreach (var pair in vec)
            {
                Dict[pair.Key] += pair.Value;
            }
        }

        public KeyedVector<TIndex> AddNew(KeyedVector<TIndex> vec)
        {
            var result = Clone();
            foreach (var pair in vec)
            {
                result.Dict[pair.Key] += pair.Value;
            }
            return result;
        }

        public void Sub(KeyedVector<TIndex> vec)
        {
            foreach (var pair in vec)
            {
                Dict[pair.Key] -= pair.Value;
            }
        }

        public KeyedVector<TIndex> SubNew(KeyedVector<TIndex> vec)
        {
            var result = Clone();
            foreach (var pair in vec)
            {
                result.Dict[pair.Key] -= pair.Value;
            }
            return result;
        }

        public void Neg()
        {
            foreach (var pair in this)
            {
                Dict[pair.Key] = -pair.Value;
            }
        }

        public KeyedVector<TIndex> NegNew()
        {
            var result = Clone();
            foreach (var pair in this)
            {
                result.Dict[pair.Key] -= pair.Value;
            }
            return result;
        }

        public KeyedVector<TIndex> Filter(List<TIndex> keys)
        {
            var result = Zeroed();
            foreach (var key in keys)
            {
                result.Dict[key] = Dict[key];
            }
            return result;
        }

        public bool IsSmaller(KeyedVector<TIndex> bigger)
        {
            foreach (var pair in bigger)
            {
                if (pair.Value <= this[pair.Key])
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsBigger(KeyedVector<TIndex> smaller)
        {
            foreach (var pair in smaller)
            {
                if (pair.Value >= this[pair.Key])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
