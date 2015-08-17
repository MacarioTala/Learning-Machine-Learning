using Encog.Engine.Network.Activation;
using Encog.ML.Data.Basic;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;

namespace MyFirstSkynet
{
    class Program
    {
        static void Main(string[] args)
        {
            var network = CreateNetwork();
            var trainingData = new BasicMLDataSet(XORStatics.XorInputMatrix,XORStatics.XorIdealMatrix);

            var functions = new BasicNeuralNetFunctions();

            var trainedNetwork = functions.TrainNetwork(network, trainingData);

            functions.EvaluateNetwork(network,trainingData);
        }
        private static BasicNetwork CreateNetwork()
        {
            var xorNetwork = new BasicNetwork();
            xorNetwork.AddLayer(new BasicLayer(null, true, 2));
            xorNetwork.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 2));
            xorNetwork.AddLayer(new BasicLayer(new ActivationSigmoid(), false, 1));
            xorNetwork.Structure.FinalizeStructure();
            xorNetwork.Reset();
            return xorNetwork;
        }
    }
}
