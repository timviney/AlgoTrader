﻿using AlgoTrader.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;
using Spectre.Console;

namespace AlgoTrader.ConsoleApp
{
    using AlgoTrader.Core.MarketData;
    using AlgoTrader.Core.Trades;
    using Spectre.Console;
    using System.Collections.Generic;
    using System.Linq;

    public static class TradingMetricsVisual
    {
        public static void DisplayVisualMetrics(TradingResults results)
        {
            if (!results.Trades.Any())
            {
                AnsiConsole.MarkupLine("[red]No trades found in the trading results.[/]");
                return;
            }

            // Title
            AnsiConsole.Write(new FigletText("Trading Results")
                .Centered()
                .Color(Color.Blue));

            // Summary Panel
            var summaryMarkup = $"""
            [bold blue]Total Profit/Loss:[/] ${results.Profit():N2}
            [bold blue]Total Trades:[/] {results.Trades.Count} ([red]{results.Buys.Count}[/] / [green]{results.Sells.Count}[/])
            [bold blue]Time Period:[/] {results.Prices.Min(m => m.DateTime):g} - {results.Prices.Max(m => m.DateTime):g}
            """;

            var summaryPanel = new Panel(
               Align.Center(new Markup(summaryMarkup)))
            {
                Border = BoxBorder.Double,
                Header = new PanelHeader("Summary"),
                Expand = true
            };
            AnsiConsole.Write(summaryPanel);

            // Trade Distribution Chart

            var positions = results.Positions;
            var wins = positions.Where(p => p.Profit() > Constants.Tol).ToList();
            var losses = positions.Where(p => p.Profit() < -Constants.Tol).ToList();
            var breakEvens = positions.Where(p => Math.Abs(p.Profit()) < Constants.Tol).ToList();

            var chart = new BarChart()
                .Width(60)
                .Label("[green bold underline]Trade Distribution[/]")
                .CenterLabel()
                .AddItem("Wins", wins.Count, Color.Green)
                .AddItem("Losses", losses.Count, Color.Red)
                .AddItem("Break Evens", breakEvens.Count, Color.Grey);

            AnsiConsole.Write(chart);

            // Trading Activity Grid
            DisplayTradingActivityGrid(results.Trades);

            // Detailed Statistics Table
            var table = new Table()
                .Border(TableBorder.Rounded)
                .Title("Detailed Statistics")
                .AddColumn(new TableColumn("Metric").Centered())
                .AddColumn(new TableColumn("Value").Centered());

            var profitStats = CalculateTradeStats(positions);

            table.AddRow("Win Rate", $"{profitStats.WinRate:P2}");
            table.AddRow("Average Profit", $"${profitStats.AverageProfit:N2}");
            table.AddRow("Largest Profit", $"${profitStats.LargestProfit:N2}");
            table.AddRow("Largest Loss", $"${profitStats.LargestLoss:N2}");
            table.AddRow("Profit Factor", $"{profitStats.ProfitFactor:N2}");

            AnsiConsole.Write(table);

            // Symbol Performance
            var symbolGroups = results.Trades
                .GroupBy(t => t.Symbol)
                .Select(g => new { Symbol = g.Key, Profit = g.Sum(t => t.Profit) })
                .OrderByDescending(g => g.Profit)
                .ToList();

            if (symbolGroups.Any())
            {
                var symbolChart = new BarChart()
                    .Width(60)
                    .Label("[blue bold underline]Profit by Symbol[/]")
                    .CenterLabel();

                foreach (var group in symbolGroups)
                {
                    symbolChart.AddItem(
                        group.Symbol.ToString(),
                        Math.Abs((double)group.Profit),
                        group.Profit >= 0 ? Color.Green : Color.Red);
                }

                AnsiConsole.Write(symbolChart);
            }

            // Price Chart if available
            if (results.Prices.Any())
            {
                DisplayPriceChart(results.Prices, results.Positions);
            }
        }

        private static void DisplayTradingActivityGrid(List<Trade> trades)
        {
            AnsiConsole.Write(new Rule("[yellow]Trading Activity[/]").RuleStyle("grey").Centered());

            // Group trades by month
            var tradesByMonth = trades
                .GroupBy(t => new { t.DateTime.Year, t.DateTime.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g => new
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1),
                    TotalTrades = g.Count(),
                    ProfitableTrades = g.Count(t => t.Profit > 0),
                    TotalProfit = g.Sum(t => t.Profit)
                })
                .ToList();

            if (!tradesByMonth.Any())
                return;

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("Month")
                .AddColumn("Trades")
                .AddColumn("Profitable")
                .AddColumn("Profit/Loss");

