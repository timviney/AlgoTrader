using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrader.Core
{
    internal class MovingAverageCrossoverStrategy(MovingAverageCrossoverInputs inputs) : Strategy<MovingAverageCrossoverInputs>(inputs)
    {
        public MovingAverageCrossoverInputs Inputs { get; } = inputs;

        public override void Run()
        {
            //var data = await _marketDataService.GetPriceDataAsync("AAPL");

            //// Calculate moving averages and trading signals
            //decimal shortTermAvg = data.Prices.Take(5).Average();
            //decimal longTermAvg = data.Prices.Take(20).Average();

            //if (shortTermAvg > longTermAvg)
            //{
            //    // Buy action
            //    _databaseService.RecordTrade("AAPL", "BUY", data.CurrentPrice, DateTime.UtcNow);
            //}
            //else if (shortTermAvg < longTermAvg)
            //{
            //    // Sell action
            //    _databaseService.RecordTrade("AAPL", "SELL", data.CurrentPrice, DateTime.UtcNow);
            //}
        }
    }
}
