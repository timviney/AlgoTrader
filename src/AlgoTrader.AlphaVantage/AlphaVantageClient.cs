using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        /// <returns>Returns JSON string with the time series data.</returns>
        public async Task<IntradayDataResponse> GetIntradayDataAsync(Symbol symbol, Interval interval)
        {
            var url = $"{Settings.BaseUrl}?function=TIME_SERIES_INTRADAY&symbol={symbol.ToString()}&interval={interval.AsString()}&apikey={Settings.Key}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var intradayDataResponse = JsonSerializer.Deserialize<IntradayDataResponse>(content, Settings.JsonSerialiserSettings);

            return intradayDataResponse!;
        }

        /// <summary>
        /// Disposes the HttpClient instance.
        /// </summary>
        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