            foreach (var month in tradesByMonth)
            {
                var profitColor = month.TotalProfit >= 0 ? "green" : "red";
                table.AddRow(
                    month.Month.ToString("MMM yyyy"),
                    month.TotalTrades.ToString(),
                    $"[green]{month.ProfitableTrades}[/]/{month.TotalTrades}",
                    $"[{profitColor}]${month.TotalProfit:N2}[/]"
                );
            }

            AnsiConsole.Write(table);
        }

        private static void DisplayPriceChart(List<MarketDataPoint> prices, List<Position> positions)
        {
            AnsiConsole.Write(new Rule("[yellow]Price History[/]").RuleStyle("grey").Centered());

            var canvas = new Canvas(60, 20);

            // Scale the prices to fit the canvas height
            var minPrice = prices.Min(p => p.Close);
            var maxPrice = prices.Max(p => p.Close);
            var priceRange = maxPrice - minPrice;

            // Get Trades and mapped datetimes
            var trades = new Dictionary<DateTime, TradeDirection>();
            foreach (Position pos in positions)
            {
                foreach (var trade in pos.Trades)
                {
                    trades[trade.DateTime] = trade.Direction;
                }
            }

            var dateTimesIndexes = new Dictionary<DateTime, int>();
            for (int i = 1; i < prices.Count; i++)
            {
                dateTimesIndexes[prices[i].DateTime] = i;
            }

            // Draw points and connect them
            for (int i = 1; i < prices.Count; i++)
            {
                CalculateAndDrawLine(prices, i, canvas, minPrice, priceRange, Color.Yellow);
            }

            // Draw Buy/Sell on top
            foreach (var (datetime, direction) in trades)
            {
                var position = dateTimesIndexes[datetime];
                Color color = direction == TradeDirection.Buy
                    ? Color.Red
                    : Color.Green;
                CalculateAndDrawLine(prices, position, canvas, minPrice, priceRange, color);
            }


            var panel = new Panel(canvas)
            {
                Header = new PanelHeader($"Price Range: ${prices.Min(p => p.Low):N2} - ${prices.Max(p => p.High):N2}"),
                Border = BoxBorder.Rounded
            };
            AnsiConsole.Write(panel);
        }

        private static void CalculateAndDrawLine(List<MarketDataPoint> prices, int i, Canvas canvas, decimal minPrice, decimal priceRange,
            Color colour)
        {
            var x1 = (i - 1) * canvas.Width / (prices.Count - 1);
            var x2 = i * canvas.Width / (prices.Count - 1);

            var y1 = canvas.Height - (int)((prices[i - 1].Close - minPrice) * canvas.Height / priceRange);
            var y2 = canvas.Height - (int)((prices[i].Close - minPrice) * canvas.Height / priceRange);

            // Draw line using Bresenham's algorithm
            DrawLine(canvas, x1, y1, x2, y2, colour);
        }

        private static void DrawLine(Canvas canvas, int x1, int y1, int x2, int y2, Color color)
        {
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = x1 < x2 ? 1 : -1;
            int sy = y1 < y2 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2;

            while (true)
            {
                if (x1 >= 0 && x1 < canvas.Width && y1 >= 0 && y1 < canvas.Height)
                    canvas.SetPixel(x1, y1, color);

                if (x1 == x2 && y1 == y2) break;

                int e2 = err;
                if (e2 > -dx)
                {
                    err -= dy;
                    x1 += sx;
                }
                if (e2 < dy)
                {
                    err += dx;
                    y1 += sy;
                }
            }
        }

        private struct TradeStats
        {
            public double WinRate { get; init; }
            public double AverageProfit { get; init; }
            public double LargestProfit { get; init; }
            public double LargestLoss { get; init; }
            public double ProfitFactor { get; init; }
        }

        private static TradeStats CalculateTradeStats(List<Position> positions)
        {
            var winningTrades = positions.Where(p => p.Profit() > 0).ToList();
            var losingTrades = positions.Where(p => p.Profit() < 0).ToList();

            var grossProfit = winningTrades.Sum(p => p.Profit());
            var grossLoss = Math.Abs(losingTrades.Sum(p => p.Profit()));

            return new TradeStats
            {
                WinRate = (double)winningTrades.Count / positions.Count,
                AverageProfit = (double)positions.Average(t => t.Profit()),
                LargestProfit = (double)positions.Max(t => t.Profit()),
                LargestLoss = (double)positions.Min(t => t.Profit()),
                ProfitFactor = grossLoss == 0 ? double.PositiveInfinity : (double)(grossProfit / grossLoss)
            };
        }
    }
}
