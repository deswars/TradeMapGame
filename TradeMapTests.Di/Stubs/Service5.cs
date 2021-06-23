#pragma warning disable IDE0079
#pragma warning disable IDE0060
#pragma warning disable CA1822
using TradeMap.Di.Attributes;
using TradeMap.Di.Constraints;
using TradeMap.GameLog;

namespace TradeMapTests.Di.Stubs
{
    [Service("S5")]
    public class Service5
    {
        [Constant("a", "C1")]
        public string C1 { get; set; }

        [Constant("b", "C2")]
        public string C2 { get; set; }

        [Constant("1", "C3", typeof(ConstraintInt), "")]
        public int C3 { get; set; }

        public Service5(IGameLog log, TypeRepository rep)
        { }

        [TurnAction("A1")]
        public void A1()
        { }

        [TurnAction("A2")]
        public void A2(string arg)
        { }

        [TurnAction("A3")]
        public void A3(int arg)
        { }
    }
}
#pragma warning restore IDE0060
#pragma warning restore CA1822
#pragma warning restore IDE0079
