using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;

namespace AlgoTrader.Core
{
    internal class TradeList : IEnumerable<Trade>
    {
        private readonly Dictionary<Symbol, List<Trade>> _bySymbol = new();

        public void Add(Trade trade)
        {
            if (!_bySymbol.TryGetValue(trade.Symbol, out var trades))
            {
                trades = new List<Trade>(1);
                _bySymbol[trade.Symbol] = trades;
            }

            trades.Add(trade);
        }

        public List<Trade> this[Symbol symbol] => _bySymbol[symbol];
        
        public IEnumerator<Trade> GetEnumerator()
        {
            return _bySymbol.SelectMany(pair => pair.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
