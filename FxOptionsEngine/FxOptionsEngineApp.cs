using FxOptionsEngine.Data;
using FxOptionsEngine.MarketData.Rates;
using FxOptionsEngine.Model;
using FxOptionsEngine.Surfaces;

namespace FxOptionsEngine
{
    internal class FxOptionsEngineApp
    {
        public void Run()
        {
            var usdCurve = new DiscountCurve(new SortedDictionary<double, double>
            {
                { 0.25, 0.997 },
                { 0.5,  0.990 },
                { 1.0,  0.980 },
                { 2.0,  0.963 }
            });

            var eurCurve = new DiscountCurve(new SortedDictionary<double, double>
            {
                { 0.25, 0.992 },
                { 0.5,  0.994 },
                { 1.0,  0.996 },
                { 2.0,  0.999 }
            });

            ForwardCurve forwardCurve = new(1.0, usdCurve, eurCurve);

            IVolatilitySurface volatilitySurface = new SabrVolatilitySurface(new SabrModel(), forwardCurve);

            var http = new HttpClient();
            IRateProvider sofrRates = new SofrRateProvider(http);
            IRateProvider estrRates = new EstrRateProvider(http);

            sofrRates.GetRates().GetAwaiter().GetResult();
            estrRates.GetRates().GetAwaiter().GetResult();

            //volatilitySurface.GenereateSurfaceGraph();

        }
    }
}
