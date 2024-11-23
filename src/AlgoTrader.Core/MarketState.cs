using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrader.Core
{
    internal class MarketState
    {
        public List<MarketDataPoint> Data { get; } = new();
        
        public MarketDataPoint? Current { get; private set; }
        
        public void Update(MarketDataPoint dataPoint)
        {
            Current = dataPoint;
            Data.Add(dataPoint);
        }
    }
}
