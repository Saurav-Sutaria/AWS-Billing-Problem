using BillingEngine.Parsers.Models;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingEngine.Parsers
{
    internal class Ec2ReservedResourceUsageParser
    {
        public List<ParsedEc2ReservedResourceUsage> Parse(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                return csv.GetRecords<ParsedEc2ReservedResourceUsage>().ToList();
        }
    }
}
