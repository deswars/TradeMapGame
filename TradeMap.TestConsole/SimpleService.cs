using TradeMap.Core.Action;
using TradeMap.Core.Map.ReadOnly;
using TradeMap.Di.Attributes;
using TradeMap.Di.Parsers;
using TradeMap.Di.Validators;
using TradeMap.GameLog;

namespace TradeMap.TestConsole
{
    [Service("SimpleService")]
    public class SimpleService
    {
        [Constant("Const1", typeof(ParserString), new string[0])]
        public string Const1 { get; set; } = "a";

        [Constant("Const2", typeof(ParserInt), new string[0], typeof(ValidatorIntMinMax), new string[4] { "min", "0", "max", "100" })]
        public int Const2 { get; set; } = 0;

        [Constant("Const3", typeof(ParserDouble), new string[0], typeof(ValidatorDoubleMinMax), new string[4] { "min", "0", "max", "100" })]
        public double Const3 { get; set; } = 10;

        [Constant("Const4", typeof(ParserString), new string[0], typeof(ValidatorId), new string[1] { "resource" })]
        public string Const4 { get; set; } = "";


        private readonly IGameLog _log;


        public SimpleService(IGameLog log)
        {
            _log = log;
        }


        [Feautre("EmptyFeautre")]
        public IGlobalAction? GlobalSubstep(int turn, IMap map)
        {
            _log.AddEntry(InfoLevels.Info, () => new BaseLogEntry($"{turn}+{map.Height}+{map.Width}", null));
            return null;
        }
    }
}
