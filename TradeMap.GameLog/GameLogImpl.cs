﻿using System;
using System.Collections.Generic;

namespace TradeMap.GameLog
{
    public class GameLogImpl : IGameLog
    {
        public IEnumerable<ILogEntry> Entries { get { return _entries; } }

        public void AddEntry(InfoLevels entryLevel, Func<ILogEntry> entry)
        {
            if ((entryLevel & _includedLevels) != InfoLevels.None)
            {
                _entries.Add(entry());
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

        private readonly List<ILogEntry> _entries = new();
        private InfoLevels _includedLevels = InfoLevels.None;
    }
}
