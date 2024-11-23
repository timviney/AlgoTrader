﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;

namespace AlgoTrader.Core
{
    internal record Trade(Symbol Symbol, TradeDirection Direction, double Quantity, double Price, DateTime DateTime)
    {
        public double Profit = Price * Quantity * (Direction == TradeDirection.Buy ? -1 : 1);
    }
}
