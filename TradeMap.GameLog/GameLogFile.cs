using System;
using System.Collections.Generic;
using System.IO;

namespace TradeMap.GameLog
{
    public class GameLogFile : IGameLog
    {
        public IReadOnlyList<ILogEntry> Entries { get { return _entries; } }


        private readonly List<ILogEntry> _entries = new();
        private InfoLevels _includedLevels = InfoLevels.None;
        private readonly string _file;
        private readonly string _logName;


        public GameLogFile(string logName, string file, InfoLevels infoLevel)
        {
            _includedLevels = infoLevel;
            _file = file;
            _logName = logName;
        }


        public void AddEntry(InfoLevels entryLevel, Func<ILogEntry> entry)
        {
            if ((entryLevel & _includedLevels) != InfoLevels.None)
            {
                var newEntry = entry();
                newEntry.InfoLevel = entryLevel;
                _entries.Add(newEntry);
                File.AppendAllText(_file, $"{DateTime.Now}:{_logName}:{newEntry}");
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
