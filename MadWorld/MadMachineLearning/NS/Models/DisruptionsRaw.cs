using System;
namespace MadMachineLearning.NS.Models
{
    public class DisruptionsRaw
    {
        public int ID { get; set; }
        public string Lines { get; set; }
        public string RdtLines { get; set; }
        public string RdtLinesIDs { get; set; }
        public string RdtStationNames { get; set; }
        public string RdtStationCodes { get; set; }
        public string CauseNL { get; set; }
        public string CauseEN { get; set; }
        public string StatisticalCauseNL { get; set; }
        public string StatisticalCauseEN { get; set; }
        public string CauseGroup { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string DurationMinutes { get; set; }
    }
}
