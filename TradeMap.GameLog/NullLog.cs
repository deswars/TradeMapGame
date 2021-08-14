using System;
using System.Collections.Generic;

namespace TradeMap.GameLog
{
    public class NullLog : IGameLog
    {
        public IReadOnlyList<ILogEntry> Entries { get { return _entries; } }


        private readonly List<ILogEntry> _entries = new();


        public void AddEntry(InfoLevels entryLevel, Func<ILogEntry> entry)
        {
        }

        public void Flush()
        {
        }

        public void SetInfoLevel(InfoLevels includedLevel)
        {
        }
    }
}
