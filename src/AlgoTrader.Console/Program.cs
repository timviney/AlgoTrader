using System.Net.WebSockets;
using AlgoTrader.AlphaVantage;
using AlgoTrader.Common;

Console.WriteLine("Running historics...");

var result = await AlgoTrader.Historic.Engine.Engine.Run(new DateTime(2024, 11, 22), new DateTime(2024, 11, 23),
    Symbol.AAPL, new AlgoTrader.Core.MovingAverageCrossover.Inputs());

Console.WriteLine($"Profit = {result.Profit()}");