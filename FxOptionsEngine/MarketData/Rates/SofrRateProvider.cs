using System.Text.Json;

namespace FxOptionsEngine.MarketData.Rates
{
    public class SofrRateProvider : RateProvider
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

        public override async Task<double> GetRate()
        {
            var url =
                $"{BASE_URL}" +
                $"?series_id=SOFR30DAYAVG" +
                $"&api_key={fredApiKey}" +
                $"&file_type=json" +
                $"&sort_order=desc" +
                $"&limit=1";

            var json = await http.GetStringAsync(url);
            using var doc = JsonDocument.Parse(json);

            return doc.RootElement
                .GetProperty("observations")
                .EnumerateArray()
                .Where(o => o.GetProperty("value").GetString() != ".")
                .Select(o => double.Parse(o.GetProperty("value").GetString()!) / 100.0)
                .Last(); 
        }
    }
}
