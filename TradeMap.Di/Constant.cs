using TradeMap.Di.Constraints;

namespace TradeMap.Di
{
    public class Constant
    {
        public string Name { get; }
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (Constraint.Check(value))
                {
                    _value = value;
                }
            }
        }
        public IConstraint Constraint { get; }

        public Constant(string name, string defaultValue, IConstraint constraint)
        {
            Name = name;
            _value = defaultValue;
            Constraint = constraint;
        }

        private string _value;
    }
}
