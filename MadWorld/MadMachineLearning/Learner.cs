using System.Collections.Generic;
using System.IO;
using MadMachineLearning.NS;
using MadMachineLearning.NS.Models;
using Microsoft.ML;

namespace MadMachineLearning
{
    public class Learner
    {
        public void Start()
        {
            string baseLocation = "../../../../MadMachineLearning/NS/Data/";
            string modelSaveLocartion = $"{baseLocation}/DisruptionModel.zip";

            NsReader reader = new();

            string locationTrain = $"{baseLocation}disruptions-2019-train.csv";
            List<DisruptionsRaw> disruptionsRawTrain = reader.GetDisruptions(locationTrain);
            List<DisruptionsML> disruptionsTrain = NsConverter.Convert(disruptionsRawTrain);

            string locationTest = $"{baseLocation}disruptions-2019-test.csv";
            List<DisruptionsRaw> disruptionsRawTest = reader.GetDisruptions(locationTest);
            List<DisruptionsML> disruptionsTest = NsConverter.Convert(disruptionsRawTest);

            MLContext mlContext = new();
            IDataView trainingDataTrain = mlContext.Data.LoadFromEnumerable(disruptionsTrain);
            IDataView trainingDataTest = mlContext.Data.LoadFromEnumerable(disruptionsTest);

            var dataProcessPipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: nameof(DisruptionsML.DurationMinutes))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "StatisticalCauseNL", inputColumnName: nameof(DisruptionsML.StatisticalCauseNL)))
                .Append(mlContext.Transforms.Concatenate("Features", "StatisticalCauseNL"));

            var trainer = mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features");
            var trainingPipeline = dataProcessPipeline.Append(trainer);

            ITransformer trainedModel = trainingPipeline.Fit(trainingDataTrain);
            IDataView predictions = trainedModel.Transform(trainingDataTest);

            var metrics = mlContext.Regression.Evaluate(predictions, labelColumnName: "Label", scoreColumnName: "Score");

            if (File.Exists(modelSaveLocartion))
            {
                File.Delete(modelSaveLocartion);
            }

            mlContext.Model.Save(trainedModel, trainingDataTrain.Schema, modelSaveLocartion);

            ITransformer trainedModelLoaded = mlContext.Model.Load(modelSaveLocartion, out var modelInputSchema);

            // Create prediction engine related to the loaded trained model

            DisruptionsML testPrediction = new DisruptionsML
            {
                StatisticalCauseNL = "brandmelding",
                DurationMinutes = 0
            };

            var predEngine = mlContext.Model.CreatePredictionEngine<DisruptionsML, DisruptionPrediction>(trainedModelLoaded);

            //Score
            var resultprediction = predEngine.Predict(testPrediction);
        }
    }
}
