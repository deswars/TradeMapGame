using System;
using System.Linq;
using TradeMap.Di;
using TradeMap.Di.GameLogEntry;
using TradeMap.GameLog;
using TradeMapTests.Di.Stubs;
using Xunit;

namespace TradeMapTests.Di
{
    public class MangerTests
    {
        [Fact()]
        public void ManagerTest()
        {
            GameLogImpl log = new();
            log.SetInfoLevel(InfoLevels.All);
            GameLogImpl gameLog = new();
            TypeRepository repository = new();
            Manager<EventHolder, TypeRepository> manager = new(log, gameLog, repository);

            var actionList = manager.GetPossibleActions();
            Assert.Equal(3, actionList.Count);
            Assert.Equal(typeof(EventHolder).GetEvent("E1"), actionList["E1"]);
            Assert.Equal(typeof(EventHolder).GetEvent("E2"), actionList["E2"]);
            Assert.Equal(typeof(EventHolder).GetEvent("E3"), actionList["E3"]);
        }

        [Fact()]
        public void CollectAllAvailableServicesTest()
        {
            GameLogImpl log = new();
            log.SetInfoLevel(InfoLevels.All);
            GameLogImpl gameLog = new();
            TypeRepository repository = new();
            Manager<EventHolder, TypeRepository> manager = new(log, gameLog, repository);

            var list = manager.CollectAllAvailableServices();
            Assert.Equal(3, list.Count);
            Assert.True(list.ContainsKey("S1"));
            Assert.False(list.ContainsKey("S2"));
            Assert.False(list.ContainsKey("S3"));
            Assert.True(list.ContainsKey("S5"));
            Assert.True(list.ContainsKey("S6"));

            Assert.Equal(3, log.Entries.Count());
            Assert.Single(log.Entries.Where(x => x.GetType() == typeof(LogEntryServiceOverride)));
            Assert.Single(log.Entries.Where(x => x.GetType() == typeof(LogEntryActionlessService)));
            Assert.Single(log.Entries.Where(x => x.GetType() == typeof(LogEntryDefaultValueConstraintError)));
        }

        [Fact()]
        public void RegisterTurnActionTest()
        {
            GameLogImpl log = new();
            log.SetInfoLevel(InfoLevels.All);
            GameLogImpl gameLog = new();
            TypeRepository repository = new();
            Manager<EventHolder, TypeRepository> manager = new(log, gameLog, repository);
            manager.CollectAllAvailableServices();

            Assert.True(manager.TryRegisterTurnAction("E1", "S1", "A1"));
            Assert.True(manager.TryRegisterTurnAction("E2", "S6", "A2"));
            Assert.True(manager.TryRegisterTurnAction("E2", "S5", "A2"));
            Assert.False(manager.TryRegisterTurnAction("E2", "S1", "A3"));
            Assert.False(manager.TryRegisterTurnAction("error", "S1", "A1"));
            Assert.False(manager.TryRegisterTurnAction("E1", "error", "A1"));
            Assert.False(manager.TryRegisterTurnAction("E1", "S1", "error"));
        }

        [Fact()]
        public void CollectConstantDemandsTest()
        {
            GameLogImpl log = new();
            log.SetInfoLevel(InfoLevels.All);
            GameLogImpl gameLog = new();
            TypeRepository repository = new();
            Manager<EventHolder, TypeRepository> manager = new(log, gameLog, repository);
            manager.CollectAllAvailableServices();
            manager.TryRegisterTurnAction("E1", "S1", "A1");
            manager.TryRegisterTurnAction("E2", "S6", "A2");

            var constants = manager.CollectConstantDemands();
            Assert.Equal(2, constants.Count);
            Assert.True(constants.ContainsKey("S1"));
            Assert.True(constants.ContainsKey("S6"));
            Assert.Equal(3, constants["S1"].Count);
            Assert.Equal(3, constants["S6"].Count);
        }

        [Fact()]
        public void CreateServicesAndSubscribeActionsTest()
        {
            GameLogImpl log = new();
            log.SetInfoLevel(InfoLevels.All);
            GameLogImpl gameLog = new();
            TypeRepository repository = new();
            Manager<EventHolder, TypeRepository> manager = new(log, gameLog, repository);
            manager.CollectAllAvailableServices();
            manager.TryRegisterTurnAction("E1", "S1", "A1");
            manager.TryRegisterTurnAction("E1", "S6", "A1");
            manager.TryRegisterTurnAction("E2", "S6", "A2");
            var constants = manager.CollectConstantDemands();

            constants["S1"]["C1"].Value = "abc";
            constants["S1"]["C3"].Value = "10";

            EventHolder eh = new();
            manager.CreateServicesAndSubscribeActions(eh, constants);

            Action e1 = eh.GetE1();
            Assert.Equal(2, e1.GetInvocationList().Length);
            Action<string> e2 = eh.GetE2();
            Assert.Single(e2.GetInvocationList());
            Action e3 = eh.GetE3();
            Assert.Null(e3);
        }
    }
}