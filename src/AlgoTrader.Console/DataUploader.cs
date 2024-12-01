using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using AlgoTrader.Common;
using AlgoTrader.Core;
using StockData;

namespace AlgoTrader.ConsoleApp
{
    public static class DataUploader
    {
        private const string DateTimeFormat = "yyyyMMddHHmmss";

        public static async Task ReadAndUpload(string folderPath, Interval interval)
        {
            var possibleSymbolsToRead = Enum.GetNames(typeof(Symbol)).ToHashSet();
            foreach (var file in Directory.GetFiles(folderPath))
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var symbol = fileName.Split('.')[0].ToUpper();

                if (!possibleSymbolsToRead.Contains(symbol)) continue;

                var data = ReadData(file);

                await Uploader.UploadFilesAsync(data, interval.AsString(), symbol);
            }
        }

        private static List<MarketDataEntry> ReadData(string file)
        {
            using var csvReader = new StreamReader(File.OpenRead(file));
            var result = new List<MarketDataEntry>();

            csvReader.ReadLine(); // skip headers
            while (!csvReader.EndOfStream)
            {
                var line = csvReader.ReadLine();
                var values = line.Split(',');

                var date = values[2];
                var time = values[3];
                var open = values[4];
                var high = values[5];
                var low = values[6];
                var close = values[7];
                var vol = values[8];

                var dataPoint = new MarketDataEntry(DateTime.ParseExact(date+time, DateTimeFormat, CultureInfo.InvariantCulture), decimal.Parse(open),
                    decimal.Parse(high), decimal.Parse(low), decimal.Parse(close), (int)Math.Round(double.Parse(vol)));

                result.Add(dataPoint);
            }

            return result;
        }
    }
}
