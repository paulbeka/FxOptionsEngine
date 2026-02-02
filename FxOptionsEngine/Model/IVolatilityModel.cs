using FxOptionsEngine.Model.Sabr;

namespace FxOptionsEngine.Model
{
    public interface IVolatilityModel<TParams>
    {
        public double BlackVolatility(
            double forward,
            double strike,
            double timeToExpiry,
            TParams sabrParams);
    }
}
