using System.Net.WebSockets;
using AlgoTrader.AlphaVantage;
using AlgoTrader.Common;
using AlgoTrader.ConsoleApp;
using AlgoTrader.Core;
using Microsoft.VisualBasic;

Console.WriteLine("Running historics...");

var results = await AlgoTrader.Historic.Engine.Engine.Run(
    new DateTime(2024, 10, 1),
    new DateTime(2024, 10, 23),
    new TradingInputs(Symbol.BP, 0.002m),
    new AlgoTrader.Core.MovingAverageCrossover.Inputs(10, 50, 1, 4, 0.001m, false));

TradingMetricsVisual.DisplayVisualMetrics(results);