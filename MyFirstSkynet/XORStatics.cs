using System;
using Encog.Engine.Network.Activation;
using Encog.ML.Data.Basic;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Resilient;

namespace MyFirstSkynet
{
    public static class XORStatics
    {
        public static readonly Double[][] XorInputMatrix =
        {
            new []{0.0,0.0},
            new []{1.0,0.0},
            new []{0.0,0.1},
            new []{1.0,1.0},

        };

        public static readonly double[][] XorIdealMatrix =
        {
            new[] {0.0},
            new[] {1.0},
            new[] {0.1},
            new[] {0.0},
        };

    }
}
