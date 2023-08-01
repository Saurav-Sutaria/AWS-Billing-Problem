using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Globalization;

namespace Enhancement1
{
    internal class Customer
    {
        [Name("Customer ID")]
        public string Id { get; set; }

        [Name("Customer Name")]
        public string Name { get; set; }

        public Dictionary<string, Region> regions;

        public static List<Customer> ParseCustomer(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                return csv.GetRecords<Customer>().ToList();
        }
        public static void MapCustomers(Dictionary<string, Customer> customerMap, List<Customer> records)
        {
            foreach (var record in records)
            {
                record.Id = record.Id.Split('-')[0] + record.Id.Split('-')[1];
                customerMap[record.Id] = record;
                customerMap[record.Id].regions = new Dictionary<string, Region>();
            }
        }
        public void PrintCustomer(Customer c)
        {
            Console.WriteLine(c.Id + " , " + c.Name);
        }
        public void PrintFullCustomer(Customer c)
        {
            PrintCustomer(c);
            foreach(Region r in c.regions.Values)
            {
                Region.PrintRegion(r);
                foreach (Instance i in r.Instances.Values)
                {
                    Instance.PrintInstance(i);
                    foreach (var id in i.ids)
                    {
                        Console.Write(id + " , ");
                    }
                    Console.WriteLine();
                    foreach (KeyValuePair<string, MonthSession> data in i.monthlySessions)
                    {
                        Console.WriteLine(data.Key);
                        data.Value.PrintSession(data.Value);

                    }
                }
            }
            
        }

    }
}
