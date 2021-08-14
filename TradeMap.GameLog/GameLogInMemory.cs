using System;
using System.Collections.Generic;

namespace TradeMap.GameLog
{
    public class GameLogInMemory : IGameLog
    {
        public IReadOnlyList<ILogEntry> Entries { get { return _entries; } }


        private readonly List<ILogEntry> _entries = new();
        private InfoLevels _includedLevels = InfoLevels.None;


        public void AddEntry(InfoLevels entryLevel, Func<ILogEntry> entry)
        {
            if ((entryLevel & _includedLevels) != InfoLevels.None)
            {
                var newEntry = entry();
                newEntry.InfoLevel = entryLevel;
                _entries.Add(newEntry);
            }
        }

        public void Flush()
        {
            _entries.Clear();
        }

        public void SetInfoLevel(InfoLevels includedLevel)
        {
            _includedLevels = includedLevel;
        }
    }
}
