using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;

namespace AlgoTrader.Core
{
    public abstract class Strategy<TInputs>(TradingInputs tradingInputs, TInputs strategyInputs) : IStrategy
        where TInputs : IStrategyInputs
    {
        protected TradingInputs TradingInputs { get; } = tradingInputs;
        protected TInputs StrategyInputs { get; } = strategyInputs;

        internal MarketState MarketState { get; } = new();
        internal TradingState TradingState { get; } = new();

        protected void RecordTrade(TradeDirection direction, decimal maxQuantity, bool allowLoss = true)
        {
            var price = MarketState.OpeningPrice(direction, TradingInputs.Slippage);

            if (direction == TradeDirection.Buy)
            {
                TradingState.OpenPosition(TradingInputs.Symbol, maxQuantity, price, MarketState.Current.DateTime);
            }
            else
            {
                var openTrades = TradingState.OpenPositions(TradingInputs.Symbol);

                decimal closedQuantity = 0;
                foreach (var openTrade in openTrades)
                {
                    if (!allowLoss && price < openTrade.Price) continue;

                    var openQuantity = openTrade.OpenQuantity();

                    var toClose = Math.Min(openQuantity, maxQuantity - closedQuantity);
                    
                    TradingState.ClosePosition(openTrade, toClose, price, MarketState.Current.DateTime);

                    closedQuantity += toClose;
                    if (closedQuantity >= maxQuantity - Constants.Tol) break;
                }
            }
        }

        protected abstract void Run();

        public void NextPeriod(MarketDataPoint marketDataPoint)
        {
            MarketState.Update(marketDataPoint);

            Run();
        }

        public void End()
        {
            // Timeframe is over, so close all open positions
            var openTrades = TradingState.OpenPositions(TradingInputs.Symbol);

            var price = MarketState.OpeningPrice(TradeDirection.Sell, TradingInputs.Slippage);

            foreach (var openTrade in openTrades)
            {
                TradingState.ClosePosition(openTrade, openTrade.Quantity, price, MarketState.Current.DateTime);
            }
        }

        public TradingResults GetResults() => new(TradingState.GetAllTrades(), MarketState.Data);
    }
}
