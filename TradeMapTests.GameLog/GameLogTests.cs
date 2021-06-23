using Moq;
using System.Linq;
using TradeMap.GameLog;
using Xunit;

namespace TradeMapTests.GameLog
{
    public class GameLogTests
    {
        [Fact()]
        public void AddEntryTest()
        {
            var logEntry1 = new Mock<ILogEntry>().SetupAllProperties();
            var logEntry2 = new Mock<ILogEntry>().SetupAllProperties();
            var logEntry3 = new Mock<ILogEntry>().SetupAllProperties();
            GameLogImpl log = new();
            InfoLevels level = InfoLevels.Error | InfoLevels.Warning;
            log.SetInfoLevel(level);
            bool isEntryCreated;

            Assert.Empty(log.Entries);

            isEntryCreated = false;
            log.AddEntry(InfoLevels.Error, () => { isEntryCreated = true; return logEntry1.Object; });
            Assert.Single(log.Entries);
            Assert.True(isEntryCreated);
            Assert.Equal(InfoLevels.Error, logEntry1.Object.InfoLevel);

            isEntryCreated = false;
            log.AddEntry(InfoLevels.Warning, () => { isEntryCreated = true; return logEntry2.Object; });
            Assert.Equal(2, log.Entries.Count());
            Assert.True(isEntryCreated);
            Assert.Equal(InfoLevels.Warning, logEntry2.Object.InfoLevel);

            isEntryCreated = false;
            log.AddEntry(InfoLevels.Info, () => { isEntryCreated = true; return logEntry3.Object; });
            Assert.Equal(2, log.Entries.Count());
            Assert.False(isEntryCreated);
        }

        [Fact()]
        public void FlushTest()
        {
            var logEntry1 = new Mock<ILogEntry>().SetupAllProperties();
            var logEntry2 = new Mock<ILogEntry>().SetupAllProperties();
            var logEntry3 = new Mock<ILogEntry>().SetupAllProperties();
            GameLogImpl log = new();
            InfoLevels level = InfoLevels.Error;
            log.SetInfoLevel(level);

            log.AddEntry(InfoLevels.Error, () => logEntry1.Object);
            log.AddEntry(InfoLevels.Error, () => logEntry2.Object);
            Assert.Equal(2, log.Entries.Count());

            log.Flush();
            Assert.Empty(log.Entries);

            log.AddEntry(InfoLevels.Error, () => logEntry3.Object);
            Assert.Single(log.Entries);
        }
    }
}