using System.Collections.Generic;

namespace TradeMapGame.TurnLog
{
    public class TurnLogImpl
    {
        public IEnumerable<ILogEntry> Entries { get { return _entries; } }
        public void AddEntry(ILogEntry entry)
        {
            _entries.Add(entry);
        }
        public void Flush()
        {
            _entries.Clear();
        }

        private readonly List<ILogEntry> _entries = new();
    }
}
