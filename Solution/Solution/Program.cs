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
        }
        public static void printRecord(ResourceUsage r)
        {
            Console.Write(r.Id + " , ");
            Console.Write(r.InstanceID + " , ");
            Console.Write(r.InstanceType + " , ");
            Console.Write(r.StartTime + " , ");
            Console.Write(r.EndTime + "\n");
        }

        public static void printSession(Session s)
        {
            Console.Write(s.startTime + " - " + s.endTime + " , ");
        }

        public static void printFullCustomer(Customer c)
        {
            printCustomer(c);
            foreach(KeyValuePair<string,Instance> kvp in c.Instances)
            {
                Instance i = kvp.Value;
                printInstance(i);
                foreach(var id in i.ids)
                {
                    Console.Write(id + " , ");
                }
                Console.WriteLine();
                foreach(var session in i.sessions)
                {
                    printSession(session);
                }
                Console.WriteLine();
            }

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
                    record.Id = record.Id.Split('-')[0] + record.Id.Split('-')[1]; 
                    customerMap[record.Id] = record;
                    customerMap[record.Id].Instances = new Dictionary<string, Instance>();
                }
            }

            //foreach (KeyValuePair<string, Customer> kvp in customerMap)
            //{
            //    printCustomer(kvp.Value);
            //}

            //parsing resource type file
            using (var reader = new StreamReader(typePath))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
            {

                var records = csv.GetRecords<Instance>();
                foreach (var record in records)
                {
                    record.Charge = record.Charge.Substring(1);
                    instanceMap[record.Type] = record;
                    instanceMap[record.Type].ids = new HashSet<string>();
                    instanceMap[record.Type].sessions = new List<Session>();
                }
            }

            //foreach (KeyValuePair<string, Instance> kvp in instanceMap)
            //{
            //    printInstance(kvp.Value);
            //}

            //parsing resource usage resource file
            using (var reader = new StreamReader(usagePath))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
            {

                var records = csv.GetRecords<ResourceUsage>();
                foreach (var record in records)
                {
                    string id = record.Id;
                    //printRecord(record);
                    //instance not exists
                    if (!customerMap[id].Instances.ContainsKey(record.InstanceType))
                    {
                        //add new instance
                        Instance newInstance = new Instance();
                        newInstance.Type = record.InstanceType;
                        newInstance.Charge = instanceMap[record.InstanceType].Charge;
                        newInstance.ids = new HashSet<string>();
                        newInstance.sessions = new List<Session>();
                        customerMap[id].Instances.Add(record.InstanceType, newInstance);
                    }
                    //check if instance id already present or not
                    if (!customerMap[id].Instances[record.InstanceType].ids.Contains(record.InstanceID)){
                        Customer cTemp = customerMap[id];
                        Instance iTemp = cTemp.Instances[record.InstanceType];
                        iTemp.ids.Add(record.InstanceID);
                        //customerMap[id].Instances[record.InstanceType].ids.Add(record.InstanceID);
                    }

                    //add session
                    Session newSession = new Session();
                    newSession.startTime = record.StartTime; newSession.endTime = record.EndTime;
                    Customer c = customerMap[id];
                    //printCustomer(c);
                    Instance i = c.Instances[record.InstanceType];
                    i.sessions.Add(newSession);
                    
                    //printInstance(i);
                    //customerMap[id].Instances[record.InstanceType].sessions.Add(newSession);
                }
            }
            foreach (KeyValuePair<string, Customer> kvp in customerMap)
            {
                printFullCustomer(kvp.Value);
            }

        }
    }
}
