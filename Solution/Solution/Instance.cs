using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution
{
    internal class Instance
    {
        [Name("Instance Type")]
        public string Type {  get; set; }

        [Name("Charge/Hour")]
        public string Charge { get; set; }
        public HashSet<string> ids;
        public List<Session> sessions;
        public string totalTime;
        public string billedTime;
        public string amount;
    }
}
