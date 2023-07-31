using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Solution
{
    internal class Customer
    {
        [Name("Customer ID")]
        public string Id { get; set; }

        [Name("Customer Name")]
        public string Name { get; set; }

        public Dictionary<string, Instance> Instances;

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
                customerMap[record.Id].Instances = new Dictionary<string, Instance>();
            }
        }
        public void PrintCustomer(Customer c)
        {
            Console.Write(c.Id + " , ");
            Console.Write(c.Name + "\n");
        }
        public void PrintFullCustomer(Customer c)
        {
            PrintCustomer(c);
            foreach (KeyValuePair<string, Instance> kvp in c.Instances)
            {
                Instance i = kvp.Value;
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
