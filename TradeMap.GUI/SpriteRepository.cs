using System.Collections.Generic;
using System.Windows.Controls;

namespace TradeMap.GUI
{
    public class SpriteRepository
    {
        public Image DefaultSettlement { get; set; } = new();
        public Image EmptySprite { get; set; } = new();
        public Image EmptyIcon { get; set; } = new();

#pragma warning disable CS8618
        public IReadOnlyDictionary<string, Image> Terrain { get; set; }
        public IReadOnlyDictionary<string, Image> MapFeautre { get; set; }
        public IReadOnlyDictionary<string, Image> Collector { get; set; }
        public IReadOnlyDictionary<string, Image> Building { get; set; }
#pragma warning restore CS8618
    }
}
