using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeMapGame
{
    public class TurnLog
    {
        public TurnLog()
        {
            _turn = 0;
            _data = new List<string>();
        }


        public List<string> GetLastTurnLog()
        {
            return _data;
        }

        public void SetTurn(int Turn)
        {
            _turn = Turn;
            _data.Clear();
        }

        public void Write(string Event)
        {
            _data.Add(_turn + " : " + Event);
        }

        public void StartEntry(string data)
        {
            _currentEntry = data;
        }

        public void ContinueEntry(string data)
        {
            _currentEntry += data;
        }

        public void FinishEntry(string data)
        {
            Write(_currentEntry + data);
            _currentEntry = "";
        }

        private int _turn;
        private List<string> _data;
        private string _currentEntry;
    }
}
