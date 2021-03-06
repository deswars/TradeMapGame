using System;
using System.Collections.Generic;

namespace TradeMap.GameLog
{
    public interface IGameLog
    {
        IReadOnlyList<ILogEntry> Entries { get; }
        void AddEntry(InfoLevels entryLevel, Func<ILogEntry> entry);
        void Flush();
        void SetInfoLevel(InfoLevels includedLevel);
    }
}
