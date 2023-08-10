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
                "E:/@internship/TestCases/Case3/Input/Customer.csv",
                "E:/@internship/TestCases/Case3/Input/AWSResourceTypes.csv",
                "E:/@internship/TestCases/Case3/Input/AWSCustomerUsage.csv",
                "E:/@internship/Test-cases-enhancment-1/input/Region.csv"
            );
            
            monthlyBills.ForEach(monthlyBill => billPrinter.PrintBill(monthlyBill, "E:/@internship/TestCases/Case3/Result/"));
        }
    }
}