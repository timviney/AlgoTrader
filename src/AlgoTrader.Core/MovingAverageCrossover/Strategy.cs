namespace AlgoTrader.Core.MovingAverageCrossover
{
    internal class Strategy(Inputs inputs) : Strategy<Inputs>(inputs)
    {
        public new Inputs Inputs = inputs;

        protected sealed override void Run()
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
