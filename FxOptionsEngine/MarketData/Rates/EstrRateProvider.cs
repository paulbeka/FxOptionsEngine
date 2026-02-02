
using FxOptionsEngine.MarketData.Models;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace FxOptionsEngine.MarketData.Rates
{
    public class EstrRateProvider : IRateProvider
    {
        private readonly HttpClient http;
        private readonly string BASE_URL = "https://api.estr.dev/historical";

        public EstrRateProvider(HttpClient http)
        {
            this.http = http;
        }
        public async Task<List<RatePoint>> GetRates()
        {
            var json = await http.GetStringAsync(BASE_URL);
            using var doc = JsonDocument.Parse(json);

            return JsonDocument.Parse(json)
                .RootElement
                .EnumerateArray()
                .Select(e => new RatePoint(
                    e.GetProperty("date").GetDateTime(),
                    e.GetProperty("value").GetDouble() / 100.0
                ))
                .ToList();
        }
    }
}
