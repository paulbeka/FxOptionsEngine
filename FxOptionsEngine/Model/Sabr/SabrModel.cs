namespace FxOptionsEngine.Model.Sabr
{
    public sealed class SabrModel : IVolatilityModel<SabrParams>
    {
        /// <summary>
        /// SABR implied vol approximation using Hagan lognormal SABR
        /// </summary>
        /// <param name="forward">Forward FX rate (F &gt; 0)</param>
        /// <param name="strike">Strike (K &gt; 0)</param>
        /// <param name="timeToExpiry">Time to expiry in years (T &get; 0)</param>
        /// <param name="sabrParams">SABR model parameters</param>
        /// <returns>Implied volatility, double</returns>
        public double BlackVolatility(double forward, double strike, double timeToExpiry, SabrParams sabrParams)
        {
            double alpha = sabrParams.Alpha;
            double beta = sabrParams.Beta;
            double rho = sabrParams.Rho;
            double volOfVol = sabrParams.VolOfVol;

            // The ATM implied volatility needs to be calculated
            if (Math.Abs(Math.Log(forward / strike)) < 1e-8)
            {
                double forwardPower = Math.Pow(forward, (1.0f - beta));
                double t1 = Math.Pow(1.0f - beta, 2.0f) / 24.0f * (alpha * alpha) / (forwardPower * forwardPower);
                double t2 = (rho * beta * volOfVol * alpha) / (4.0f * forwardPower);
                double t3 = ((2.0f - 3.0f * rho * rho) / 24.0f) * (Math.Pow(volOfVol, 2));
                return (alpha / forwardPower) * (1.0f + ((t1 + t2 + t3) * timeToExpiry));
            }

            double forwardTimesStrike = forward * strike;
            double z = (volOfVol / alpha) * Math.Pow(forwardTimesStrike, ((1.0f - beta) / 2.0f)) * Math.Log(forward / strike);
            double zOverX = z / XOfZ(z, rho);

            double term1 = alpha / (Math.Pow(forwardTimesStrike, (1.0f - beta) / 2.0f));

            double x1 = (Math.Pow(1.0f - beta, 2) / 24.0f) * (Math.Pow(alpha, 2) / Math.Pow(forwardTimesStrike, 1 - beta));
            double x2 = (rho * beta * volOfVol * alpha) / (4 * Math.Pow(forwardTimesStrike, (1.0f - beta) / 2.0f));
            double x3 = ((2 - (3 * Math.Pow(rho, 2))) / 24.0f) * Math.Pow(volOfVol, 2);

            double term2 = 1.0f + (timeToExpiry * (x1 + x2 + x3));

            return term1 * zOverX * term2; 
        }

        private static double XOfZ(double z, double rho)
        { 
            double s = Math.Sqrt(Math.Max(0.0, 1.0f - 2.0f * rho * z + z * z));
            return Math.Log((s + z - rho) / (1.0f - rho));
        }
    }
}
