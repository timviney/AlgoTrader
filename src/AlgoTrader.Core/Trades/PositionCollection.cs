using AlgoTrader.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrader.Core.Trades
{
    internal class PositionCollection : IEnumerable<Position>
    {
        // Assumes no parallel access - will need to update if multithreading is introduced

        private readonly Dictionary<Symbol, List<Position>> _bySymbol = new();
        private readonly List<Position> _byId = new();

        public void Record(Position position)
        {
            position.Id = _byId.Count;
            _byId.Add(position);
            if (!_bySymbol.TryGetValue(position.Symbol, out var positions))
            {
                positions = new List<Position>(1);
                _bySymbol[position.Symbol] = positions;
            }
            positions.Add(position);
        }

        public List<Position>? this[Symbol symbol] => _bySymbol.GetValueOrDefault(symbol, null);
        public Position? this[int id] => _byId.Count > id ? null : _byId[id];
        public int Count => _byId.Count;

        public List<Position>? GetOpenPositionsOrNull(Symbol symbol)
        {
            if (!_bySymbol.TryGetValue(symbol, out var positions)) return null;

            var openPositions = new List<Position>();
            for (int i = positions.Count - 1; i >= 0; i--)
            {
                var position = positions[i];
                if (position.Status == PositionStatus.Closed) break;
                openPositions.Add(position);
            }

            openPositions.Reverse();

            return openPositions.Any() ? openPositions : null;
        }

        public IEnumerator<Position> GetEnumerator()
        {
            return _byId.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
