using AlgoTrader.Common;
using AlgoTrader.ConsoleApp;
using AlgoTrader.Core;

Console.WriteLine("Running historics...");

var results = await AlgoTrader.Historic.Engine.Engine.Run(
    new DateTime(2024, 1, 1),
    new DateTime(2024, 10, 23),
    new TradingInputs(Symbol.NFLX, 0.002m),
    new AlgoTrader.Core.MovingAverageCrossover.InputsMovingAverageCrossover(10, 50, 100, 100));

TradingMetricsVisual.DisplayVisualMetrics(results);