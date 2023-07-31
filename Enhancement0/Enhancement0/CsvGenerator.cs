using System.Text;

namespace Enhancement0
{
    internal class CsvGenerator
    {
        public static void GenerateCsv(Dictionary<string,Bill> bills)
        {
            foreach(var bill in bills.Values)
            {
                var csv = new StringBuilder();
                string csvPath = @"E:/@internship/TestCases/Case1/Result/" + bill.fileName + ".csv";
                csv.AppendLine(bill.customerName);
                csv.AppendLine(string.Format("Bill for month of {0} {1}", bill.month, bill.year));
                csv.AppendLine(string.Format("Total Amount: {0}", bill.totalAmount));
                csv.AppendLine("Resource Type,Total Resources,Total Used Time (HH:mm:ss),Total Billed Time (HH:mm:ss),Rate (per hour),Total Amount");
                foreach (Resource r in bill.resourceUsedInfo.Values)
                {
                    csv.AppendLine(string.Format("{0},{1},{2},{3},${4},${5}", r.resourceType, r.totalResources, Helper.ConvertToHour(r.totalTime), Helper.ConvertToHour(r.billedTime), r.ratePerHour, r.totalAmount));
                }
                File.WriteAllText(csvPath, csv.ToString());
            }
        }
    }
}
