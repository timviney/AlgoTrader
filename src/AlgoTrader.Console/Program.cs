using System.Net.WebSockets;
using AlgoTrader.AlphaVantage;

Console.WriteLine("Hello, World!");

using var client = new AlphaVantageClient();

var result = await client.GetIntradayDataAsync(Symbol.AAPL, Interval._5min);

int banana = 0;