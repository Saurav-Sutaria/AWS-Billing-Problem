namespace Enhancement1
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

                Console.WriteLine(bill.fileName + " , " + bill.customerName + " , " + bill.month + " , " + bill.year + " , " + (bill.totalAmount));
                foreach (Resource r in bill.resourceUsedInfo.Values)
                {
                    r.PrintResource(r);
                }
            }
        }

        public static void GenerateBillData(Dictionary<string, Customer> customerMap, Dictionary<string, Bill> billList)
        {
            foreach (Customer c in customerMap.Values)
            {
                foreach (Region r in c.regions.Values)
                {
                    foreach (Instance i in r.Instances.Values)
                    {
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
                                newBill.month = Helper.GetFullMonthName(monthSession.Key[..3]);
                                newBill.year = monthSession.Key[4..];
                                newBill.totalAmount = 0;
                                newBill.resourceUsedInfo = new Dictionary<string, Resource>();
                                billList.Add(fileName, newBill);
                            }
                            //add info in the bill
                            Bill currBill = billList[fileName];
                            //check if the given instance already exists or not
                            if (!currBill.resourceUsedInfo.ContainsKey(r.RegionName))
                            {
                                //instance not presenet
                                Resource newResource = new Resource();
                                newResource.resourceType = i.Type;
                                newResource.totalResources = i.ids.Count;
                                newResource.region = r.RegionName;
                                newResource.ratePerHour = Double.Parse(i.OnDemandCharge);
                                newResource.totalAmount = 0;
                                currBill.resourceUsedInfo.Add(r.RegionName, newResource);
                            }
                            Resource currResource = currBill.resourceUsedInfo[r.RegionName];
                            foreach (Session s in monthSession.Value.sessions)
                            {
                                currResource.totalTime += (s.totalTime);
                                currResource.billedTime += (Helper.RoundToNearestNextHour(s.totalTime));
                            }
                            currResource.totalAmount += Math.Round(currResource.billedTime.TotalHours * currResource.ratePerHour, 4);
                            currBill.totalAmount += Math.Round(currResource.totalAmount, 4);
                        }
                    }
                }

            }
        }
    }
}