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
        [JsonPropertyName("Meta Data")]
        public MetaData MetaData { get; set; }

        [JsonPropertyName("Time Series (5min)")]
        public Dictionary<string, IntradayTimeSeries> TimeSeries { get; set; }
    }

    public class MetaData
    {
        [JsonPropertyName("1. Information")]
        public string Information { get; set; }

        [JsonPropertyName("2. Symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("3. Last Refreshed")]
        public string LastRefreshed { get; set; }

        [JsonPropertyName("4. Interval")]
        public string Interval { get; set; }

        [JsonPropertyName("5. Output Size")]
        public string OutputSize { get; set; }

        [JsonPropertyName("6. Time Zone")]
        public string TimeZone { get; set; }
    }

    public class IntradayTimeSeries
    {
        [JsonPropertyName("1. open")]
        public decimal Open { get; set; }

        [JsonPropertyName("2. high")]
        public decimal High { get; set; }

        [JsonPropertyName("3. low")]
        public decimal Low { get; set; }

        [JsonPropertyName("4. close")]
        public decimal Close { get; set; }

        [JsonPropertyName("5. volume")]
        public int Volume { get; set; }
    }
}
