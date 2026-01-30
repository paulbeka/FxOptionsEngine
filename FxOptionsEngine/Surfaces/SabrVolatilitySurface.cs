using FxOptionsEngine.Calibration.SabrCalibration;
using FxOptionsEngine.Data;
using FxOptionsEngine.Model;
using ScottPlot;
using ScottPlot.WinForms;

namespace FxOptionsEngine.Surfaces
{
    public sealed class SabrVolatilitySurface : IVolatilitySurface
    {

        private readonly ISabrModel model;
        private readonly ForwardCurve forwardCurve;
        private readonly SabrVolatilityCalibration sabrCalibration;

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

            sabrCalibration = new SabrVolatilityCalibration(model, marketVolPoints);
        }

        public double GetVolatility(double strike, double timeToExpiry)
        {
            var parameters = GetParameters(strike, timeToExpiry);
            double forward = forwardCurve.GetForwardPrice(timeToExpiry);

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

        private SabrParams GetParameters(double strike, double timeToExpiry)
        {
            SabrParams calibration = sabrCalibration.Calibrate(strike, timeToExpiry);
            return calibration;
        }
    }
}
