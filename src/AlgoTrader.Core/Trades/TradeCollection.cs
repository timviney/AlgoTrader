using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;

namespace AlgoTrader.Core.Trades
{
    internal class TradeCollection : IEnumerable<Trade>
    {
        // Assumes no parallel access - will need to update if multithreading is introduced

        private readonly Dictionary<Symbol, List<Trade>> _bySymbol = new();
        private readonly List<Trade> _byId = new();

        public void Record(Trade trade)
        {
            trade.Id = _byId.Count;
            if (!_bySymbol.TryGetValue(trade.Symbol, out var trades))
            {
                trades = new List<Trade>(1);
                _bySymbol[trade.Symbol] = trades;
            }

            trades.Add(trade);
            _byId.Add(trade);
        }

        public List<Trade>? this[Symbol symbol] => _bySymbol.GetValueOrDefault(symbol, null);
        public Trade? this[int id] => _byId.Count > id ? null : _byId[id];
        public int Count => _byId.Count;

        public IEnumerator<Trade> GetEnumerator()
        {
            return _byId.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
