using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;

namespace Solution
{
    internal class Program
    {
        public static void printInstance(Instance i)
        {
            Console.Write(i.Type +" , ");
            Console.Write(i.Charge + "\n");
        }
        public static void printCustomer(Customer c)
        {
            Console.Write(c.Id + " , ");
            Console.Write(c.Name + "\n");
            //foreach (KeyValuePair<string, Instance> kvp in c.instances)
            //{
            //    printInstance(kvp.Value);
            //}
        }
        public static void printRecord(ResourceUsage r)
        {
            Console.Write(r.Id + " , ");
            Console.Write(r.InstanceID + " , ");
            Console.Write(r.InstanceType + " , ");
            Console.Write(r.StartTime + " , ");
            Console.Write(r.EndTime + "\n");
        }


        static void Main(string[] args){
            string usagePath = "E:/@internship/TestCases/Case1/Input/AWSCustomerUsage.csv";
            string typePath = "E:/@internship/TestCases/Case1/Input/AWSResourceTypes.csv";
            string customerPath = "E:/@internship/TestCases/Case1/Input/Customer.csv";

            Dictionary<string, Customer> customerMap = new Dictionary<string, Customer>(); //map customer id & customer
            Dictionary<string, Instance> instanceMap = new Dictionary<string, Instance>(); //map instance type & instanace

            //parsing customer info file
            using (var reader = new StreamReader(customerPath)) 
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
            {
              
                var records = csv.GetRecords<Customer>();
                foreach(var record in records)
                {
                    customerMap[record.Id] = record;                   
                }
            }

            foreach (KeyValuePair<string, Customer> kvp in customerMap)
            {
                printCustomer(kvp.Value);
            }

            //parsing resource type file
            using (var reader = new StreamReader(typePath))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
            {

                var records = csv.GetRecords<Instance>();
                foreach (var record in records)
                {
                    record.Charge = record.Charge.Substring(1);
                    instanceMap[record.Type] = record;
                }
            }

            foreach (KeyValuePair<string, Instance> kvp in instanceMap)
            {
                printInstance(kvp.Value);
            }

            //parsing resource usage resource file
            using (var reader = new StreamReader(usagePath))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
            {

                var records = csv.GetRecords<ResourceUsage>();
                foreach (var record in records)
                {
                    printRecord(record);
                }
            }

        }
    }
}
