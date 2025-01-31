using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;

namespace AlgoTrader.Core.MarketData
{
    internal class MarketState
    {
        public List<MarketDataPoint> Data { get; } = new();

        public CurrentPrice Current { get; private set; }

        public void Update(CurrentPrice currentPrice, MarketDataPoint previousPeriod)
        {
            Current = currentPrice;
            Data.Add(previousPeriod);
        }

        public int NumberOfRecordedPeriods => Data.Count;

        public decimal PriceAverage(int lastNPeriods)
        {
            var data = LastNPeriods(lastNPeriods);

            return data.Average(d => d.Close);
        }

        public List<MarketDataPoint> LastNPeriods(int lastNPeriods)
        {
            int len = Data.Count;
            var data = Data[(len - lastNPeriods)..];
            return data;
        }

        public decimal CurrentPriceWithSlippage(TradeDirection direction, decimal slippage)
        {
            return Current.Price * (1 + slippage * direction.Multiplier());
        }
    }
}
