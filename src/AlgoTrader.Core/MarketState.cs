using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrader.Core
{
    internal class MarketState
    {
        private readonly List<MarketDataPoint> _data = new();
        
        public MarketDataPoint? Current { get; private set; }
        
        public void Update(MarketDataPoint dataPoint)
        {
            Current = dataPoint;
            _data.Add(dataPoint);
        }

        public IEnumerable<MarketDataPoint> GetData(DateTime startTime, DateTime endTime)
        {
            return _data.Where(dp => dp.DateTime >= startTime && dp.DateTime <= endTime);
        }
    }
}
