namespace AlgoTrader.Core.MovingAverageCrossover
{
    public record Inputs(int ShortTerm, int LongTerm, decimal MaximumBuy, decimal MaximumSell, decimal CrossoverThreshold, bool AllowLoss) : IStrategyInputs
    {
    }
}
