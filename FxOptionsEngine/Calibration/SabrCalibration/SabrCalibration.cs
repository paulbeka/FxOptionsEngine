using FxOptionsEngine.Model;

namespace FxOptionsEngine.Calibration.SabrCalibration
{
    public sealed class SabrCalibration
    {
        
        private readonly ISabrModel model;

        public SabrCalibration(ISabrModel model) 
        {
            this.model = model;
        }

        public SabrParams Calibrate(
            List<StrikeToMarketVolatility> marketVolPoints, 
            double forward,
            double timeToExpiry,
            double fixedBeta)
        {

            double alpha = 0.5f;
            double beta = 0.5f;
            double v = 0.5f;
            double rho = 0.5f;
            return new SabrParams(alpha, beta, v, rho);
        }
    }
}
