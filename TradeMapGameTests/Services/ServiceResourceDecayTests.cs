using System.Collections.Generic;
using System.Linq;
using TradeMapGame.Map;
using TradeMapGame.TurnLog;
using Xunit;

namespace TradeMapGame.Services.Tests
{
    public class ServiceResourceDecayTests
    {
        [Fact()]
        public void DecayResourcesTest()
        {
            TurnLogImpl log = new();
            ServiceResourceDecay serv = new(log);
            ResourceType res1 = new("r1", 1, 0.1);
            double res1Start = 10;
            ResourceType res2 = new("r2", 1, 0.2);
            double res2Start = 20;
            KeyedVector<ResourceType> stored = new(new List<ResourceType> { res1, res2 }) { [res1] = res1Start, [res2] = res2Start };
            Settlement settl = new("s", new Point(1, 1), 1, stored);

            serv.DecayResources(1, settl);

            double res1End = res1Start - res1Start * res1.DecayRate;
            Assert.Equal(res1End, settl.Resources[res1], 2);
            double res2End = res2Start - res2Start * res2.DecayRate;
            Assert.Equal(res2End, settl.Resources[res2], 2);

            Assert.Single(log.Entries);
            Assert.Equal(typeof(LogEntryDecay), log.Entries.First().GetType());
        }
    }
}