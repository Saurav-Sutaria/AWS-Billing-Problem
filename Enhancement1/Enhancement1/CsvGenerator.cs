using System.Text;

namespace Enhancement1
{
    internal class CsvGenerator
    {
        public static void GenerateCsv(Dictionary<string, Bill> bills)
        {
            foreach (var bill in bills.Values)
            {
                var csv = new StringBuilder();
                string csvPath = @"E:/@internship/Test-cases-enhancment-1/result/" + bill.fileName + ".csv";
                csv.AppendLine(bill.customerName);
                csv.AppendLine(string.Format("Bill for month of {0} {1}", bill.month, bill.year));
                csv.AppendLine(string.Format("Total Amount: ${0}", bill.totalAmount));
                csv.AppendLine("Discount: $0.00");
                csv.AppendLine(string.Format("Actual Amount: ${0}",bill.totalAmount));
                csv.AppendLine("Region,Resource Type,Total Resources,Total Used Time (HH:mm:ss),Total Billed Time (HH:mm:ss),Total Amount,Discount,Actual Amount");
                foreach (Resource r in bill.resourceUsedInfo.Values)
                {
                    csv.AppendLine(string.Format("{0},{1},{2},{3},{4},${5},${6},${7}", r.region,r.resourceType, r.totalResources, Helper.ConvertToHour(r.totalTime), Helper.ConvertToHour(r.billedTime), r.totalAmount ,0,r.totalAmount));
                }
                File.WriteAllText(csvPath, csv.ToString());
            }
        }
    }
}
