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

        // todo: replace this with live market data
        private readonly List<StrikeToMarketVolatility> marketVolPoints = new()
        {
            new(0.90f, 0.14f),
            new(0.95f, 0.12f),
            new(1.00f, 0.11f),
            new(1.05f, 0.12f),
            new(1.10f, 0.14f)
        };

        public SabrVolatilitySurface(ISabrModel model, ForwardCurve forwardCurve)
        {
            this.model = model;
            this.forwardCurve = forwardCurve;

            sabrCalibration = new SabrCalibration(model, marketVolPoints);
        }

        public double GetVolatility(double strike, double timeToExpiry)
        {
            var parameters = GetParameters(strike, timeToExpiry);
            double forward = forwardCurve.GetForwardPrice(timeToExpiry);

            return model.BlackVolatility(forward, strike, timeToExpiry, parameters);
        }

        public void genereateSurfaceGraph()
        {
            return;
        }

        private SabrParams GetParameters(double strike, double timeToExpiry)
        {
            SabrParams calibration = sabrCalibration.Calibrate(strike, timeToExpiry);
            return calibration;
        }
    }
}
