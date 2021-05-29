using TradeMapGame.Log;
using TradeMapGame.Map;

namespace TradeMapGame.Services
{
    public class ServiceResourceDecay
    {
        public ServiceResourceDecay(TurnLog? log)
        {
            _log = log;
        }

        public void DecayResources(int turn, Settlement settlement)
        {
            KeyedVector<ResourceType>? delta = null;
            if (_log != null)
            {
                delta = settlement.Resources.Zeroed();
            }
            foreach (var res in settlement.Resources)
            {
                if (delta != null)
                {
                    delta[res.Key] = res.Key.DecayRate * res.Value;
                }
                settlement.Resources[res.Key] = (1 - res.Key.DecayRate) * res.Value;
            }
            if ((_log != null) && (delta != null))
            {
                _log.AddEntry(new LogEntryDecay(turn, settlement, delta));
            }
        }

        private readonly TurnLog? _log;
    }
}
