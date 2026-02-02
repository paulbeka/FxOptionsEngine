using FxOptionsEngine.MarketData.Models;
using System.Text.Json;

namespace FxOptionsEngine.MarketData.Rates
{
    public class SofrRateProvider : IRateProvider
    {
        private readonly HttpClient http;
        private readonly string BASE_URL = "https://api.stlouisfed.org/fred/series/observations";
        private readonly string fredApiKey;

        public SofrRateProvider(HttpClient http)
        {
            this.http = http;
            fredApiKey = Environment.GetEnvironmentVariable("FRED_API_KEY") 
                ?? throw new InvalidOperationException("FRED_API_KEY environment variable is not set.");
        }

        public async Task<List<RatePoint>> GetRates()
        {
            var url =
                $"{BASE_URL}" +
                $"?series_id=SOFRINDEX" +
                $"&api_key={fredApiKey}" +
                $"&file_type=json";

            var json = await http.GetStringAsync(url);
            using var doc = JsonDocument.Parse(json);

            return doc.RootElement
                .GetProperty("observations")
                .EnumerateArray()
                .Where(o => o.GetProperty("value").GetString() != ".")
                .Select(o => new RatePoint(
                    DateTime.Parse(o.GetProperty("date").GetString()!),
                    double.Parse(o.GetProperty("value").GetString()!) / 100.0
                ))
                .ToList();
        }
    }
}
