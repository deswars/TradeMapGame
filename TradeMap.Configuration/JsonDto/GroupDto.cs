using System.Collections.Generic;

namespace TradeMap.Configuration.JsonDto
{
    class GroupDto<DataType>
    {
        public List<string>? IfDef { get; set; }
        public List<string>? IfNotDef { get; set; }
        public List<DataType> Include { get; set; } = new();
    }
}
