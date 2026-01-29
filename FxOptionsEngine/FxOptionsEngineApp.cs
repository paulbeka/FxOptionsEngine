using FxOptionsEngine.Data;
using FxOptionsEngine.Model;
using FxOptionsEngine.Surfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FxOptionsEngine
{
    internal class FxOptionsEngineApp
    {
        public void Run()
        {
            var usdCurve = new DiscountCurve(new SortedDictionary<double, double>
            {
                { 0.25, 0.997 },
                { 0.5,  0.990 },
                { 1.0,  0.980 },
                { 2.0,  0.963 }
            });

            var eurCurve = new DiscountCurve(new SortedDictionary<double, double>
            {
                { 0.25, 0.992 },
                { 0.5,  0.994 },
                { 1.0,  0.996 },
                { 2.0,  0.999 }
            });

            ForwardCurve forwardCurve = new(1.0, usdCurve, eurCurve);

            IVolatilitySurface volatilitySurface = new SabrVolatilitySurface(new SabrModel(), forwardCurve);

            Console.WriteLine($"Vol @ 1 strike & 2 TTE: {volatilitySurface.GetVolatility(1.0f, 2.0f)}");
        }  
    }
}
