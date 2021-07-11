using System;
using Microsoft.ML.Data;

namespace MadMachineLearning.NS.Models
{
    public class DisruptionPrediction
    {
        [ColumnName("Score")]
        public float Duration;
    }
}
