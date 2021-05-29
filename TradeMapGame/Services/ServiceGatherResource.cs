using TradeMapGame.Log;
using TradeMapGame.Map;

namespace TradeMapGame.Services
{
    public class ServiceGatherResource
    {
        public ServiceGatherResource(TurnLog? log)
        {
            _log = log;
        }

        public void CollectorsGatherResources(int turn, Settlement settlement)
        {
            var delta = settlement.Resources.Zeroed();

            foreach (var collector in settlement.Collectors)
            {
                CollectorGather(collector, delta);
            }
            settlement.Resources.Add(delta);
            if (_log != null)
            {
                _log.AddEntry(new LogEntryCollect(turn, settlement, delta));
            }
        }

        public void BuildingsProduceResources(int turn, Settlement settlement)
        {
            var delta = settlement.Resources.Zeroed();

            foreach (var building in settlement.Buildings)
            {
                BuildingGetTotalConsumption(building, delta);
            }
            if (delta.IsSmaller(settlement.Resources))
            {
                settlement.Resources.Sub(delta);
                if (_log != null)
                {
                    _log.AddEntry(new LogEntryProduce(turn, settlement, delta));
                }
            }
            else
            {
                if (_log != null)
                {
                    _log.AddEntry(new LogEntryProduce(turn, settlement, delta.Zeroed()));
                }
            }
        }

        private static void CollectorGather(Collector coll, KeyedVector<ResourceType> delta)
        {
            var cur = delta.Zeroed();
            var location = coll.Location;
            foreach (var deposit in location.Terrain.Resources)
            {
                cur[deposit.Type] += deposit.Richness;
            }
            foreach (var feautre in location.MapFeautres)
            {
                foreach (var deposit in feautre.Resources)
                {
                    cur[deposit.Type] += deposit.Richness;
                }
            }
            foreach (var res in cur)
            {
                if (res.Value < 0)
                {
                    cur[res.Key] = 0;
                }
            }
            delta.Add(cur);
        }

        private static void BuildingGetTotalConsumption(Building build, KeyedVector<ResourceType> delta)
        {
            delta.Add(build.Type.Input);
            delta.Sub(build.Type.Output);
        }

        private readonly TurnLog? _log;
    }
}
