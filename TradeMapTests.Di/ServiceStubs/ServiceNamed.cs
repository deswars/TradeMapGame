using TradeMap.Di;
using TradeMap.Di.Attributes;

namespace TradeMapTests.Di.ServiceStubs
{
    [Service("SN")]
    public class ServiceNamed
    {
        [Constant("1")]
        public int C1 { get; set; }

        [Constant("a")]
        public int Cerror { get; set; }

        [Constant("2", "C-N")]
        public int CNamed1 { get; set; }

        [Constant("3", "SN-C-NF", true)]
        public int CNamed2 { get; set; }

        [Constant("4", "C-N-TII", ValueTypes.Int)]
        public int CTyped1 { get; set; }

        [Constant("10", "C-N-TError", ValueTypes.String)]
        public int CTyped3 { get; set; }

        [Constant("1", "Common", true, ValueTypes.String)]
        public string CCommon { get; set; }

        [Constant("2", "CommonD", true, ValueTypes.String)]
        public string CCommonD { get; set; }
    }
}
