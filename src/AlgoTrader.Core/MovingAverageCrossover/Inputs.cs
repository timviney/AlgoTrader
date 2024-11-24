namespace AlgoTrader.Core.MovingAverageCrossover
{
    public record struct Inputs(int ShortTerm, int LongTerm, decimal MaximumBuy, decimal MaximumSell, decimal CrossoverThreshold) : IStrategyInputs
    {
        
    }
}
