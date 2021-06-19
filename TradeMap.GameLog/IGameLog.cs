using System;
using System.Collections.Generic;

namespace TradeMap.GameLog
{
    public interface IGameLog
    {
        public IEnumerable<ILogEntry> Entries { get; }
        public void AddEntry(InfoLevels entryLevel, Func<ILogEntry> entry);
        public void Flush();
        public void SetInfoLevel(InfoLevels includedLevel);
    }
}
