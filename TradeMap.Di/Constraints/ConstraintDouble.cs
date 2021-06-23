using System.Globalization;

namespace TradeMap.Di.Constraints
{
    public class ConstraintDouble : IConstraint
    {
        public ConstraintDouble()
        {
        }

        public bool Check(string value)
        {
            return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
        }
    }
}
