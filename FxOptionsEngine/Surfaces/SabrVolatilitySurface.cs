using FxOptionsEngine.Data;
using FxOptionsEngine.Model;

namespace FxOptionsEngine.Surfaces
{
    public sealed class SabrVolatilitySurface : IVolatilitySurface
    {

        private readonly ISabrModel model;
        private readonly ForwardCurve forwardCurve;

        public SabrVolatilitySurface(ISabrModel model, ForwardCurve forwardCurve)
        {
            this.model = model;
            this.forwardCurve = forwardCurve;
        }

        public double GetVolatility(double strike, double timeToExpiry)
        {
            var parameters = GetParameters();
            double forward = forwardCurve.GetForwardPrice(timeToExpiry);

            return model.BlackVolatility(forward, strike, timeToExpiry, parameters);
        }

        public void genereateSurfaceGraph()
        {
            return;
        }

        private SabrParams GetParameters()
        {
            double alpha = 0.5f;
            double beta = 0.5f;
            double v = 0.5f;
            double rho = 0.5f;
            return new SabrParams(alpha, beta, v, rho);
        }
    }
}
