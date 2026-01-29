namespace FxOptionsEngine.Data
{
    public sealed class DiscountCurve
    {
        private readonly SortedDictionary<double, double> discountFactors;

        public DiscountCurve(SortedDictionary<double, double> discountFactors)
        {
            if (discountFactors == null || discountFactors.Count == 0)
                throw new ArgumentException("Discount curve cannot have no data.");

            this.discountFactors = discountFactors;
        }

        public double GetDiscountFactor(double timeToExpiry)
        {
            return InterpolateLogDiscountFactor(timeToExpiry);
        }

        private double InterpolateLogDiscountFactor(double t)
        {
            var keys = discountFactors.Keys.ToList();

            if (t <= keys.First())
            {
                return discountFactors[keys.First()];
            }

            if (t >= keys.Last())
            {
                return discountFactors[keys.Last()];
            }

            for (int i = 0; i < keys.Count - 1; i++)
            {
                double t1 = keys[i];
                double t2 = keys[i + 1];

                if (t1 <= t && t <= t2)
                {
                    double discountFactor1 = Math.Log(discountFactors[t1]);
                    double discountFactor2 = Math.Log(discountFactors[t2]);

                    double w = (t - t1) / (t2 - t1);
                    return Math.Exp(discountFactor1 + w * (discountFactor2 - discountFactor1));
                }
            }

            throw new InvalidOperationException("Failes to get interpolation.");
        }
    }
}
