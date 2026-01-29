using System;
using System.Collections.Generic;
using System.Text;

namespace FxOptionsEngine.Surfaces
{
    internal interface IVolatilitySurface
    {
        double GetVolatility(double strike, double timeToExpiry);

        void GenereateSurfaceGraph();
    }
}
