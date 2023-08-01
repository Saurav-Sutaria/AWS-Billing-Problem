using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Globalization;

namespace Enhancement1
{
    internal class ResourceUsage
    {
        [Name("Customer ID")]
        public string Id { get; set; }

        [Name("EC2 Instance ID")]
        public string InstanceID { get; set; }

        [Name("EC2 Instance Type")]
        public string InstanceType { get; set; }

        [Name("Used From")]
        public string StartTime { get; set; }

        [Name("Used Until")]
        public string EndTime { get; set; }

        [Name("Region")]
        public string RegionName { get; set; }

        public static List<ResourceUsage> ParseResourceUsage(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                return csv.GetRecords<ResourceUsage>().ToList();
        }

        public static void AddResourcesUsedData(List<ResourceUsage> records, Dictionary<string, Customer> customerMap, Dictionary<string, Instance> instanceMap)
        {
            foreach (var record in records)
            {
                //check if the region is present or not
                string id = record.Id;
                Customer c = customerMap[id];
                if (!c.regions.ContainsKey(record.RegionName))
                {
                    //create new region and add it to the customer object
                    Region newRegion = new Region();
                    newRegion.RegionName = record.RegionName;
                    //newRegion.FreeInstanceType = regionMap[record.RegionName].FreeInstanceType;
                    newRegion.Instances = new Dictionary<string, Instance>();
                    c.regions.Add(record.RegionName, newRegion);
                }
                Region r = c.regions[record.RegionName];
                //instance not exists
                if (!r.Instances.ContainsKey(record.InstanceType))
                {
                    //add new instance
                    Instance newInstance = new Instance();
                    newInstance.Type = record.InstanceType;
                    //newInstance.OnDemandCharge = instanceMap[record.InstanceType].OnDemandCharge;
                    newInstance.ids = new HashSet<string>();
                    newInstance.OnDemandCharge = instanceMap[record.InstanceType + record.RegionName].OnDemandCharge;
                    newInstance.sessions = new List<Session>();
                    newInstance.monthlySessions = new Dictionary<string, MonthSession>();
                    r.Instances.Add(record.InstanceType, newInstance);
                }
                Instance i = r.Instances[record.InstanceType];
                //check if instance id already present or not
                if (!i.ids.Contains(record.InstanceID))
                {
                    i.ids.Add(record.InstanceID);
                }

                //add session
                Session newSession = new Session();
                newSession.startTime = record.StartTime; newSession.endTime = record.EndTime;
                i.sessions.Add(newSession);
            }
        }

    }
}
