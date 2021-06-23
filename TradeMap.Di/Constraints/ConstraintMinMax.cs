using System;
using System.Globalization;

namespace TradeMap.Di.Constraints
{
    public class ConstraintMinMax : IConstraint
    {
        public ConstraintMinMax(string input)
        {
            var args = input.Split(" ");
            double min = double.NegativeInfinity;
            double max = double.PositiveInfinity;
            if (args.Length == 2)
            {
                if (double.TryParse(args[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double res))
                {
                    min = res;
                }
                if (double.TryParse(args[1], NumberStyles.Any, CultureInfo.InvariantCulture, out res))
                {
                    max = res;
                }
            }
            _min = min;
            _max = max;
        }

        public bool Check(string value)
        {
            if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
            {
                if (val >= _min && val <= _max)
                {
                    return true;
                }
            }
            return false;
        }

        private readonly double _min;
        private readonly double _max;
    }
}
