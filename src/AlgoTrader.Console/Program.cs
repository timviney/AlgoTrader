using System.Net.WebSockets;
using AlgoTrader.AlphaVantage;
using AlgoTrader.Common;
using AlgoTrader.ConsoleApp;
using AlgoTrader.Core;
using Microsoft.VisualBasic;

Console.WriteLine("Running historics...");

var results = await AlgoTrader.Historic.Engine.Engine.Run(
    new DateTime(2024, 11, 1),
    new DateTime(2024, 11, 23),
    new TradingInputs(Symbol.AMZN, 0.002m),
    new AlgoTrader.Core.MovingAverageCrossover.Inputs(10, 50, 1, 4, 0.002m));

TradingMetricsVisual.DisplayVisualMetrics(results);