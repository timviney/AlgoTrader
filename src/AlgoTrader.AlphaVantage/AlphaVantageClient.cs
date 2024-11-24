using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AlgoTrader.Common;
using Microsoft.AspNetCore.WebUtilities;

namespace AlgoTrader.AlphaVantage
{
    public class AlphaVantageClient : IDisposable
    {
        private readonly HttpClient _httpClient = new();

        /// <summary>
        /// Gets intraday stock data for a specified symbol and interval.
        /// </summary>
        /// <param name="symbol">Stock symbol, e.g., "AAPL".</param>
        /// <param name="interval">Time interval, e.g., "5min".</param>
        /// <param name="from">Start of data</param>
        /// <param name="to">End of data (inclusive)</param>
        /// <returns>Returns JSON string with the time series data.</returns>
        public async Task<Dictionary<DateTime, IntradayTimeSeries>> GetIntradayDataAsync(Symbol symbol, Interval interval, DateTime from, DateTime to)
        {
            var fromMonth = new DateTime(from.Year, from.Month, 1);
            var toMonth = new DateTime(to.Year, to.Month, 1);

            var months = new List<DateTime>();
            for (var i = fromMonth; i <= toMonth; i = i.AddMonths(1))
            {
                months.Add(i);
            }

            var results = new Dictionary<DateTime, IntradayTimeSeries>();

            foreach (var month in months)
            {
                string monthStr = $"{month:yyyy-MM}";

                var url = QueryHelpers.AddQueryString(Settings.BaseUrl, new Dictionary<string, string?>()
                {
                    {"function", "TIME_SERIES_INTRADAY"},
                    {"symbol", symbol.ToString()},
                    {"interval", interval.AsString()},
                    {"apikey", Settings.Key},
                    {"month", monthStr},
                    {"outputsize", "full"},
                });

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var monthResults = Serialiser.Deserialise(content);

                foreach (var (dateTime, value) in monthResults.TimeSeries)
                {
                    if (dateTime < from || dateTime > to) continue;

                    results.Add(dateTime, value);
                }
            }
            //var url = $"{Settings.BaseUrl}?function=TIME_SERIES_INTRADAY&symbol={symbol.ToString()}&interval={interval.AsString()}&apikey={Settings.Key}";

            return results.Reverse().ToDictionary();
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
