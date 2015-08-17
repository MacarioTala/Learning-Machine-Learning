using System;
using System.IO;
using Encog.App.Analyst;
using Encog.App.Analyst.CSV.Normalize;
using Encog.App.Analyst.CSV.Segregate;
using Encog.App.Analyst.CSV.Shuffle;
using Encog.App.Analyst.Wizard;
using Encog.Engine.Network.Activation;
using Encog.MathUtil;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Persist;
using Encog.Util.CSV;
using Encog.Util.Simple;

namespace IrisClassifier
{
    class Program
    {
        static void Main(string[] args)
        {
            var fo = new FileOps(@"C:\programming\Machine Learning\MyFirstSkynet\IrisClassifier\Data", "IrisData");
            ShuffleDataFile(fo);
            Segregate(fo);
            Normalise(fo);
            CreateNetwork(fo);
            TrainNetwork(fo);
            Evaluate(fo);
        }

        public static void ShuffleDataFile(FileOps fileOps)
        {   
            var shuffle = new ShuffleCSV{ProduceOutputHeaders = true};
            shuffle.Analyze(fileOps.BaseFile,true,CSVFormat.English);
            shuffle.Process(fileOps.ShuffledFile);
            
        }

        public static void Segregate(FileOps fileOps)
        {   var segregator = new SegregateCSV{ProduceOutputHeaders = true};
            segregator.Targets.Add(new SegregateTargetPercent(fileOps.TrainingFile,75));
            segregator.Targets.Add(new SegregateTargetPercent(fileOps.EvaluationFile, 25));
            segregator.Analyze(fileOps.ShuffledFile,true,CSVFormat.English);
            segregator.Process();
        }

        public static void Normalise(FileOps fileOps)
        {
            var analyst = new EncogAnalyst();

            var wizard = new AnalystWizard(analyst);
            wizard.Wizard(fileOps.BaseFile,true,AnalystFileFormat.DecpntComma);

            var norm = new AnalystNormalizeCSV{ProduceOutputHeaders = true};
            norm.Analyze(fileOps.TrainingFile, true, CSVFormat.English, analyst);
            norm.Normalize(fileOps.NormalisedTrainingFile);
            norm.Analyze(fileOps.EvaluationFile, true, CSVFormat.English, analyst);
            norm.Normalize(fileOps.NormalisedEvaluationFile);

            analyst.Save(fileOps.AnalystFile);
            
        }

        public static void CreateNetwork(FileOps fileOps)
        {
            var network = new BasicNetwork();
            network.AddLayer(new BasicLayer(new ActivationLinear(),true,4));
            network.AddLayer(new BasicLayer(new ActivationTANH(), true, 6));
            network.AddLayer(new BasicLayer(new ActivationTANH(), true, 2));
            network.Structure.FinalizeStructure();
            network.Reset();
            EncogDirectoryPersistence.SaveObject(fileOps.TrainedNeuralNetworkFile, network);
        }

        public static void TrainNetwork(FileOps fileOps)
        {
            var network = (BasicNetwork)EncogDirectoryPersistence.LoadObject(fileOps.TrainedNeuralNetworkFile);
            var trainingSet = EncogUtility.LoadCSV2Memory(fileOps.NormalisedTrainingFile.ToString(), network.InputCount,
                network.OutputCount, true, CSVFormat.English, false);

            var train = new ResilientPropagation(network, trainingSet);
            int epoch = 1;
            do
            {
                train.Iteration();
                Console.WriteLine("Epoch: {0} Error: {1}",epoch,train.Error);
                epoch++;
            } while (train.Error > 0.01);
            Console.ReadKey();
            EncogDirectoryPersistence.SaveObject(fileOps.TrainedNeuralNetworkFile,network);
        }

        public static void Evaluate(FileOps fileOps)
        {
            var network = (BasicNetwork) EncogDirectoryPersistence.LoadObject(fileOps.TrainedNeuralNetworkFile);
            var analyst = new EncogAnalyst();
            analyst.Load(fileOps.AnalystFile);
            var evaluationSet = EncogUtility.LoadCSV2Memory(fileOps.NormalisedEvaluationFile.ToString(),
                network.InputCount, network.OutputCount, true, CSVFormat.English, false);
            var iteration = 0;
            var hitCount = 0;
            foreach (var evaluation in evaluationSet)
            {
                iteration++;
                var output = network.Compute(evaluation.Input);

                var sepalL = analyst.Script.Normalize.NormalizedFields[0].DeNormalize(evaluation.Input[0]);
                var sepalW = analyst.Script.Normalize.NormalizedFields[0].DeNormalize(evaluation.Input[1]);
                var petalL = analyst.Script.Normalize.NormalizedFields[0].DeNormalize(evaluation.Input[2]);
                var petalW = analyst.Script.Normalize.NormalizedFields[0].DeNormalize(evaluation.Input[3]);

                var classCount = analyst.Script.Normalize.NormalizedFields[4].Classes.Count;

                var normalisationHigh = analyst.Script.Normalize.NormalizedFields[4].NormalizedHigh;
                var normalisationLow = analyst.Script.Normalize.NormalizedFields[4].NormalizedLow;

                var eq = new Equilateral(classCount, normalisationHigh, normalisationLow);
                var predictedClassInt = eq.Decode(output);
                var predictedClass = analyst.Script.Normalize.NormalizedFields[4].Classes[predictedClassInt].Name;
                var idealClassInt = eq.Decode(evaluation.Ideal);
                var idealClass = analyst.Script.Normalize.NormalizedFields[4].Classes[idealClassInt].Name;

                Console.WriteLine("Predicted: {0} Ideal: {1}",predictedClass,idealClass);
                if (predictedClass == idealClass)
                {
                    hitCount++;
                }
               
            }
            Console.WriteLine("Total Test Count:{0}",iteration);
            Console.WriteLine("Total Correct Predictions: {0}",hitCount);
            Console.WriteLine("Success rate: {0}%", (((float)hitCount/(float)iteration)*100f));
            Console.ReadKey();
        }
    }
}
