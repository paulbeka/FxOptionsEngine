using FxOptionsEngine.Calibration.SabrCalibration;
using MathNet.Numerics.Distributions;

public static class OptionsProvider
{

    // TODO: Replace with real market data fetching or synthetic data generation
    public static List<StrikeToMarketVolatility> FetchData(
        double forward,
        double timeToExpiry)
    {
        double atmVol = 0.085;

        double rr25 = -0.005;
        double rr10 = -0.0075;

        double bf25 = 0.035;
        double bf10 = 0.06;

        double call25Vol = atmVol + bf25 + 0.5 * rr25;
        double call10Vol = atmVol + bf10 + 0.5 * rr10;
        double put10Vol = atmVol + bf10 - 0.5 * rr10;
        double put25Vol = atmVol + bf25 - 0.5 * rr25;

        var vols = new List<StrikeToMarketVolatility>
        {
            new StrikeToMarketVolatility(
                StrikeFromDelta(forward, put25Vol, timeToExpiry, 0.25, false),
                put25Vol
            ),
            new StrikeToMarketVolatility(
                StrikeFromDelta(forward, put10Vol, timeToExpiry, 0.10, false),
                put10Vol
            ),
            new StrikeToMarketVolatility(
                forward,
                atmVol
            ),
            new StrikeToMarketVolatility(
                StrikeFromDelta(forward, call25Vol, timeToExpiry, 0.25, true),
                call25Vol
            ),
            new StrikeToMarketVolatility(
                StrikeFromDelta(forward, call10Vol, timeToExpiry, 0.10, true),
                call10Vol
            ),
        };

        var sorted = vols.OrderBy(v => v.Strike).ToList();
        return vols;
    }

    static double StrikeFromDelta(
        double forward,
        double vol,
        double time,
        double delta,
        bool isCall)
    {
        double sign = isCall ? 1.0 : -1.0;
        return forward * Math.Exp(
            -sign * vol * Math.Sqrt(time) * Normal.InvCDF(0, 1, delta)
            + 0.5 * vol * vol * time
        );
    }
}