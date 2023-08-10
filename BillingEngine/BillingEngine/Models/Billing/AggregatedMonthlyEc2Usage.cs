using BillingEngine.Models.Ec2;
using System;
using System.Collections.Generic;

namespace BillingEngine.Models.Billing
{
    public class AggregatedMonthlyEc2Usage
    {   
        public string RegionName { get; set; }
        public string ResourceType { get; set; }
        public int TotalResources { get; set; }
        public double CostPerHour { get; set; }
        public TimeSpan TotalBilledTime { get; set; }
        public TimeSpan TotalUsedTime { get; set; }
        public TimeSpan TotalDiscountedTime { get; set; }

        public double TotalAmount { get; set; }
        public double TotalDiscount { get; set; }
        public HashSet<string> ids { get; set; }
        public double GetActualAmountToBePaid()
        {
            return TotalAmount - TotalDiscount;
        }

    }
}