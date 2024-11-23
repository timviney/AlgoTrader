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
        
        public MarketDataPoint Current { get; private set; }
        
        public void Update(MarketDataPoint dataPoint)
        {
            Current = dataPoint;
            Data.Add(dataPoint);
        }

        public int NumberOfRecordedPeriods => Data.Count;

        public decimal PriceAverage(int lastNPeriods)
        {
            int len = Data.Count;
            var data = Data[(len - lastNPeriods)..];

            return data.Average(d => d.Open);
        }

        public decimal OpeningPrice(TradeDirection direction, decimal slippage)
        {
            var price = Current.Open;
            return direction == TradeDirection.Buy
                ? price * (1 + slippage)
                : price * (1 - slippage);
        }
    }
}
