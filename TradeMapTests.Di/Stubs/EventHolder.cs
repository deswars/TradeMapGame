using System;

namespace TradeMapTests.Di.Stubs
{
    public class EventHolder
    {
        public event Action E1;
        public event Action<string> E2;
        public event Action E3;

        public Action GetE1()
        {
            return E1;
        }

        public Action<string> GetE2()
        {
            return E2;
        }

        public Action GetE3()
        {
            return E3;
        }
    }
}
