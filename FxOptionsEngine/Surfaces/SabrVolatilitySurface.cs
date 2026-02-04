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
        private readonly Dictionary<double, List<StrikeToMarketVolatility>> marketDataByExpiry;

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

            marketDataByExpiry = new Dictionary<double, List<StrikeToMarketVolatility>>();

            foreach (double timeToExpiry in Expiries)
            {
                double forward = forwardCurve.GetForwardPrice(timeToExpiry);
                var vols = OptionsProvider.FetchData(forward, timeToExpiry);
                marketDataByExpiry[timeToExpiry] = vols;
            }

            sabrCalibration = new SabrVolatilityCalibration(model);
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

        private SabrParams GetParameters(double forward, double timeToExpiry)
        {
            if (!sabrParamsByExpiry.TryGetValue(timeToExpiry, out var calibration))
            {
                calibration = sabrCalibration.Calibrate(forward, timeToExpiry, marketDataByExpiry[timeToExpiry]);
                sabrParamsByExpiry[timeToExpiry] = calibration;
            }
            return calibration;
        }
    }
}
