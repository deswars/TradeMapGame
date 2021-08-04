namespace TradeMap.Configuration
{
    public class TurnActionInfo
    {
        public string EventName { get; }
        public string ServiceName { get; }
        public string ActionName { get; }

        public TurnActionInfo(string eventName, string serviceName, string actionName)
        {
            EventName = eventName;
            ServiceName = serviceName;
            ActionName = actionName;
        }
    }
}
