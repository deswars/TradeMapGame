using System;

namespace TradeMap.Di.Constraints
{
    public static class ConstraintFactory
    {
        public static IConstraint CreateConstraint(Type type, string args)
        {
            if (type == typeof(ConstraintAll))
            {
                return new ConstraintAll();
            }
            if (type == typeof(ConstraintInt))
            {
                return new ConstraintInt();
            }
            if (type == typeof(ConstraintDouble))
            {
                return new ConstraintDouble();
            }
            if (type == typeof(ConstraintMinMax))
            {
                return new ConstraintMinMax(args);
            }
            return new ConstraintAll();
        }
    }
}
