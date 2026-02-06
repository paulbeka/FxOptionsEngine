namespace FxOptionsEngine.Surfaces
{
    public interface IVolatilitySurface
    {
        double GetVolatility(double strike, double timeToExpiry);

        void GenereateSurfaceGraph();
    }
}
