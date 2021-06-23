using System;

namespace TradeMap.Di.Constraints
{
    public interface IConstraint
    {
        bool Check(string value);
    }
}
