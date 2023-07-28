using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Solution
{
    internal class Program
    {
        static void Main(string[] args){
            string usagePath = "E:/@internship/TestCases/Case1/Input/AWSCustomerUsage.csv";
            string typePath = "E:/@internship/TestCases/Case1/Input/AWSResourceTypes.csv";
            string customerPath = "E:/@internship/TestCases/Case1/Input/Customer.csv";

            string[] customerArr = File.ReadAllLines(customerPath);
            string[] resourceTypeArr = File.ReadAllLines(typePath);
            string[] usageArr = File.ReadAllLines(usagePath);

            //foreach(string line in usageArr) Console.WriteLine(line);
            //Console.WriteLine();
            //foreach(string line in resourceTypeArr) Console.WriteLine(line);
            //Console.WriteLine();
            //foreach (string line in customerArr) Console.WriteLine(line);

            Dictionary<string, Customer> customerMap = new Dictionary<string, Customer>();
            Dictionary<string, Instance> instanceMap = new Dictionary<string, Instance>();

            //parsing customer file
            for (int i = 1; i < customerArr.Length; i++){
                
                string[] infoSplit = customerArr[i].Split(',');
                infoSplit[1] = infoSplit[1].Split('-')[0] + infoSplit[1].Split('-')[1];
                Customer newCustomer = new Customer();
                newCustomer.id = infoSplit[1];
                newCustomer.name = infoSplit[2];
            }

            //parsing aws resource type file
            for(int i=1;i<resourceTypeArr.Length;i++){
                string[] infoSplit = resourceTypeArr[i].Split(',');
                Instance newInstance = new Instance();
                newInstance.type = infoSplit[1];
                //double num = double.Parse(infoSplit[2].Split('\"')[1].Split('$')[1]);
                double num = double.Parse(infoSplit[2].Trim('\"').Trim('$'));
                
                Console.WriteLine(num + 100);
                //newInstance.charge = Int32.Parse()
            }

        }
    }
}
