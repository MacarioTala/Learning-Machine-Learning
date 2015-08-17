using System;
using Encog.ML.Data.Basic;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Training.Propagation.Resilient;

namespace MyFirstSkynet
{
    public class BasicNeuralNetFunctions
    {
        public ResilientPropagation TrainNetwork(BasicNetwork network, BasicMLDataSet trainingData)
        {
            var trainedNetwork = new ResilientPropagation(network, trainingData);
            var epoch = 0;
            do
            {
                trainedNetwork.Iteration();
                epoch++;
                Console.WriteLine("Epoch:{0}, Error{1}", epoch, trainedNetwork.Error);
            } while (trainedNetwork.Error > 0.01);

            return trainedNetwork;
        }

        public void EvaluateNetwork(BasicNetwork trainedNetwork, BasicMLDataSet trainingData)
        {
            foreach (var trainingItem in trainingData)
            {
                var output = trainedNetwork.Compute(trainingItem.Input);
                Console.WriteLine("Input:{0}, {1}  Ideal: {2}  Actual : {3}", trainingItem.Input[0], trainingItem.Input[1], trainingItem.Ideal, output[0]);
            }
            Console.ReadKey();
        }

       
    }
}
