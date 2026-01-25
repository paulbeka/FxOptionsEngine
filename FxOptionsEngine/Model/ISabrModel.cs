using System;
using System.Collections.Generic;
using System.Text;

namespace FxOptionsEngine.Model
{
    public interface ISabrModel
    {
        public double BlackVolatility(
            double forward,
            double strike,
            double timeToExpiry,
            SabrParams sabrParams);
    }
}
