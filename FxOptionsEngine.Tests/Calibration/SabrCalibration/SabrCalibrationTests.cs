using FxOptionsEngine.Calibration.SabrCalibration;
using FxOptionsEngine.Model.Sabr;

namespace FxOptionsEngine.Tests.Calibration.SabrCalibration
{
    public class SabrCalibrationTests
    {
        [Fact]
        public void SabrCalibratior_ReturnsExpectedParameters()
        {
            List<StrikeToMarketVolatility> marketVolPoints =
            [
                new(0.90f, 0.14f),
                new(0.95f, 0.12f),
                new(1.00f, 0.11f),
                new(1.05f, 0.12f),
                new(1.10f, 0.14f)
            ];
            SabrVolatilityCalibration calibrator = new(new SabrModel(), marketVolPoints);

            double strike = 1.0f;
            double timeToExpiry = 2.0f;

            SabrParams expectedParameters = new SabrParams(
                0.12571933093026666, 
                0.5f,
                0.821062524511184,
                0.064616403159745942);
            SabrParams parameters = calibrator.Calibrate(strike, timeToExpiry);

            Assert.Equal(expectedParameters, parameters);
        }

        [Fact]
        public void SabrCalibrator_DoesNotFailOnExtremeValues()
        {
            List<StrikeToMarketVolatility> marketVolPoints =
            [
                new(0.5f, 0.80f),
                new(0.7f, 0.602f),
                new(1.00f, 0.11f),
                new(1.3f, 0.28f),
                new(1.9f, 1.98f)
            ];
            SabrVolatilityCalibration calibrator = new(new SabrModel(), marketVolPoints);

            double strike = 1.0f;
            double timeToExpiry = 2.0f;

            SabrParams expectedParameters = new SabrParams(
                0.49152252804298807,
                0.5f,
                0.58281175897107707,
                1.4999999999927236);
            SabrParams parameters = calibrator.Calibrate(strike, timeToExpiry);

            Assert.Equal(expectedParameters, parameters);
        }

        [Fact]
        public void SabrCalibrator_DoesNotAcceptInvalidStrike()
        {
            List<StrikeToMarketVolatility> marketVolPoints =
            [
                new(0.5f, 0.80f),
                new(0.7f, 0.602f),
                new(1.00f, 0.11f),
                new(1.3f, 0.28f),
                new(1.9f, 1.98f)
            ];
            SabrVolatilityCalibration calibrator = new(new SabrModel(), marketVolPoints);

            double strike = -1.0f;
            double timeToExpiry = 2.0f;

            Assert.Throws<ArgumentOutOfRangeException>(() => calibrator.Calibrate(strike, timeToExpiry));
        }

        [Fact]
        public void SabrCalibrator_DoesNotAcceptInvalidTimeToExpiry()
        {
            List<StrikeToMarketVolatility> marketVolPoints =
            [
                new(0.5f, 0.80f),
                new(0.7f, 0.602f),
                new(1.00f, 0.11f),
                new(1.3f, 0.28f),
                new(1.9f, 1.98f)
            ];
            SabrVolatilityCalibration calibrator = new(new SabrModel(), marketVolPoints);

            double strike = 1.0f;
            double timeToExpiry = -2.0f;

            Assert.Throws<ArgumentOutOfRangeException>(() => calibrator.Calibrate(strike, timeToExpiry));
        }
    }
}
