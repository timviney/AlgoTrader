using AlgoTrader.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;

namespace AlgoTrader.ConsoleApp
{
    public static class TradingMetricsDisplay
    {
        public static void DisplayMetrics(TradingResults results)
        {
            var trades = results.Trades;
            var prices = results.Prices;

            Console.WriteLine("=== Trading Performance Metrics ===\n");

            if (!trades.Any())
            {
                Console.WriteLine("No trades found in the trading results.");

                if (prices.Any())
                {
                    Console.WriteLine($"\nPrice Data Period: {prices.Count} data points");
                    Console.WriteLine($"From: {prices.Min(p => p.DateTime):yyyy-MM-dd HH:mm:ss}");
                    Console.WriteLine($"To: {prices.Max(p => p.DateTime):yyyy-MM-dd HH:mm:ss}");
                }
                else
                {
                    Console.WriteLine("No price data available.");
                }

                return;
            }

            // Overall Performance
            Console.WriteLine($"Total Profit/Loss: ${results.Profit():N2}");
            Console.WriteLine($"Number of Trades: {trades.Count}");

            // Trade Direction Breakdown
            var buyTrades = trades.Where(t => t.Direction == TradeDirection.Buy).ToList();
            var sellTrades = trades.Where(t => t.Direction == TradeDirection.Sell).ToList();
            Console.WriteLine($"Buy Trades: {buyTrades.Count}");
            Console.WriteLine($"Sell Trades: {sellTrades.Count}");

            // Profit Analysis
            var profitableTrades = trades.Where(t => t.Profit > 0).ToList();
            var losingTrades = trades.Where(t => t.Profit < 0).ToList();
            var breakEvenTrades = trades.Where(t => t.Profit == 0).ToList();

            Console.WriteLine($"\nProfitable Trades: {profitableTrades.Count} ({(double)profitableTrades.Count / trades.Count:P2})");
            Console.WriteLine($"Losing Trades: {losingTrades.Count} ({(double)losingTrades.Count / trades.Count:P2})");
            if (breakEvenTrades.Any())
            {
                Console.WriteLine($"Break-even Trades: {breakEvenTrades.Count} ({(double)breakEvenTrades.Count / trades.Count:P2})");
            }

            if (profitableTrades.Any())
            {
                Console.WriteLine($"Average Profit (winning trades): ${profitableTrades.Average(t => t.Profit):N2}");
                Console.WriteLine($"Largest Profit: ${profitableTrades.Max(t => t.Profit):N2}");
            }

            if (losingTrades.Any())
            {
                Console.WriteLine($"Average Loss (losing trades): ${losingTrades.Average(t => t.Profit):N2}");
                Console.WriteLine($"Largest Loss: ${losingTrades.Min(t => t.Profit):N2}");
            }

            // Time Analysis
            var tradingPeriod = trades.Max(t => t.DateTime) - trades.Min(t => t.DateTime);
            Console.WriteLine($"\nTrading Period: {tradingPeriod.Days} days");
            Console.WriteLine($"Average Trades per Day: {trades.Count / (tradingPeriod.Days == 0 ? 1 : tradingPeriod.Days):N1}");

            // Symbol Analysis
            var symbolGroups = trades.GroupBy(t => t.Symbol)
                .Select(g => new
                {
                    Symbol = g.Key,
                    TradeCount = g.Count(),
                    Profit = g.Sum(t => t.Profit)
                })
                .OrderByDescending(g => g.Profit);

            Console.WriteLine("\n=== Performance by Symbol ===");
            foreach (var group in symbolGroups)
            {
                Console.WriteLine($"{group.Symbol}: {group.TradeCount} trades, ${group.Profit:N2} profit");
            }

            // Calculate and display drawdown if we have price data
            if (prices.Any())
            {
                var maxDrawdown = CalculateMaxDrawdown(prices.Select(p => (double)p.Close).ToList());
                Console.WriteLine($"\nMaximum Drawdown: {maxDrawdown:P2}");
            }
            else
            {
                Console.WriteLine("\nNo price data available for drawdown calculation.");
            }
        }

        private static double CalculateMaxDrawdown(List<double> prices)
        {
            if (!prices.Any())
                return 0;

            double maxDrawdown = 0;
            double peak = prices[0];

            foreach (var price in prices)
            {
                if (price > peak)
                {
                    peak = price;
                }
                else
                {
                    double drawdown = (peak - price) / peak;
                    maxDrawdown = Math.Max(maxDrawdown, drawdown);
                }
            }

            return maxDrawdown;
        }
    }
}
