using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlgoTrader.AlphaVantage
{
    public class IntradayDataResponse
    {
        public MetaData MetaData { get; set; }

        public Dictionary<DateTime, IntradayTimeSeries> TimeSeries { get; set; }
    }

    public class MetaData
    {
        public string Information { get; set; }

        public Symbol Symbol { get; set; }

        public DateTime LastRefreshed { get; set; }

        public Interval Interval { get; set; }

        public string OutputSize { get; set; }

        public string TimeZone { get; set; }
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