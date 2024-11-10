using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrader.Core
{
    internal abstract class Strategy<TInputs>(TInputs inputs) : IStrategy
        where TInputs : IStrategyInputs
    {
        public TInputs Inputs { get; } = inputs;
        public IStrategyInputs InputsGeneric => Inputs;

        public abstract void Run();
    }
}
