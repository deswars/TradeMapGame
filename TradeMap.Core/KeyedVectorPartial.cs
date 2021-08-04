using System.Collections;
using System.Collections.Generic;

namespace TradeMap.Core
{
    public class KeyedVectorPartial<TIndex> : IReadOnlyKeyedVectorPartial<TIndex> where TIndex : notnull
    {
        protected Dictionary<TIndex, double> Dict { get; private set; }


        public double this[TIndex index]
        {
            get
            {
                return Dict[index];
            }
            set
            {
                if (Dict.ContainsKey(index))
                {
                    Dict[index] = value;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
        }


        protected KeyedVectorPartial(Dictionary<TIndex, double> initializer)
        {
            Dict = new(initializer);
        }

        protected KeyedVectorPartial()
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

        public virtual KeyedVectorPartial<TIndex> Clone()
        {
            KeyedVectorPartial<TIndex> result = new(Dict);
            return result;
        }

        public virtual void Zero()
        {
            foreach (var key in Dict.Keys)
            {
                Dict[key] = 0;
            }
        }

        public void Neg()
        {
            foreach (var pair in this)
            {
                Dict[pair.Key] = -pair.Value;
            }
        }

        public KeyedVectorPartial<TIndex> FilterSmaller(IEnumerable<TIndex> keys)
        {
            KeyedVectorPartial<TIndex> result = new();
            foreach (var key in keys)
            {
                result.Dict[key] = Dict[key];
            }
            return result;
        }
    }
}
