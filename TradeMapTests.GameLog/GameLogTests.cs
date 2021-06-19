using Xunit;
using TradeMap.GameLog;
using System.Linq;
using Moq;

namespace TradeMapTests.GameLog
{
    public class GameLogTests
    {
        [Fact()]
        public void AddEntryTest()
        {
            var logEntry1 = Mock.Of<ILogEntry>(MockBehavior.Strict);
            var logEntry2 = Mock.Of<ILogEntry>(MockBehavior.Strict);
            var logEntry3 = Mock.Of<ILogEntry>(MockBehavior.Strict);
            GameLogImpl log = new();
            InfoLevels level = InfoLevels.Error | InfoLevels.Warning;
            log.SetInfoLevel(level);
            bool isEntryCreated;

            Assert.Empty(log.Entries);

            isEntryCreated = false;
            log.AddEntry(InfoLevels.Error , () => { isEntryCreated = true; return logEntry1; });
            Assert.Single(log.Entries);
            Assert.True(isEntryCreated);

            isEntryCreated = false;
            log.AddEntry(InfoLevels.Info | InfoLevels.Warning, () => { isEntryCreated = true; return logEntry2; });
            Assert.Equal(2, log.Entries.Count());
            Assert.True(isEntryCreated);

            isEntryCreated = false;
            log.AddEntry(InfoLevels.Info, () => { isEntryCreated = true; return logEntry3; });
            Assert.Equal(2, log.Entries.Count());
            Assert.False(isEntryCreated);
        }

        [Fact()]
        public void FlushTest()
        {
            var logEntry1 = Mock.Of<ILogEntry>(MockBehavior.Strict);
            var logEntry2 = Mock.Of<ILogEntry>(MockBehavior.Strict);
            var logEntry3 = Mock.Of<ILogEntry>(MockBehavior.Strict);
            GameLogImpl log = new();
            InfoLevels level = InfoLevels.Error;
            log.SetInfoLevel(level);

            log.AddEntry(InfoLevels.Error, () => logEntry1);
            log.AddEntry(InfoLevels.Error, () => logEntry2);
            Assert.Equal(2, log.Entries.Count());

            log.Flush();
            Assert.Empty(log.Entries);

            log.AddEntry(InfoLevels.Error, () => logEntry3);
            Assert.Single(log.Entries);
        }
    }
}