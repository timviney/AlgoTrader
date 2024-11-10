using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AlgoTrader.Common;

namespace AlgoTrader.AlphaVantage
{
    internal static class Serialiser
    {
        private static readonly JsonSerializerOptions Settings = AlphaVantage.Settings.JsonSerialiserSettings;

        private static T Deserialise<T>(string serialised) => JsonSerializer.Deserialize<T>(serialised, Settings)!;
        private static TEnum DeserialiseEnum<TEnum>(string serialised) where TEnum : Enum => (TEnum)Enum.Parse(typeof(TEnum), serialised);

        private static DateTime ToDateTime(string serialised) => DateTime.ParseExact(serialised, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        private static decimal ToDecimal(string serialised) => decimal.Parse(serialised, CultureInfo.InvariantCulture);
        private static int ToInt(string serialised) => int.Parse(serialised, CultureInfo.InvariantCulture);
        private static Interval ToInterval(string serialised) => DeserialiseEnum<Interval>('_' + serialised);
        private static Symbol ToSymbol(string serialised) => DeserialiseEnum<Symbol>(serialised);

        public static IntradayDataResponse Deserialise(string serialised)
        {
            var responseSerialisable = Deserialise<IntradayDataResponseSerialisable>(serialised);

            return new IntradayDataResponse
            {
                MetaData = new MetaData
                {
                    Information = responseSerialisable.MetaData.Information,
                    Interval = ToInterval(responseSerialisable.MetaData.Interval),
                    LastRefreshed = ToDateTime(responseSerialisable.MetaData.LastRefreshed),
                    OutputSize = responseSerialisable.MetaData.OutputSize,
                    Symbol = ToSymbol(responseSerialisable.MetaData.Symbol),
                    TimeZone = responseSerialisable.MetaData.TimeZone,
                },
                TimeSeries = responseSerialisable.TimeSeries.Select(
                    pair =>
                    {
                        (string dateTimeSerialised, IntradayTimeSeriesSerialisable series) = pair;

                        return (ToDateTime(dateTimeSerialised), new IntradayTimeSeries
                        {
                            Close = ToDecimal(series.Close),
                            High = ToDecimal(series.High),
                            Low = ToDecimal(series.Low),
                            Open = ToDecimal(series.Open),
                            Volume = ToInt(series.Volume)
                        });
                    }).ToDictionary()
            };
        }
    }
}
