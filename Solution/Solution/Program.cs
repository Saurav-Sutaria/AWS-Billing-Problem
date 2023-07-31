using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using Microsoft.SqlServer.Server;
using System.Security.Policy;

namespace Solution
{
    internal class Program
    {
        
        static TimeSpan RoundToNearestNextHour(TimeSpan time)
        {
            // Get the total second in the TimeSpan
            double totalSeconds = time.TotalSeconds;
            int newHour = (int)(Math.Ceiling(totalSeconds / 3600.0));
            // Convert back to TimeSpan
            TimeSpan roundedTime = new TimeSpan(0, newHour, 0, 0);
            return roundedTime;
        }

        static string GetFullMonthName(string monthInitial)
        {
            // Example with the current culture
            CultureInfo currentCulture = CultureInfo.CurrentCulture;

            // Get the array of full month names in the current culture
            string[] fullMonthNames = currentCulture.DateTimeFormat.MonthNames;

            // Find the index of the month initial (ignoring case)
            int index = Array.FindIndex(fullMonthNames, name => name.StartsWith(monthInitial, StringComparison.OrdinalIgnoreCase));

            // Return the full month name if found, otherwise return an empty string
            return index >= 0 ? fullMonthNames[index] : string.Empty;
        }
        public static void generateSession(Session s,Instance i)
        {
            //parse time string
            DateTime startDate = DateTime.Parse(s.startTime);
            DateTime endDate = DateTime.Parse(s.endTime);
            //Console.WriteLine(startDate + " " + endDate);
            while (startDate < endDate)
            {
                string monthId = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(startDate.Month).Substring(0, 3).ToUpper() + "-" + startDate.Year;

                //check if the month id already exists or not
                if (!i.monthlySessions.ContainsKey(monthId))
                {
                    //month not exists -> add new month id and its month session
                    MonthSession newMonthSession = new MonthSession();
                    newMonthSession.sessions = new List<Session>();
                    i.monthlySessions.Add(monthId, newMonthSession);

                }
                DateTime endOfMonth = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month), 23, 59, 59);
                //Console.WriteLine(monthId);
                if (endOfMonth <= endDate)
                {
                    Session newSession = new Session();
             
                    newSession.startTime = $"{startDate}";
                    newSession.endTime = $"{endOfMonth}";
                    //start time is 23:59:59 of the last day of last month
                    if (startDate == endOfMonth) newSession.totalTime = new TimeSpan(0, 0, 1);
                    else newSession.totalTime = endOfMonth - startDate;
                    if(newSession.totalTime.Minutes == 59 && newSession.totalTime.Seconds == 59)
                    {
                        newSession.totalTime = new TimeSpan(newSession.totalTime.Days,newSession.totalTime.Hours+1, 0, 0);
                    }
                    startDate = endOfMonth.AddSeconds(1);
                    //Console.WriteLine(newSession.startTime + " -- " + newSession.endTime + " -- " + newSession.totalTime);
                    i.monthlySessions[monthId].sessions.Add(newSession);
                }
                else
                {
                    Session newSession = new Session();
                    newSession.startTime = $"{startDate}";
                    newSession.endTime = $"{endDate}";
                    newSession.totalTime = endDate - startDate;
                   // Console.WriteLine(newSession.startTime + " -- " + newSession.endTime + " -- " + newSession.totalTime);
                    i.monthlySessions[monthId].sessions.Add(newSession);
                    break;
                }
            }
        }
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
        public static void printSession(MonthSession m)
        {
            foreach(Session s in m.sessions)
            {
                Console.WriteLine(s.startTime + " -- " + s.endTime + " -- " + s.totalTime);
            }
        }
        public static void printFullCustomer(Customer c)
        {
            printCustomer(c);
            foreach (KeyValuePair<string, Instance> kvp in c.Instances)
            {
                Instance i = kvp.Value;
                printInstance(i);
                foreach (var id in i.ids)
                {
                    Console.Write(id + " , ");
                }
                Console.WriteLine();
                //foreach(var session in i.sessions)
                //{
                //    printSession(session);
                //}
                //Console.WriteLine();
                foreach (KeyValuePair<string, MonthSession> data in i.monthlySessions)
                {
                    Console.WriteLine(data.Key);
                    printSession(data.Value);

                }
            }

        }
        public static string convertToHour(TimeSpan t)
        {
            int hours = (int)t.TotalHours;
            return hours.ToString() + "." + t.Minutes.ToString() + "." + t.Seconds.ToString();
        }
        public static void printResource(Resource r)
        {
            Console.WriteLine(r.resourceType + " , " + r.totalResources + " , " + convertToHour(r.totalTime) + " , " + convertToHour(r.billedTime) + " , " + r.ratePerHour + " , " + r.totalAmount);
        }
        public static void printBill(Bill bill)
        {
            Console.WriteLine(bill.fileName + " , " + bill.customerName + " , " + bill.month + " , " + bill.year + " , " + bill.totalAmount);
            foreach(Resource r in bill.resourceUsedInfo.Values)
            {
                printResource(r);
            }
        }
        public static void generateCSV(Bill bill)
        {
            var csv = new StringBuilder();
            string csvPath = @"E:/@internship/TestCases/Case4/Result/" + bill.fileName + ".csv";
            csv.AppendLine(bill.customerName);
            csv.AppendLine(string.Format("Bill for month of {0} {1}",bill.month,bill.year));
            csv.AppendLine(string.Format("Total Amount: {0}", bill.totalAmount));
            csv.AppendLine("Resource Type,Total Resources,Total Used Time (HH:mm:ss),Total Billed Time (HH:mm:ss),Rate (per hour),Total Amount");
            foreach(Resource r in bill.resourceUsedInfo.Values)
            {
                csv.AppendLine(string.Format("{0},{1},{2},{3},${4},${5}",r.resourceType,r.totalResources, convertToHour(r.totalTime), convertToHour(r.billedTime),r.ratePerHour,r.totalAmount));
            }
            File.WriteAllText(csvPath, csv.ToString());
        }
        static void Main(string[] args){
            string usagePath = "E:/@internship/TestCases/Case4/Input/AWSCustomerUsage.csv";
            string typePath = "E:/@internship/TestCases/Case4/Input/AWSResourceTypes.csv";
            string customerPath = "E:/@internship/TestCases/Case4/Input/Customer.csv";

            Dictionary<string, Customer> customerMap = new Dictionary<string, Customer>(); //map -> customer id & customer
            Dictionary<string, Instance> instanceMap = new Dictionary<string, Instance>(); //map -> instance type & instanace

            //parsing customer info file
            using (var reader = new StreamReader(customerPath))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
            {

                var records = csv.GetRecords<Customer>();
                foreach (var record in records)
                {
                    record.Id = record.Id.Split('-')[0] + record.Id.Split('-')[1];
                    customerMap[record.Id] = record;
                    customerMap[record.Id].Instances = new Dictionary<string, Instance>();
                }
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
                    instanceMap[record.Type].ids = new HashSet<string>();
                    instanceMap[record.Type].sessions = new List<Session>();
                    instanceMap[record.Type].monthlySessions = new Dictionary<string, MonthSession>();
                }
            }

            //parsing resource usage resource file
            using (var reader = new StreamReader(usagePath))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
            {

                var records = csv.GetRecords<ResourceUsage>();
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
                    if (!customerMap[id].Instances[record.InstanceType].ids.Contains(record.InstanceID)){
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
            //divide each session based on month
            foreach(KeyValuePair<string,Customer> kvp in customerMap)
            {
                Customer c = kvp.Value;
                foreach(Instance i in c.Instances.Values)
                {
                    foreach(Session s in i.sessions)
                    {
                        generateSession(s, i);
                    }
                }
            }
            //generate bill
            Dictionary<string, Bill> billList = new Dictionary<string, Bill>();
            foreach(KeyValuePair<string,Customer> kvp in customerMap)
            {
                Customer c = kvp.Value;
                foreach(KeyValuePair<string,Instance> keyValuePair in c.Instances)
                {
                    Instance i = keyValuePair.Value;
                    foreach(KeyValuePair<string,MonthSession> monthSession in i.monthlySessions)
                    {
                        
                        string fileName = c.Id + "_" + monthSession.Key;
                        //Console.WriteLine(monthSession.Key);
                        //bill for a particular month not exists
                        if (!billList.ContainsKey(fileName))
                        {
                            //generate new bill object and add it to list
                            Bill newBill = new Bill();
                            newBill.fileName = fileName;
                            newBill.customerName = c.Name;
                            newBill.month = GetFullMonthName(monthSession.Key.Substring(0, 3));
                            newBill.year = monthSession.Key.Substring(4);
                            newBill.totalAmount = 0;
                            newBill.resourceUsedInfo = new Dictionary<string, Resource>();
                            billList.Add(fileName, newBill);
                        }
                        //add info in the bill
                        //check if the given instance already exists or not
                        Bill currBill = billList[fileName];
                        if (!currBill.resourceUsedInfo.ContainsKey(i.Type))
                        {
                            //instance not presenet
                            Resource newResource = new Resource();
                            newResource.resourceType = i.Type;
                            newResource.totalResources = i.ids.Count;
                            newResource.ratePerHour = Double.Parse(i.Charge);
                            newResource.totalAmount = 0;
                            currBill.resourceUsedInfo.Add(i.Type, newResource);
                        }
                        Resource currResource = currBill.resourceUsedInfo[i.Type];
                        foreach (Session s in monthSession.Value.sessions)
                        {
                            currResource.totalTime += (s.totalTime);
                            currResource.billedTime += (RoundToNearestNextHour(s.totalTime));
                        }
                        currResource.totalAmount += (currResource.billedTime.TotalHours * currResource.ratePerHour);
                        currBill.totalAmount += currResource.totalAmount;
                    }
                }
            }
            //print the bill
            foreach(Bill bill in billList.Values)
            {
                generateCSV(bill);
                printBill(bill);
            }
        }
    }
}
