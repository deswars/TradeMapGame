using System;
using System.Runtime.Serialization;

namespace TradeMap.GameLog
{
    [Flags]
    public enum InfoLevels
    {
        [EnumMember(Value = "None")]
        None = 0,

        [EnumMember(Value = "Info")]
        Info = 1,

        [EnumMember(Value = "Warn")]
        Warning = 2,

        [EnumMember(Value = "Erro")]
        Error = 4,

        [EnumMember(Value = "All")]
        All = 0xFFFF
    }
}
