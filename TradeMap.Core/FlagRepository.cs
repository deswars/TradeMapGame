using System.Collections.Generic;

namespace TradeMap.Core
{
    public class FlagRepository : IReadOnlyFlagRepository
    {
        private Dictionary<string, string>? _flagsStr;
        private Dictionary<string, int>? _flagsInt;
        private Dictionary<string, double>? _flagsDouble;


        public void ClearFlags()
        {
            _flagsStr = null;
            _flagsInt = null;
            _flagsDouble = null;
        }


        public bool HasFlagStr(string name)
        {
            if (_flagsStr == null)
            {
                return false;
            }
            return _flagsStr.ContainsKey(name);
        }

        public bool TryGetFlagStr(string name, out string value)
        {
            if (_flagsStr == null)
            {
                value = "";
                return false;
            }
            bool result = _flagsStr.TryGetValue(name, out string? outStr);
            value = outStr ?? "";
            return result;
        }

        public string GetFlagStr(string name)
        {
            if (_flagsStr == null)
            {
                throw new KeyNotFoundException();
            }
            return _flagsStr[name];
        }

        public void SetFlagStr(string name, string value)
        {
            if (_flagsStr == null)
            {
                _flagsStr = new();
            }
            _flagsStr[name] = value;
        }

        public void RemoveFlagStr(string name)
        {
            if (!(_flagsStr == null))
            {
                _flagsStr.Remove(name);
            }
        }


        public bool HasFlagInt(string name)
        {
            if (_flagsInt == null)
            {
                return false;
            }
            return _flagsInt.ContainsKey(name);
        }

        public bool TryGetFlagInt(string name, out int value)
        {
            if (_flagsInt == null)
            {
                value = 0;
                return false;
            }
            return _flagsInt.TryGetValue(name, out value);
        }

        public int GetFlagInt(string name)
        {
            if (_flagsInt == null)
            {
                throw new KeyNotFoundException();
            }
            return _flagsInt[name];
        }

        public void SetFlagInt(string name, int value)
        {
            if (_flagsInt == null)
            {
                _flagsInt = new();
            }
            _flagsInt[name] = value;
        }

        public void RemoveFlagInt(string name)
        {
            if (!(_flagsInt == null))
            {
                _flagsInt.Remove(name);
            }
        }


        public bool HasFlagDouble(string name)
        {
            if (_flagsDouble == null)
            {
                return false;
            }
            return _flagsDouble.ContainsKey(name);
        }

        public bool TryGetFlagDouble(string name, out double value)
        {
            if (_flagsDouble == null)
            {
                value = 0;
                return false;
            }
            return _flagsDouble.TryGetValue(name, out value);
        }

        public double GetFlagDouble(string name)
        {
            if (_flagsDouble == null)
            {
                throw new KeyNotFoundException();
            }
            return _flagsDouble[name];
        }

        public void SetFlagDouble(string name, double value)
        {
            if (_flagsDouble == null)
            {
                _flagsDouble = new();
            }
            _flagsDouble[name] = value;
        }

        public void RemoveFlagDouble(string name)
        {
            if (!(_flagsDouble == null))
            {
                _flagsDouble.Remove(name);
            }
        }
    }
}
