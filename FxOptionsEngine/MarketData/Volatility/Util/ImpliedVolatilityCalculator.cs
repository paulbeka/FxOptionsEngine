public static class ImpliedVolatilityCalculator
{
    public static double SolveImpliedVolatility(
        double price, 
        double forward, 
        double strike, 
        double timeToExpiry, 
        double discountFactor,
        bool isCall)
    {
        double left = 1e-6, right = 3.0;

        for (int i = 0; i < 100; i++)
        {
            double mid = 0.5 * (left + right);
            double model = Black76.Price(forward, strike, timeToExpiry, mid, discountFactor, isCall);

            if (model > price) right = mid;
            else left = mid;
        }

        return 0.5 * (left + right);
    }
}
