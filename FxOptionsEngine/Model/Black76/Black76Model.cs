using MathNet.Numerics.Distributions;

public static class Black76
{
    /// <summary>
    /// Prices a European option on a forward
    /// </summary>
    public static double Price(
        double forward,
        double strike,
        double timeToExpiry,
        double volatility,
        double discountFactor,
        bool isCall)
    {
        if (timeToExpiry <= 0.0 || volatility <= 0.0)
        {
            double intrinsic = isCall ? Math.Max(forward - strike, 0.0): Math.Max(strike - forward, 0.0);
            return discountFactor * intrinsic;
        }

        double sqrtT = Math.Sqrt(timeToExpiry);

        double d1 = (Math.Log(forward / strike)
                    + 0.5 * volatility * volatility * timeToExpiry)
                    / (volatility * sqrtT);

        double d2 = d1 - volatility * sqrtT;

        if (isCall)
        {
            return discountFactor * (forward * Normal.CDF(0.0, 1.0, d1) - strike * Normal.CDF(0.0, 1.0, d2));
        }
        else
        {
            return discountFactor * (strike * Normal.CDF(0.0, 1.0, -d2)- forward * Normal.CDF(0.0, 1.0, -d1));
        }
    }
}
