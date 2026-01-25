using FxOptionsEngine.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FxOptionsEngine
{
    internal class FxOptionsEngineApp
    {
        public void Run()
        {
            
            double alpha = 0.5f;     // the initial volatility
            double beta = 0.5f;      // Elasticity of the forwards
            double v = 0.5f;         // the volatility of the volatility
            double rho = 0.5f;       // correlation between beta and v

            SabrParams sabrParams = new(alpha, beta, v, rho);

            ISabrModel sabrModel = new SabrModel();

            double vol = sabrModel.BlackVolatility(
                forward: 1.25,
                strike: 1.20,
                timeToExpiry: 0.5,
                sabrParams: sabrParams
            );

            Console.WriteLine($"SABR Black vol: {vol}");
        }  
    }
}
