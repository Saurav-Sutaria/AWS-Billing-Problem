using BillingEngine.Helper;
using BillingEngine.Models.Billing;
using System;

namespace BillingEngine.Printers
{
    public class BillPrinter
    {
        public string PrintBill(MonthlyBill monthlyBill, string pathToOutputDir)
        {
            
            //This method will just print the bill by generating CSV and returns file path of that csv.
            // Print header information like customer name, month, year and then print
            // monthlyBill.GetTotalAmount();
            // monthlyBill.GetTotalDiscount();
            // monthlyBill.GetAmountToBePaid();
            Console.WriteLine(
                monthlyBill.CustomerId + " -- " +
                monthlyBill.CustomerName + " -- " +
                monthlyBill.MonthYear.Month + "," + monthlyBill.MonthYear.Year
                );

            //Now print itemized bill
            monthlyBill.GetAggregatedMonthlyEc2Usages().ForEach(PrintBillItem);

            //Return path of generated CSV
            return null;
        }

        private void PrintBillItem(AggregatedMonthlyEc2Usage aggregatedMonthlyEc2Usage)
        {
            Console.WriteLine(
                aggregatedMonthlyEc2Usage.ResourceType + " -- " +
                aggregatedMonthlyEc2Usage.TotalResources + " -- " +
                HelperMethods.ConvertToHour(aggregatedMonthlyEc2Usage.TotalUsedTime) + " -- " +
                HelperMethods.ConvertToHour(aggregatedMonthlyEc2Usage.TotalBilledTime) + " -- " +
                aggregatedMonthlyEc2Usage.CostPerHour + " -- " +
                Math.Round(aggregatedMonthlyEc2Usage.TotalAmount,4));
        }   
    }
}