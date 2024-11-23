using System.Net.WebSockets;
using AlgoTrader.AlphaVantage;
using AlgoTrader.Common;
using AlgoTrader.ConsoleApp;

Console.WriteLine("Running historics...");

var results = await AlgoTrader.Historic.Engine.Engine.Run(new DateTime(2024, 11, 22), new DateTime(2024, 11, 23),
    Symbol.AAPL, new AlgoTrader.Core.MovingAverageCrossover.Inputs());

TradingMetricsDisplay.DisplayMetrics(results);
Console.WriteLine("\r\n------------------------------------------------------- \r\n");
TradingMetricsVisual.DisplayVisualMetrics(results);