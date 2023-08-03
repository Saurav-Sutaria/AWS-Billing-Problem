using System;

namespace BillingEngine.Models.Billing
{
    public class AggregatedMonthlyEc2Usage
    {
        public string ResourceType { get; set; }
        public int TotalResources { get; set; }
        public double CostPerHour { get; set; }
        public TimeSpan TotalBilledTime { get; set; }
        public TimeSpan TotalUsedTime { get; set; }
        public TimeSpan TotalDiscountedTime { get; set; }

        public double TotalAmount { get; set; }
        public double TotalDiscount { get; set; }

        public AggregatedMonthlyEc2Usage()
        {
            TotalAmount = 0;
            TotalDiscount = 0;
            TotalResources = 0;
            TotalBilledTime = TimeSpan.Zero;
            TotalUsedTime = TimeSpan.Zero;
            TotalDiscountedTime = TimeSpan.Zero;
        }

        public double GetActualAmountToBePaid()
        {
            return TotalAmount - TotalDiscount;
        }

    }
}