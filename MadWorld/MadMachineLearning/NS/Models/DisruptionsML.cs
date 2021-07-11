using System;
using Microsoft.ML.Data;

namespace MadMachineLearning.NS.Models
{
    public class DisruptionsML
    {
        [LoadColumn(0)]
        public int ID { get; set; }
        [LoadColumn(1)]
        public string StatisticalCauseNL { get; set; }
        [LoadColumn(2)]
        public string LineName { get; set; }
        [LoadColumn(3)]
        public string CauseGroup { get; set; }
        [LoadColumn(4)]
        public float DurationMinutes { get; set; }
    }
}
