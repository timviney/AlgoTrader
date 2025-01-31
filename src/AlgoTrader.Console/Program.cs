using AlgoTrader.Common;
using AlgoTrader.ConsoleApp;
using AlgoTrader.Core.BollingerBands;
using AlgoTrader.Core.MovingAverageCrossover;
using AlgoTrader.Core.Trades;

Console.WriteLine("Running historics...");

var results = await AlgoTrader.Historic.Engine.Engine.Run(
    new DateTime(2024, 1, 1),
    new DateTime(2024, 10, 23),
    new TradingInputs(Symbol.TSLA, 0.002m),
    new InputsBollingerBands(100, 2, 100, 100));

TradingMetricsVisual.DisplayVisualMetrics(results);