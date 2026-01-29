using FxOptionsEngine.Model;

namespace FxOptionsEngine.Surfaces
{
    public sealed class SabrVolatilitySurface : IVolatilitySurface
    {

        private readonly ISabrModel model;

        public SabrVolatilitySurface(ISabrModel model)
        {
            this.model = model;
        }

        public double GetVolatility(double strike, double timeToExpiry)
        {
            var parameters = GetParameters();
            double forward = GetForward(timeToExpiry);

            return model.BlackVolatility(forward, strike, timeToExpiry, parameters);
        }

        public void genereateSurfaceGraph()
        {
            return;
        }
    }
}
