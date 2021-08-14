using TradeMap.Core;

namespace TradeMap.Di
{
    public interface IValidator
    {
        public bool Validate(object value, ITypeRepository repo);
    }
}
