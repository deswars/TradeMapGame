using System;

namespace TradeMap.GameLog
{
    [Flags]
    public enum InfoLevels
    {
        None = 0,
        Info = 1,
        Warning = 2,
        Error = 4
    }
}
