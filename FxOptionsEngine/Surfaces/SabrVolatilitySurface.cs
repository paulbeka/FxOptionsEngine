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
            return 0.0;
        }

        public void genereateSurfaceGraph()
        {
            return;
        }
    }
}
