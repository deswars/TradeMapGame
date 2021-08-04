using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeMap.Interfaces.Map.ReadOnly
{
    public interface ICollector
    {
        public ISettlement Owner { get; }
        public ICell Location { get; }
        public ICollectorType Type { get; }
        public double Power { get; }
        public int Level { get; }
    }
}
