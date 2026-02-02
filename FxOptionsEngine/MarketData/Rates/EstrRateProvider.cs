
using System.Text.Json;

namespace FxOptionsEngine.MarketData.Rates
{
    public class EstrRateProvider : RateProvider
    {
        private readonly HttpClient http;
        private readonly string BASE_URL = "https://api.estr.dev/historical";

        public EstrRateProvider(HttpClient http)
        {
            this.http = http;
        }

        public override async Task<double> GetRate()
        {
            var json = await http.GetStringAsync(BASE_URL);
            using var doc = JsonDocument.Parse(json);

            return JsonDocument.Parse(json)
                .RootElement
                .EnumerateArray()
                .Select(e => e.GetProperty("value").GetDouble() / 100.0)
                .First();
        }
    }
}
