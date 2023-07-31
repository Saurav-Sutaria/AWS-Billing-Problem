using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Globalization;

namespace Enhancement0
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
                string id = record.Id;
                //instance not exists
                if (!customerMap[id].Instances.ContainsKey(record.InstanceType))
                {
                    //add new instance
                    Instance newInstance = new Instance();
                    newInstance.Type = record.InstanceType;
                    newInstance.Charge = instanceMap[record.InstanceType].Charge;
                    newInstance.ids = new HashSet<string>();
                    newInstance.sessions = new List<Session>();
                    newInstance.monthlySessions = new Dictionary<string, MonthSession>();
                    customerMap[id].Instances.Add(record.InstanceType, newInstance);
                }
                //check if instance id already present or not
                if (!customerMap[id].Instances[record.InstanceType].ids.Contains(record.InstanceID))
                {
                    Customer cTemp = customerMap[id];
                    Instance iTemp = cTemp.Instances[record.InstanceType];
                    iTemp.ids.Add(record.InstanceID);
                }

                //add session
                Session newSession = new Session();
                newSession.startTime = record.StartTime; newSession.endTime = record.EndTime;
                Customer c = customerMap[id];
                Instance i = c.Instances[record.InstanceType];
                i.sessions.Add(newSession);
            }
        }

    }
}
