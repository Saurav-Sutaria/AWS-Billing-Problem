using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Globalization;

namespace Enhancement0
{
    internal class Instance
    {
        [Name("Instance Type")]
        public string Type { get; set; }

        [Name("Charge/Hour")]
        public string Charge { get; set; }
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
                record.Charge = record.Charge.Substring(1);
                instanceMap[record.Type] = record;
                instanceMap[record.Type].ids = new HashSet<string>();
                instanceMap[record.Type].sessions = new List<Session>();
                instanceMap[record.Type].monthlySessions = new Dictionary<string, MonthSession>();
            }
        }
        public static void PrintInstance(Instance i)
        {
            Console.Write(i.Type + " , ");
            Console.Write(i.Charge + "\n");
        }

    }
}