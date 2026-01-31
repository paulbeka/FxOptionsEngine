using System;
using System.Collections.Generic;
using Xunit;
using FxOptionsEngine.Data;

namespace FxOptionsEngine.Tests
{
    public class DiscountCurveTests
    {
        private static DiscountCurve CreateSimpleCurve()
        {
            return new DiscountCurve(new SortedDictionary<double, double>
            {
                { 0.5, 1 },
                { 1.0, 0.97 },
                { 2.0, 0.94 }
            });
        }

        [Fact]
        public void GetDiscountFactor_BeforeFirstNode_ReturnsFirstDiscountFactor()
        {
            var curve = CreateSimpleCurve();
            Assert.Equal(1, curve.GetDiscountFactor(0.1), 10);
        }

        [Fact]
        public void GetDiscountFactor_AfterLastNode_ReturnsLastDiscountFactor()
        {
            var curve = CreateSimpleCurve();
            Assert.Equal(0.94, curve.GetDiscountFactor(5.0), 10);
        }


        [Fact]
        public void GetDiscountFactor_InterpolatesBetweenNodes()
        {
            var curve = CreateSimpleCurve();

            double t = 0.75;

            double df1 = Math.Log(1);
            double df2 = Math.Log(0.97);
            double w = (t - 0.5) / (1.0 - 0.5);

            double expected = Math.Exp(df1 + w * (df2 - df1));

            double actual = curve.GetDiscountFactor(t);

            Assert.Equal(expected, actual, 10);
        }

        [Fact]
        public void GetDiscountFactor_Monotonicity_IsPreserved()
        {
            var curve = CreateSimpleCurve();

            double dfShort = curve.GetDiscountFactor(0.6);
            double dfLong = curve.GetDiscountFactor(1.5);

            Assert.True(dfShort > dfLong);
        }
    }
}
