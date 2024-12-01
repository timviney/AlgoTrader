using System;
using System.Linq.Expressions;
using AlgoTrader.AlphaVantage;
using AlgoTrader.Common;
using AlgoTrader.Core;

namespace AlgoTrader.Historic.Engine
{
    public static class Engine
    {
        private const bool UseAlphaVantage = false; // Replaced with DB from public data as API is too limiting :(

        public static async Task<TradingResults> Run(DateTime from, DateTime to, TradingInputs tradingInputs, IStrategyInputs strategyInputs)
        {
            var strategy = StrategyFactory.Get(tradingInputs, strategyInputs);

            var historicIntradayData = await GetHistoricIntradayData(from, to, tradingInputs);

            foreach ((DateTime dateTime, MarketDataPoint data) in historicIntradayData)
            {
                strategy.NextPeriod(data);
            }

            strategy.End();

            return strategy.GetResults();
        }

        private static async Task<Dictionary<DateTime, MarketDataPoint>> GetHistoricIntradayData(DateTime from, DateTime to, TradingInputs tradingInputs)
        {
            if (UseAlphaVantage)
            {
                using var dataClient = new AlphaVantageClient();

                var historicIntradayData = await dataClient.GetIntradayDataAsync(tradingInputs.Symbol, Interval._60min, from, to);
                return historicIntradayData.Select(pair => new KeyValuePair<DateTime, MarketDataPoint>(
                        pair.Key,
                        new MarketDataPoint(pair.Key, pair.Value.Open, pair.Value.High, pair.Value.Low,
                            pair.Value.Close, pair.Value.Volume)))
                    .ToDictionary();
            }
            else
            {
                using var downloader = new StockData.Downloader();

                var data = await downloader.GetDataAsync(tradingInputs.Symbol.ToString(), Interval._60min.AsString(), from, to);
                return data.Select(pair => new KeyValuePair<DateTime, MarketDataPoint>(
                        pair.Key,
                        new MarketDataPoint(pair.Key, pair.Value.Open, pair.Value.High, pair.Value.Low,
                            pair.Value.Close, pair.Value.Volume)))
                    .ToDictionary();
            }
        }
    }
}
