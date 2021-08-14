using TradeMap.Core;
using TradeMap.Di.Attributes;

namespace TradeMap.Di.Validators
{
    [SupportedType(typeof(double))]
    public class ValidatorDoubleMinMax : IValidator
    {
        private readonly double _min = double.MinValue;
        private readonly double _max = double.MaxValue;


        public ValidatorDoubleMinMax(string[] args)
        {
            if (args[0] == "min")
            {
                _min = double.Parse(args[1]);
            }
            else if (args[0] == "max")
            {
                _max = double.Parse(args[1]);
            }

            if (args.Length > 2)
            {
                if (args[2] == "min")
                {
                    _min = double.Parse(args[3]);
                }
                else if (args[2] == "max")
                {
                    _max = double.Parse(args[3]);
                }
            }
        }


        public bool Validate(object value, ITypeRepository repo)
        {
            double doubleValue = (double)value;
            if (doubleValue > _min && doubleValue < _max)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return $"{nameof(ValidatorDoubleMinMax)}+{_min}+{_max}";
        }

    }
}
