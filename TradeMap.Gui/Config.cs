using System.Collections.Generic;

namespace TradeMap.Gui
{
    public class Config
    {
        public string Feautres { get; set; } = "";
        public List<string> Packages { get; set; } = new();
        public string Log { get; set; } = "";
        public string Localization { get; set; } = "eng.loc";
        public int SubstepCount { get; set; } = 1;
    }
}
