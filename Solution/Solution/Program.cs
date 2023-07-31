using System.Collections.Generic;

namespace Solution
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string usagePath = "E:/@internship/TestCases/Case1/Input/AWSCustomerUsage.csv";
            string typePath = "E:/@internship/TestCases/Case1/Input/AWSResourceTypes.csv";
            string customerPath = "E:/@internship/TestCases/Case1/Input/Customer.csv";

            Dictionary<string, Customer> customerMap = new Dictionary<string, Customer>(); //map -> customer id & customer
            Dictionary<string, Instance> instanceMap = new Dictionary<string, Instance>(); //map -> instance type & instanace

            //parsing customer info file
            var customerRecord = Customer.ParseCustomer(customerPath);
            Customer.MapCustomers(customerMap, customerRecord);

            //parsing resource type file
            var resourceRecord = Instance.ParseResources(typePath);
            Instance.MapResources(instanceMap, resourceRecord);

            //parsing resource usage resource file
            var records = ResourceUsage.ParseResourceUsage(usagePath);
            ResourceUsage.AddResourcesUsedData(records, customerMap, instanceMap);

            //divide each session based on month
            Session.GenerateMontlySessions(customerMap);

            //generate bill
            Dictionary<string, Bill> bills = new Dictionary<string, Bill>();
            Bill.GenerateBillData(customerMap, bills);

            //print the bill 
            Bill.PrintBill(bills);

            //generate csv files
            CsvGenerator.GenerateCsv(bills);
        }
    }
}