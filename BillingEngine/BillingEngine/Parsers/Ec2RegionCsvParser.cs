using BillingEngine.Parsers.Models;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BillingEngine.Parsers
{
    public class Ec2RegionCsvParser
    {
        public List<ParsedEc2Region> Parse(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                return csv.GetRecords<ParsedEc2Region>().ToList();
        }
    }
}