using FxOptionsEngine.Calibration.SabrCalibration;
using FxOptionsEngine.Data;
using FxOptionsEngine.Model;
using FxOptionsEngine.Model.Sabr;
using ScottPlot;
using ScottPlot.WinForms;

namespace FxOptionsEngine.Surfaces
{
    public sealed class SabrVolatilitySurface : IVolatilitySurface
    {
        private readonly IVolatilityModel<SabrParams> model;
        private readonly ForwardCurve forwardCurve;
        private readonly SabrVolatilityCalibration sabrCalibration;

        // todo: implement an LRU cache to prevent out of memory issues
        private readonly Dictionary<double, SabrParams> sabrParamsByExpiry = new();

        // todo: input this in some sort of file
        private static readonly double[] Expiries =
        {
            0.25,
            0.5,
            1.0
        };

        public SabrVolatilitySurface(IVolatilityModel<SabrParams> model, ForwardCurve forwardCurve)
        {
            this.model = model;
            this.forwardCurve = forwardCurve;
            sabrCalibration = new SabrVolatilityCalibration(model);

            foreach (double timeToExpiry in Expiries)
            {
                double forward = forwardCurve.GetForwardPrice(timeToExpiry);
                var vols = OptionsProvider.FetchData(forward, timeToExpiry);
                sabrParamsByExpiry[timeToExpiry] = sabrCalibration.Calibrate(forward, timeToExpiry, vols);
            }
        }

        public double GetVolatility(double strike, double timeToExpiry)
        {
            double forward = forwardCurve.GetForwardPrice(timeToExpiry);
            var parameters = GetParameters(timeToExpiry);

            return model.BlackVolatility(forward, strike, timeToExpiry, parameters);
        }

        public void GenereateSurfaceGraph()
        {
            int strikeSteps = 50;
            int timeSteps = 50;

            double minStrike = 0.90;
            double maxStrike = 1.20;

            double minTimeToExiry = 0.1;
            double maxTimeToExiry = 3.0;

            double[,] vols = new double[timeSteps, strikeSteps];

            for (int i = 0; i < timeSteps; i++)
            {
                double t = minTimeToExiry + i * (maxTimeToExiry - minTimeToExiry) / (timeSteps - 1);

                for (int j = 0; j < strikeSteps; j++)
                {
                    double strike = minStrike + j * (maxStrike - minStrike) / (strikeSteps - 1);
                    vols[i, j] = GetVolatility(strike, t);
                }
            }
            
            var plot = new Plot();
            plot.Add.Heatmap(vols);
            plot.Title("SABR Implied Volatility Surface");
            plot.XLabel("Strike");
            plot.YLabel("Time to Expiry");

            FormsPlotViewer.Launch(plot);
        }

        public SabrParams GetParameters(double timeToExpiry)
        {
            var (leftParameter, rightParameter) = FindParameterBracket(timeToExpiry);

            var p1 = sabrParamsByExpiry[leftParameter];
            var p2 = sabrParamsByExpiry[rightParameter];

            double alpha = Math.Exp(LinearlyInterpolate(timeToExpiry, leftParameter, rightParameter, p1.Alpha, p2.Alpha));
            double rho = LinearlyInterpolate(timeToExpiry, leftParameter, rightParameter, p1.Rho, p2.Rho);
            double v = LinearlyInterpolate(timeToExpiry, leftParameter, rightParameter, p1.VolOfVol, p2.VolOfVol);

            return new SabrParams(alpha, p1.Beta, rho, v);
        }

        private (double, double) FindParameterBracket(double timeToExpiry)
        {
            var expiries = sabrParamsByExpiry.Keys.OrderBy(e => e).ToArray();

            if (timeToExpiry <= expiries[0])
                return (expiries[0], expiries[0]);

            if (timeToExpiry >= expiries[^1])
                return (expiries[^1], expiries[^1]);

            for (int i = 0; i < expiries.Length - 1; i++)
            {
                if (timeToExpiry >= expiries[i] && timeToExpiry <= expiries[i + 1])
                    return (expiries[i], expiries[i + 1]);
            }

            throw new InvalidOperationException("Failed to bracket expiry");
        }

        private static double LinearlyInterpolate(double x, double x1, double x2, double y1, double y2)
        {
            if (x1 == x2)
                return y1;

            double w = (x - x1) / (x2 - x1);
            return y1 + w * (y2 - y1);
        }
    }
}
