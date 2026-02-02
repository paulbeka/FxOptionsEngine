using FxOptionsEngine.Data;
using FxOptionsEngine.MarketData.Rates;
using FxOptionsEngine.Model;
using FxOptionsEngine.Model.Sabr;
using FxOptionsEngine.Surfaces;

namespace FxOptionsEngine
{
    internal class FxOptionsEngineApp
    {
        public void Run()
        {
            var http = new HttpClient();
            RateProvider sofrRates = new SofrRateProvider(http);
            RateProvider estrRates = new EstrRateProvider(http);

            var usdCurve = sofrRates.GetDicsountCurve().GetAwaiter().GetResult();
            var eurCurve = estrRates.GetDicsountCurve().GetAwaiter().GetResult();

            var usdDiscountCurve = new DiscountCurve(usdCurve);
            var eurDiscountCurve = new DiscountCurve(eurCurve);

            ForwardCurve forwardCurve = new(1.0, usdDiscountCurve, eurDiscountCurve);

            IVolatilitySurface volatilitySurface = new SabrVolatilitySurface(new SabrModel(), forwardCurve);

            volatilitySurface.GenereateSurfaceGraph();
        }
    }
}
