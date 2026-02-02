using FxOptionsEngine.MarketData.Models;

namespace FxOptionsEngine.MarketData.Rates
{
    internal interface IRateProvider
    {
        Task<List<RatePoint>> GetRates();
    }
}
