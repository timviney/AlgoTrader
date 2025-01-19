using AlgoTrader.Common;
using AlgoTrader.ConsoleApp;
using AlgoTrader.Core;

Console.WriteLine("Running historics...");

var results = await AlgoTrader.Historic.Engine.Engine.Run(
    new DateTime(2024, 10, 1),
    new DateTime(2024, 10, 23),
    new TradingInputs(Symbol.BP, 0.002m),
    new AlgoTrader.Core.MovingAverageCrossover.InputsMovingAverageCrossover(10, 50, 1, 1, 0.001m));

TradingMetricsVisual.DisplayVisualMetrics(results);