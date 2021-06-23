using TradeMap.Di.Attributes;
using TradeMap.Di.Constraints;
using TradeMap.GameLog;

namespace TradeMapTests.Di.Stubs
{
    [Service("S2")]
    public class Service2
    {
        [Constant("a", "C1")]
        public string C1 { get; set; }

        [Constant("b", "C2")]
        public string C2 { get; set; }

        [Constant("2.5", "C3", typeof(ConstraintInt), "")]
        public string C3 { get; set; }

#pragma warning disable CA1822
#pragma warning disable IDE0060
        public Service2(IGameLog log, TypeRepository rep)
        { }
#pragma warning restore IDE0060
        [TurnAction("A1")]
        public void A1()
        { }
#pragma warning restore CA1822
    }
}
