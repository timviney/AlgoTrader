using AlgoTrader.Common;
using AlgoTrader.ConsoleApp;
using AlgoTrader.Core.BollingerBands;
using AlgoTrader.Core.MovingAverageCrossover;
using AlgoTrader.Core.Trades;

Console.WriteLine("Running historics...");

var results = await AlgoTrader.Historic.Engine.Engine.Run(
    new DateTime(2024, 1, 1),
    new DateTime(2025, 1, 31),
    new TradingInputs(Symbol.AMZN, 0.002m),
    new InputsBollingerBands(150, 2, 50000, 100, 100));

TradingMetricsVisual.DisplayVisualMetrics(results);