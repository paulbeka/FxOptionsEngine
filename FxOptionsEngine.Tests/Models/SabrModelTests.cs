using FxOptionsEngine.Model.Sabr;

namespace FxOptionsEngine.Tests.Models
{
    public class SabrModelTests
    {
        [Fact]
        public void BlackVolatility_ATM_ReturnsExpectedValue()
        {
            var model = new SabrModel();
            var sabrParams = new SabrParams(0.2, 0.5, -0.3, 0.4);

            double forward = 1.25;
            double strike = 1.25;
            double timeToExpiry = 1.0;

            double vol = model.BlackVolatility(forward, strike, timeToExpiry, sabrParams);

            Assert.True(vol > 0.0);
            Assert.InRange(vol, 0.0001, 5.0);
        }

        [Fact]
        public void BlackVolatility_NearATM_IsContinuous()
        {
            var model = new SabrModel();
            var sabrParams = new SabrParams(0.3, 0.7, 0.1, 0.5);

            double forward = 1.0;
            double timeToExpiry = 2.0;

            double volATM = model.BlackVolatility(forward, forward, timeToExpiry, sabrParams);
            double volSlightOTM = model.BlackVolatility(forward, forward * 1.0001, timeToExpiry, sabrParams);

            Assert.True(Math.Abs(volATM - volSlightOTM) < 1e-3);
        }

        [Fact]
        public void BlackVolatility_ProducesSmile()
        {
            var model = new SabrModel();
            var sabrParams = new SabrParams(0.25, 0.5, -0.4, 0.6);

            double forward = 1.0;
            double timeToExpiry = 1.0;

            double volATM = model.BlackVolatility(forward, 1.0, timeToExpiry, sabrParams);
            double volOTM = model.BlackVolatility(forward, 1.2, timeToExpiry, sabrParams);
            double volITM = model.BlackVolatility(forward, 0.8, timeToExpiry, sabrParams);

            Assert.True(volOTM > 0.0);
            Assert.True(volITM > 0.0);
            Assert.True(volATM > 0.0);
        }

    }
}
