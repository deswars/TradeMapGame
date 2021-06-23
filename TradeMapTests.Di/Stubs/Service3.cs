#pragma warning disable IDE0079
#pragma warning disable IDE0060
#pragma warning disable CA1822
using TradeMap.Di.Attributes;
using TradeMap.Di.Constraints;
using TradeMap.GameLog;

namespace TradeMapTests.Di.Stubs
{
    [Service("S3")]
    public class Service3
    {
        [Constant("a", "C1")]
        public string C1 { get; set; }

        [Constant("b", "C2")]
        public string C2 { get; set; }

        [Constant("2", "C3", typeof(ConstraintInt), "")]
        public string C3 { get; set; }

        public Service3(IGameLog log, TypeRepository rep)
        { }
    }
}
#pragma warning restore IDE0060
#pragma warning restore CA1822
#pragma warning restore IDE0079
