using FxOptionsEngine.MarketData;

namespace FxOptionsEngine.Tests.MarketData.Volatility.Util
{
    public class ImpliedVolatilityCalculatorTests
    {
        [Fact]
        public void Test_CalculateImpliedVolatility()
        {
            double marketPrice = 10.0;
            double strikePrice = 100.0;
            double underlyingPrice = 105.0;
            double timeToMaturity = 0.5;
            double riskFreeRate = 0.01;
            double dividendYield = 0.02;

            double forward = underlyingPrice * Math.Exp((riskFreeRate - dividendYield) * timeToMaturity);
            double discountFactor = Math.Exp(-riskFreeRate * timeToMaturity);
            bool isCall = true;

            double impliedVolatility = ImpliedVolatilityCalculator.SolveImpliedVolatility(
                marketPrice,
                forward,
                strikePrice,
                timeToMaturity,
                discountFactor,
                isCall);

            Assert.InRange(impliedVolatility, 0.0, 1.0);
        }
    }
}