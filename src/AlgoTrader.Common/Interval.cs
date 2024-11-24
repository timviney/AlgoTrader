namespace AlgoTrader.Common
{
    public enum Interval
    {
        _1min = 1,
        _5min = 5,
        _15min = 15,
        _30min = 30,
        _60min = 60,
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

            return Enum.TryParse<Interval>(intervalStr, out var result)
                ? result
                : throw new Exception($"Cannot parse interval value {intervalStr}");
        }
    }
}