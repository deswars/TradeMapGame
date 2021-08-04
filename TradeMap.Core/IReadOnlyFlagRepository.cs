using System.Collections.Generic;

namespace TradeMap.Core
{
    public interface IReadOnlyFlagRepository
    {
        bool HasFlagStr(string name);
        bool TryGetFlagStr(string name, out string value);
        string GetFlagStr(string name);
        
        bool HasFlagInt(string name);
        bool TryGetFlagInt(string name, out int value);
        int GetFlagInt(string name);

        bool HasFlagDouble(string name);
        bool TryGetFlagDouble(string name, out double value);
        double GetFlagDouble(string name);
    }
}