using FxOptionsEngine.Model;

namespace FxOptionsEngine.Calibration.SabrCalibration
{
    public sealed class SabrVolatilityCalibration
    {
        private readonly ISabrModel model;
        private readonly List<StrikeToMarketVolatility> marketVolPoints;

        private readonly double FIXED_BETA = 0.5; // todo: compare with other fixed beta and choose lowest error
        private readonly int N_ITERS = 500;

        public SabrVolatilityCalibration(
            ISabrModel model,
            List<StrikeToMarketVolatility> marketVolPoints)
        {
            this.model = model;
            this.marketVolPoints = marketVolPoints;
        }

        public SabrParams Calibrate(
            double forward,
            double timeToExpiry)
        {
            if (forward < 0)
            {
                throw new ArgumentOutOfRangeException("Forward should be above 0");
            }

            if (timeToExpiry < 0)
            {
                throw new ArgumentOutOfRangeException("Time to Expiry should be above 0");
            }

            double alpha = 0.5;
            double v = 0.5;
            double rho = 0.5;

            double stepAlpha = 0.05;
            double stepV = 0.05;
            double stepRho = 0.05;

            double bestError = Error(alpha, rho, v, forward, timeToExpiry);

            for (int iter = 0; iter < N_ITERS; iter++)
            {
                UpdateAlpha(ref alpha, stepAlpha, ref bestError, rho, v, forward, timeToExpiry);
                UpdateRho(ref rho, stepRho, ref bestError, alpha, v, forward, timeToExpiry);
                UpdateV(ref v, stepV, ref bestError, alpha, rho, forward, timeToExpiry);

                stepAlpha *= 0.95;
                stepRho *= 0.95;
                stepV *= 0.95;
            }

            return new SabrParams(alpha, FIXED_BETA, rho, v);
        }

        private void UpdateAlpha(
            ref double alpha,
            double step,
            ref double best,
            double rho,
            double v,
            double forward,
            double timeToExpiry)
        {
            double up = alpha + step;
            double down = Math.Max(1e-6, alpha - step);

            double errUp = Error(up, rho, v, forward, timeToExpiry);
            double errDown = Error(down, rho, v, forward, timeToExpiry);

            if (errUp < best)
            {
                alpha = up;
                best = errUp;
            }
            else if (errDown < best)
            {
                alpha = down;
                best = errDown;
            }
        }

        private void UpdateRho(
            ref double rho,
            double step,
            ref double best,
            double alpha,
            double v,
            double forward,
            double timeToExpiry)
        {
            double up = Math.Min(0.999, rho + step);
            double down = Math.Max(-0.999, rho - step);

            double errUp = Error(alpha, up, v, forward, timeToExpiry);
            double errDown = Error(alpha, down, v, forward, timeToExpiry);

            if (errUp < best)
            {
                rho = up;
                best = errUp;
            }
            else if (errDown < best)
            {
                rho = down;
                best = errDown;
            }
        }

        private void UpdateV(
            ref double v,
            double step,
            ref double best,
            double alpha,
            double rho,
            double forward,
            double timeToExpiry)
        {
            double up = v + step;
            double down = Math.Max(1e-6, v - step);

            double errUp = Error(alpha, rho, up, forward, timeToExpiry);
            double errDown = Error(alpha, rho, down, forward, timeToExpiry);

            if (errUp < best)
            {
                v = up;
                best = errUp;
            }
            else if (errDown < best)
            {
                v = down;
                best = errDown;
            }
        }

        private double Error(
            double a,
            double r,
            double v,
            double forward,
            double timeToExpiry)
        {
            if (a <= 0 || v <= 0 || r <= -0.999 || r >= 0.999)
                return double.MaxValue;

            var parameters = new SabrParams(a, FIXED_BETA, r, v);
            double sum = 0.0;

            foreach (var point in marketVolPoints)
            {
                double modelVol =
                    model.BlackVolatility(forward, point.Strike, timeToExpiry, parameters);

                double diff = modelVol - point.MarketVolatility;
                sum += Math.Pow(diff, 2);
            }

            return sum;
        }
    }
}
