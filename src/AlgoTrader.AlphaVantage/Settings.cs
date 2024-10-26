using System.Text.Json;

namespace AlgoTrader.AlphaVantage
{
    internal static class Settings
    {
        public const string BaseUrl = "https://www.alphavantage.co/query";

        public static readonly JsonSerializerOptions JsonSerialiserSettings = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

    public static readonly string Key = Environment.GetEnvironmentVariable("AV_API_KEY")
                                              ?? throw new Exception("Cannot locate AV API KEY");
    }
}
