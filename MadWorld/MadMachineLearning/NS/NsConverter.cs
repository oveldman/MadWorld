using System;
using System.Collections.Generic;
using MadMachineLearning.NS.Models;

namespace MadMachineLearning.NS
{
    public class NsConverter
    {
        public static List<DisruptionsML> Convert(List<DisruptionsRaw> disruptionsRaws)
        {
            List<DisruptionsML> disruptionsML = new();

            foreach (DisruptionsRaw disruption in disruptionsRaws)
            {
                disruptionsML.Add(new DisruptionsML
                {
                    ID = disruption.ID,
                    StatisticalCauseNL = disruption.StatisticalCauseNL,
                    CauseGroup = disruption.CauseGroup,
                    DurationMinutes = float.TryParse(disruption.DurationMinutes, out float duration) ? duration : 0
                });
            }

            return disruptionsML;
        }
    }
}
