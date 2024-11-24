using System.Net.WebSockets;
using AlgoTrader.AlphaVantage;
using AlgoTrader.Common;
using AlgoTrader.ConsoleApp;
using AlgoTrader.Core;

Console.WriteLine("Running historics...");

var results = await AlgoTrader.Historic.Engine.Engine.Run(
    new DateTime(2024, 11, 22), 
    new DateTime(2024, 11, 23),
    new TradingInputs(Symbol.AMZN, 0.002m),
    new AlgoTrader.Core.MovingAverageCrossover.Inputs(5, 20, 1, 2));

TradingMetricsVisual.DisplayVisualMetrics(results);