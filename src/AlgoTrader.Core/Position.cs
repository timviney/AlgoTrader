using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrader.Core
{
    public record Position(Trade Buy, List<Trade> Sells)
    {
        public decimal Profit { get; init; } = Buy.Profit + Sells.Sum(s => s.Profit);
    }
}
