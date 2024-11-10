using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AlgoTrader.Common;

namespace AlgoTrader.AlphaVantage
{
    public class IntradayDataResponse
    {
        public required MetaData MetaData { get; set; }

        public required Dictionary<DateTime, IntradayTimeSeries> TimeSeries { get; set; }
    }

    public class MetaData
    {
        public required string Information { get; set; }

        public required Symbol Symbol { get; set; }

        public required DateTime LastRefreshed { get; set; }

        public required Interval Interval { get; set; }

        public required string OutputSize { get; set; }

        public required string TimeZone { get; set; }
    }

    public class IntradayTimeSeries
    {
        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public int Volume { get; set; }
    }
}