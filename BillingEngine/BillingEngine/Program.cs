using BillingEngine.Billing;
using BillingEngine.Models.Billing;
using BillingEngine.Printers;
using System;

namespace BillingEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            BillingService billingService = new BillingService();
            BillPrinter billPrinter = new BillPrinter();

            var monthlyBills = billingService.GenerateMonthlyBills(
                "E:/@internship/Test-cases-enhancment-1/input/Customer.csv",
                "E:/@internship/Test-cases-enhancment-1/input/AWSResourceTypes.csv",
                "E:/@internship/Test-cases-enhancment-1/input/AWSOnDemandResourceUsage.csv",
                "E:/@internship/Test-cases-enhancment-1/input/Region.csv"
            );
            //Console.WriteLine(monthlyBills.Count);
            monthlyBills.ForEach(monthlyBill => billPrinter.PrintBill(monthlyBill, "E:/@internship/Test-cases-enhancment-1/result_FreeTier/"));
        }
    }
}