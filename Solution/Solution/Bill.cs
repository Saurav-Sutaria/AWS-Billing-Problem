using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Solution
{
    internal class Bill
    {
        public string fileName;
        public string customerName;
        public string month;
        public string year;
        public double totalAmount;
        public Dictionary<string, Resource> resourceUsedInfo;
        public static void PrintBill(Dictionary<string, Bill> billList)
        {
            foreach (Bill bill in billList.Values)
            {

                Console.WriteLine(bill.fileName + " , " + bill.customerName + " , " + bill.month + " , " + bill.year + " , " + bill.totalAmount);
                foreach (Resource r in bill.resourceUsedInfo.Values)
                {
                    r.PrintResource(r);
                }
            }
        }

       
        public static void GenerateBillData(Dictionary<string, Customer> customerMap, Dictionary<string, Bill> billList)
        {
            foreach (KeyValuePair<string, Customer> kvp in customerMap)
            {
                Customer c = kvp.Value;
                foreach (KeyValuePair<string, Instance> keyValuePair in c.Instances)
                {
                    Instance i = keyValuePair.Value;
                    foreach (KeyValuePair<string, MonthSession> monthSession in i.monthlySessions)
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
                            newBill.month = Helper.GetFullMonthName(monthSession.Key.Substring(0, 3));
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
                            currResource.billedTime += (Helper.RoundToNearestNextHour(s.totalTime));
                        }
                        currResource.totalAmount += (currResource.billedTime.TotalHours * currResource.ratePerHour);
                        currBill.totalAmount += currResource.totalAmount;
                    }
                }
            }
        }
    }
}