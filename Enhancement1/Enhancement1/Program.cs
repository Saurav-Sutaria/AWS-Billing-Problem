namespace Enhancement1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string onDemandUsagePath = "E:/@internship/Test-cases-enhancment-1/input/AWSOnDemandResourceUsage.csv";
            //string reservedUsagePath = "E:/@internship/Test-cases-enhancment-1/input/AWSReservedInstanceUsage.csv";
            string resourceTypesPath = "E:/@internship/Test-cases-enhancment-1/input/AWSResourceTypes.csv";
            string customerPath = "E:/@internship/Test-cases-enhancment-1/input/Customer.csv";
            //string regionPath = "E:/@internship/Test-cases-enhancment-1/input/Region.csv";


            Dictionary<string, Customer> customerMap = new Dictionary<string, Customer>(); //map -> customer id & customer
            //Dictionary<string, Region> regionMap = new Dictionary<string, Region>();
            Dictionary<string, Instance> instanceMap = new Dictionary<string, Instance>();
            //parsing customer info file
            var customerRecord = Customer.ParseCustomer(customerPath);
            Customer.MapCustomers(customerMap, customerRecord);

            //parsing resource type file
            var instances = Instance.ParseResources(resourceTypesPath);
            Instance.MapResources(instanceMap, instances);

            //parsing region file
            //var regionRecords = Region.ParseRegion(regionPath);
            //Region.MapRegions(regionMap, regionRecords);

            //parsing resource usage resource file
            var records = ResourceUsage.ParseResourceUsage(onDemandUsagePath);
            ResourceUsage.AddResourcesUsedData(records, customerMap, instanceMap);

            //divide each session based on month
            Session.GenerateMontlySessions(customerMap);

            //foreach(Customer c in customerMap.Values)
            //{
            //    c.PrintFullCustomer(c);
            //}

            //generate bill
            Dictionary<string, Bill> bills = new Dictionary<string, Bill>();
            Bill.GenerateBillData(customerMap, bills);

            //print the bill 
            Console.WriteLine(bills.Count);
            Bill.PrintBill(bills);

            //generate csv files
            CsvGenerator.GenerateCsv(bills);
        }
    }
}