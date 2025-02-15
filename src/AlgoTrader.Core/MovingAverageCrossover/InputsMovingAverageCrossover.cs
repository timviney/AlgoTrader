using AlgoTrader.Core.Strategy;

namespace AlgoTrader.Core.MovingAverageCrossover
{
    public record InputsMovingAverageCrossover(int ShortTerm, int LongTerm, decimal MaxExposure, decimal MaximumBuy, decimal MaximumSell) : IStrategyInputs
    {
    }
}
