using System.Globalization;

namespace TradeMap.Di.Constraints
{
    public class ConstraintInt : IConstraint
    {
        public ConstraintInt()
        {
        }

        public bool Check(string value)
        {
            return int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
        }
    }
}
