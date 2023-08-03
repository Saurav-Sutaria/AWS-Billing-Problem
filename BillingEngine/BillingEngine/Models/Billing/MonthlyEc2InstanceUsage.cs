using BillingEngine.Models.Ec2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BillingEngine.Models.Billing
{
    public class MonthlyEc2InstanceUsage
    {
        public string Ec2InstanceId { get; }

        public Ec2InstanceType Ec2InstanceType { get; }

        public List<ResourceUsageEvent> Usages { get; }

        public int DiscountedHours { get; private set; }

        public MonthlyEc2InstanceUsage(string ec2InstanceId, Ec2InstanceType ec2InstanceType)
        {
            Ec2InstanceId = ec2InstanceId;
            Ec2InstanceType = ec2InstanceType;
            Usages = new List<ResourceUsageEvent>();
        }

        public void AddEc2UsageEvent(ResourceUsageEvent usageEvent)
        {
            Usages.Add(usageEvent);
        }

        public void ApplyDiscount(int discountedHours)
        {
            DiscountedHours = discountedHours;
        }

        public int GetTotalBillableHours()
        {
            return Usages.Select(usage => usage.GetBillableHours()).Sum();
        }
        public TimeSpan GetTotalUsageTime()
        {
            TimeSpan totalTime = new TimeSpan();
            foreach(var usage in Usages)
            {
                totalTime += usage.GetTotalTime();
            }
            return totalTime; 
        }
    }
}