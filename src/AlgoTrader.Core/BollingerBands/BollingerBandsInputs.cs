using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Core.Strategy;

namespace AlgoTrader.Core.BollingerBands
{
    public record InputsBollingerBands(int Period, decimal StandardDeviationMultiplier, decimal MaximumBuy, decimal MaximumSell)
    : IStrategyInputs
    {
    }
}
