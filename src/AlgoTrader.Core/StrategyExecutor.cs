using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;

namespace AlgoTrader.Core
{
    public abstract class StrategyExecutor<TInputs>(TradingInputs tradingInputs, TInputs strategyInputs) : IStrategy
        where TInputs : IStrategyInputs
    {
        protected TradingInputs TradingInputs { get; } = tradingInputs;
        protected TInputs StrategyInputs { get; } = strategyInputs;

        internal MarketState MarketState { get; } = new();
        internal TradingState TradingState { get; } = new();

        protected void RecordTrade(TradeDirection direction, decimal maxQuantity, bool allowSwing = true)
        {
            var price = MarketState.OpeningPrice(direction, TradingInputs.Slippage);

            if (TradingState.TryGetOpenPositions(TradingInputs.Symbol, out List<Position> openPositions))
            {
                var quantityToRecord = maxQuantity;
                foreach (var openPosition in openPositions)
                {
                    if (openPosition.Direction == direction)
                    {
                        // all positions open in same direction, so open a new one.
                        OpenNewPosition(direction, maxQuantity, price);
                        return;
                    }
                    else
                    {
                        // seek to close off all positions starting with the oldest one.
                        var volume = openPosition.Quantity();
                        if (volume > quantityToRecord + Constants.Tol)
                        {
                            // Partially close position and end
                            TradingState.RecordTrade(direction, TradingInputs.Symbol, quantityToRecord, price,
                                MarketState.Current.DateTime, openPosition);
                            return;
                        }
                        else
                        {
                            // Fully close position
                            quantityToRecord -= volume;
                            TradingState.RecordTrade(direction, TradingInputs.Symbol, volume, price,
                                MarketState.Current.DateTime, openPosition);

                            // End if volume left was equal to quantity
                            if (quantityToRecord < Constants.Tol) return;
                        }
                    }
                }

                if (quantityToRecord > Constants.Tol && allowSwing)
                {
                    // Closed all open positions and now want to open in the other direction
                    OpenNewPosition(direction, quantityToRecord, price);
                }
            }
            else
            {
                OpenNewPosition(direction, maxQuantity, price);
            }
        }

        private void OpenNewPosition(TradeDirection direction, decimal maxQuantity, decimal price)
        {
            var position = TradingState.OpenPosition(direction, TradingInputs.Symbol);
            TradingState.RecordTrade(direction, TradingInputs.Symbol, maxQuantity, price,
                MarketState.Current.DateTime, position);
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
            if (!TradingState.TryGetOpenPositions(TradingInputs.Symbol, out var openPositions)) return;

            var price = MarketState.OpeningPrice(TradeDirection.Sell, TradingInputs.Slippage);

            foreach (var openPosition in openPositions)
            {
                TradingState.ClosePosition(openPosition, price, MarketState.Current.DateTime);
            }
        }

        public TradingResults GetResults() => new(TradingState.GetAllPositions(), MarketState.Data);
    }
}
