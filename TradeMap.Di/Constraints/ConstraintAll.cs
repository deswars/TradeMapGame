namespace TradeMap.Di.Constraints
{
    public class ConstraintAll : IConstraint
    {
        public ConstraintAll()
        {
        }

        public bool Check(string value)
        {
            return true;
        }

        public override string ToString()
        {
            return "No constraint";
        }
    }
}
