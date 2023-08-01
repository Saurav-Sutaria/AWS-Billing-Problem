using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Globalization;

namespace Enhancement1
{
    internal class Region
    {
        [Name("Region")]
        public string RegionName { get; set; }

        [Name("Free Tier Eligible")]
        public string FreeInstanceType { get; set; }

        public Dictionary<string, Instance> Instances;

        public static void PrintRegion(Region r)
        {
            Console.WriteLine(r.RegionName + " , " + r.FreeInstanceType);
        }
        public static void MapRegions(Dictionary<string, Region> regions, List<Region> records)
        {
            foreach (var record in records)
            {
                regions[record.RegionName] = record;
                regions[record.RegionName].Instances = new Dictionary<string, Instance>();
            }
        }
        public static List<Region> ParseRegion(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                return csv.GetRecords<Region>().ToList();
        }

    }
}
