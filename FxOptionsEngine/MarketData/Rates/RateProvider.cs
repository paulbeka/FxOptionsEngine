namespace FxOptionsEngine.MarketData.Rates
{
    public abstract class RateProvider
    {
        public abstract Task<double> GetRate();
        private readonly double ACT_360 = 360;

        /// <summary>
        /// Discount curve generation using continuous compounding
        /// Does not use bootstrapping for simplicity
        /// </summary>
        public async Task<SortedDictionary<double, double>> GetDicsountCurve()
        {
            var sofrRate = await GetRate();
            var curve = new SortedDictionary<double, double>();

            double[] maturities =
            {
                1.0 / ACT_360,
                1.0 / 12.0,
                0.25,
                0.5,
                1.0,
                2.0,
                5.0,
                10.0
            };

            foreach (var time in maturities)
            {
                var discount = Math.Exp(-sofrRate * time);
                curve.Add(time, discount);
            }

            return curve;
        }
    }
}
