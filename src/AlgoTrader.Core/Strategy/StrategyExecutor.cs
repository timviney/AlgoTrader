using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;
using AlgoTrader.Core.MarketData;
using AlgoTrader.Core.Trades;

namespace AlgoTrader.Core.Strategy
{
    public abstract class StrategyExecutor<TInputs>(TradingInputs tradingInputs, TInputs strategyInputs) : IStrategy
        where TInputs : IStrategyInputs
    {
        protected TradingInputs TradingInputs { get; } = tradingInputs;
        protected TInputs StrategyInputs { get; } = strategyInputs;

        internal MarketState MarketState { get; } = new();
        internal TradingState TradingState { get; } = new();

        protected void RecordTrade(TradeDirection direction, decimal maxQuantity, decimal maxExposure, bool allowSwing = true)
        {
            var price = MarketState.CurrentPriceWithSlippage(direction, TradingInputs.Slippage);
            
            var currentExposure = TradingState.Exposure(direction);
            var maxIncreaseInExposure = maxExposure - currentExposure;
            var quantity = Math.Min(maxQuantity, maxIncreaseInExposure / price);

            if (TradingState.TryGetOpenPositions(TradingInputs.Symbol, out List<Position> openPositions))
            {
                var quantityToRecord = quantity;
                foreach (var openPosition in openPositions)
                {
                    if (openPosition.Direction == direction)
                    {
                        // all positions open in same direction, so open a new one.
                        OpenNewPosition(direction, quantity, price);
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
                OpenNewPosition(direction, quantity, price);
            }
        }

        private void OpenNewPosition(TradeDirection direction, decimal quantity, decimal price)
        {
            var position = TradingState.OpenPosition(direction, TradingInputs.Symbol);
            TradingState.RecordTrade(direction, TradingInputs.Symbol, quantity, price,
                MarketState.Current.DateTime, position);
        }

        protected abstract void Run();

        public void NextPeriod(CurrentPrice currentPrice, MarketDataPoint previousPeriod)
        {
            MarketState.Update(currentPrice, previousPeriod);

            Run();
        }

        public void End()
        {
            // Timeframe is over, so close all open positions
            if (!TradingState.TryGetOpenPositions(TradingInputs.Symbol, out var openPositions)) return;

            var price = MarketState.CurrentPriceWithSlippage(TradeDirection.Sell, TradingInputs.Slippage);

            foreach (var openPosition in openPositions)
            {
                TradingState.ClosePosition(openPosition, price, MarketState.Current.DateTime);
            }
        }

        public TradingResults GetResults() => new(TradingState.GetAllPositions(), MarketState.Data);
    }
}
