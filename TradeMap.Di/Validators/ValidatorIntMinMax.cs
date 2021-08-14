using TradeMap.Core;
using TradeMap.Di.Attributes;

namespace TradeMap.Di.Validators
{
    [SupportedType(typeof(int))]
    public class ValidatorIntMinMax : IValidator
    {
        private readonly int _min = int.MinValue;
        private readonly int _max = int.MaxValue;


        public ValidatorIntMinMax(string[] args)
        {
            if (args[0] == "min")
            {
                _min = int.Parse(args[1]);
            }
            else if (args[0] == "max")
            {
                _max = int.Parse(args[1]);
            }

            if (args.Length > 2)
            {
                if (args[2] == "min")
                {
                    _min = int.Parse(args[3]);
                }
                else if (args[2] == "max")
                {
                    _max = int.Parse(args[3]);
                }
            }
        }


        public bool Validate(object value, ITypeRepository repo)
        {
            int intValue = (int)value;
            if (intValue > _min && intValue < _max)
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
            return $"{nameof(ValidatorIntMinMax)}+{_min}+{_max}";
        }

    }
}
