using System;
using CsvHelper.Configuration;

namespace MadMachineLearning.NS.Models
{
    public sealed class DisruptionsMapping : ClassMap<DisruptionsRaw>
    {
        public DisruptionsMapping()
        {
            Map(x => x.ID).Name("rdt_id");
            Map(x => x.Lines).Name("ns_lines");
            Map(x => x.RdtLines).Name("rdt_lines");
            Map(x => x.RdtLinesIDs).Name("rdt_lines_id");
            Map(x => x.RdtStationNames).Name("rdt_station_names");
            Map(x => x.RdtStationCodes).Name("rdt_station_codes");
            Map(x => x.CauseNL).Name("cause_nl");
            Map(x => x.CauseEN).Name("cause_en");
            Map(x => x.StatisticalCauseNL).Name("statistical_cause_nl");
            Map(x => x.StatisticalCauseEN).Name("statistical_cause_en");
            Map(x => x.CauseGroup).Name("cause_group");
            Map(x => x.StartTime).Name("start_time");
            Map(x => x.EndTime).Name("end_time");
            Map(x => x.DurationMinutes).Name("duration_minutes");
        }
    }
}
