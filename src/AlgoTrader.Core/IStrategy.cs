using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrader.Core
{
    public interface IStrategy
    {
        IStrategyInputs InputsGeneric { get; }

        void Run();
    }
}
