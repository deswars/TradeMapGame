using System.Collections.Generic;

namespace TradeMap.Gui
{
    public class GfxConfig
    {
        public string Empty16 { get; set; } = "";
        public string Empty32 { get; set; } = "";
        public Dictionary<string, string> Resources { get; set; } = new();
        public Dictionary<string, string> Terrains { get; set; } = new();
    }
}
