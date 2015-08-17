using System;
using System.IO;

namespace IrisClassifier
{
    public class FileOps
    {
        public string DataPath { get; set; }
        public string FileName { get; set; }

        private const string PathSeparator = @"\";
        private const string Csv = ".csv";
        private const string Ega = ".ega";
        private const string Eg = ".eg";
        private const string Shuffled = "_shuffled";
        private const string Train = "Train";
        private const string Evaluate = "Evaluate";
        private const string Normalised = "Normalised";
        private const string Analyst = "Analyst";


        public FileInfo BaseFile
        {
            get { return GetBaseFile(); }
        }

        public FileInfo ShuffledFile
        {
            get { return GetShuffledFile(); }
        }

        public FileInfo TrainingFile
        {
            get { return GetTrainingFile(); }
        }

        public FileInfo EvaluationFile
        {
            get { return GetEvaluationFile(); }
        }

        public FileInfo NormalisedTrainingFile { get { return GetNormalisedTrainingFile(); } }

        public FileInfo NormalisedEvaluationFile { get { return GetNormalisedEvaluationFile(); } }
        public FileInfo AnalystFile { get { return GetAnalystFile(); } }
        public FileInfo TrainedNeuralNetworkFile { get { return GetTrainedNeuralNetworkFile(); } }

        private FileInfo GetTrainedNeuralNetworkFile()
        {
            return new FileInfo(DataPath + PathSeparator + FileName + Train + Eg);
        }

        private FileInfo GetAnalystFile()
        {
            return new FileInfo(DataPath + PathSeparator + FileName + Analyst + Ega);
        }

        private FileInfo GetNormalisedEvaluationFile()
        {
            return new FileInfo(DataPath + PathSeparator + FileName + Evaluate + Normalised + Csv);
        }

        private FileInfo GetNormalisedTrainingFile()
        {
            return new FileInfo(DataPath + PathSeparator + FileName + Train+Normalised + Csv);
        }

        private FileInfo GetEvaluationFile()
        {
            return new FileInfo(DataPath + PathSeparator + FileName + Evaluate + Csv);
        }

        private FileInfo GetTrainingFile()
        {
            return new FileInfo(DataPath + PathSeparator+FileName+Train+Csv);
        }

        private FileInfo GetShuffledFile()
        {
            return new FileInfo(DataPath + PathSeparator+FileName+ Shuffled + Csv);
        }

        public FileOps(string path, string fileName)
        {
            DataPath = path;
            FileName = fileName;
        }
        private FileInfo GetBaseFile()
        {
            return new FileInfo(DataPath+PathSeparator+FileName+Csv);
        }
    }
}
