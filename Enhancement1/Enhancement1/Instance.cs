using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Globalization;

namespace Enhancement1
{
    internal class Instance
    {
        [Name("Instance Type")]
        public string Type { get; set; }

        [Name("Charge/Hour(OnDemand)")]
        public string OnDemandCharge { get; set; }
        //[Name("Charge/Hour(Reserved)")]
        //public string ReservedCharge { get; set; }
        [Name("Region")]
        public string Region { get; set; }
        public HashSet<string> ids;
        public List<Session> sessions;
        public Dictionary<string, MonthSession> monthlySessions; //map -> month Name-year (JUL-2021) with all session in that month


        public static List<Instance> ParseResources(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                return csv.GetRecords<Instance>().ToList();
        }
        public static void MapResources(Dictionary<string, Instance> instanceMap, List<Instance> records)
        {
            foreach (var record in records)
            {
                record.OnDemandCharge = record.OnDemandCharge.Substring(1);
                string id = record.Type + record.Region;
                instanceMap[id] = record;
                instanceMap[id].ids = new HashSet<string>();
                instanceMap[id].sessions = new List<Session>();
                instanceMap[id].monthlySessions = new Dictionary<string, MonthSession>();
            }
        }
        public static void PrintInstance(Instance i)
        {
            Console.WriteLine(i.Type + " , " + i.OnDemandCharge);
        }

    }
}