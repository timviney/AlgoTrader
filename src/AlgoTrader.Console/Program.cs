using System.Net.WebSockets;
using AlgoTrader.AlphaVantage;
using AlgoTrader.Common;
using AlgoTrader.ConsoleApp;
using AlgoTrader.Core;

Console.WriteLine("Running historics...");

var results = await AlgoTrader.Historic.Engine.Engine.Run(
    new DateTime(2024, 11, 22), 
    new DateTime(2024, 11, 23),
    new TradingInputs(Symbol.AAPL, 0.002m),
    new AlgoTrader.Core.MovingAverageCrossover.Inputs(5, 20, 1, 2));

TradingMetricsVisual.DisplayVisualMetrics(results);

//var v = new List<double>();
//for (var i = 0; i < results.Trades.Count; i++)
//{
//    var resultsTrade = results.Trades[i];
//    if (i > 1 && results.Trades[i-1].DateTime == resultsTrade.DateTime) continue;
//    Console.WriteLine($"{resultsTrade.DateTime}");
//}
//for (var i = 0; i < results.Trades.Count; i++)
//{
//    var resultsTrade = results.Trades[i];
//    Console.Write($"{resultsTrade.Profit} ");
//    if (i < results.Trades.Count-1 && results.Trades[i + 1].DateTime == resultsTrade.DateTime) continue;
//    Console.Write("\r\n");
//}