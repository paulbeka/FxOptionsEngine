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
        double rr25 = -0.002;
        double bf25 = 0.0015;

        double call25Vol = atmVol + bf25 + 0.5 * rr25;
        double put25Vol = atmVol + bf25 - 0.5 * rr25;

        var vols = new List<StrikeToMarketVolatility>
        {
            new StrikeToMarketVolatility(
                forward,
                atmVol
            ),
            new StrikeToMarketVolatility(
                StrikeFromDelta(forward, call25Vol, timeToExpiry, 0.25, true),
                call25Vol
            ),
            new StrikeToMarketVolatility(
                StrikeFromDelta(forward, put25Vol, timeToExpiry, 0.25, false),
                put25Vol
            )
        };

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