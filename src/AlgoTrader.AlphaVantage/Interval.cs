using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrader.AlphaVantage
{
    public enum Interval
    {
        _1min,
        _5min,
        _15min,
        _30min,
        _60min,
    }

    public static class IntervalExtensions
    {
        public static string AsString(this Interval interval)
        {
            return interval.ToString().Trim('_');
        }

        public static Interval ToInterval(this string intervalStr)
        {
            if (intervalStr[0] == '_') intervalStr = '_' + intervalStr;

            return Enum.Parse<Interval>(intervalStr);
        }
    }
}
