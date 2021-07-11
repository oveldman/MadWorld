using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.TypeConversion;
using MadMachineLearning.NS.Models;

namespace MadMachineLearning.NS
{
    public class NsReader
    {
        public List<DisruptionsRaw> GetDisruptions(string location)
        {
            using (var reader = new StreamReader(location, Encoding.Default))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
            {
                csv.Context.RegisterClassMap<DisruptionsMapping>();
                return csv.GetRecords<DisruptionsRaw>().ToList();
            }
        }
    }
}
