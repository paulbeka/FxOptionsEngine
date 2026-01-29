using FxOptionsEngine.Calibration.SabrCalibration;
using FxOptionsEngine.Data;
using FxOptionsEngine.Model;

namespace FxOptionsEngine.Surfaces
{
    public sealed class SabrVolatilitySurface : IVolatilitySurface
    {

        private readonly ISabrModel model;
        private readonly ForwardCurve forwardCurve;
        private readonly SabrCalibration sabrCalibration;

        private readonly double FIXED_BETA = 0.5f;  // todo: compare with other fixed beta and choose lowest error

        public SabrVolatilitySurface(ISabrModel model, ForwardCurve forwardCurve)
        {
            this.model = model;
            this.sabrCalibration = new SabrCalibration(model);
            this.forwardCurve = forwardCurve;
        }

        public double GetVolatility(
            List<StrikeToMarketVolatility> marketVolPoints, 
            double strike, 
            double timeToExpiry)
        {
            var parameters = GetParameters(marketVolPoints, strike, timeToExpiry);
            double forward = forwardCurve.GetForwardPrice(timeToExpiry);

            return model.BlackVolatility(forward, strike, timeToExpiry, parameters);
        }

        public void genereateSurfaceGraph()
        {
            return;
        }

        private SabrParams GetParameters(
            List<StrikeToMarketVolatility> marketVolPoints,
            double strike, 
            double timeToExpiry)
        {

            return sabrCalibration.Calibrate(marketVolPoints, strike, timeToExpiry, FIXED_BETA);
        }
    }
}
