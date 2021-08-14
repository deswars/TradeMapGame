using System.Collections.Generic;
using System.Windows.Controls;

namespace TradeMap.Gui
{
    public class GfxRepository
    {
        public Image Empty16 { get; set; } = new();
        public Image Empty32 { get; set; } = new();
        public Dictionary<string, Image> Resources { get; set; } = new();
        public Dictionary<string, Image> Terrains { get; set; } = new();
    }
}
