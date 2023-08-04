using BillingEngine.DomainModelGenerators;
using BillingEngine.Models.Ec2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BillingEngine.Models.Billing
{
    public class MonthlyBill
    {
        public string CustomerId { get; }
        public string CustomerName { get; }

        public MonthYear MonthYear { get; }

        public List<MonthlyEc2InstanceUsage> MonthlyEc2InstanceUsages { get; }

        public MonthlyBill(string customerId, string customerName, MonthYear monthYear)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            MonthYear = monthYear;
            MonthlyEc2InstanceUsages = new List<MonthlyEc2InstanceUsage>();
        }

        public void AddMonthlyEc2Usages(List<MonthlyEc2InstanceUsage> monthlyEc2InstanceUsages)
        {
            MonthlyEc2InstanceUsages.AddRange(monthlyEc2InstanceUsages);
        }

        public List<AggregatedMonthlyEc2Usage> GetAggregatedMonthlyEc2Usages()
        {
            //Using MonthlyEc2InstanceUsages, compute List<AggregatedMonthlyEc2Usage>

            List<AggregatedMonthlyEc2Usage> aggregatedMonthlyEc2Usages = new List<AggregatedMonthlyEc2Usage>();
            foreach(var monthlyEc2InstanceUsage in MonthlyEc2InstanceUsages)
            {
                var getAggreg = aggregatedMonthlyEc2Usages.FindAggregatedMonthlyEc2Usage(monthlyEc2InstanceUsage.Ec2InstanceType.InstanceType);
                if(getAggreg == null)
                {
                    AggregatedMonthlyEc2Usage newAggregateUsage = new AggregatedMonthlyEc2Usage();
                    newAggregateUsage.ResourceType = monthlyEc2InstanceUsage.Ec2InstanceType.InstanceType;
                    newAggregateUsage.TotalResources = 1;
                    newAggregateUsage.CostPerHour = monthlyEc2InstanceUsage.Ec2InstanceType.CostPerHour;
                    newAggregateUsage.TotalUsedTime = monthlyEc2InstanceUsage.GetTotalUsageTime();
                    if (newAggregateUsage.TotalUsedTime.TotalSeconds == 0) continue;
                    newAggregateUsage.TotalBilledTime = new TimeSpan(monthlyEc2InstanceUsage.GetTotalBillableHours(),0,0);
                    newAggregateUsage.TotalAmount = newAggregateUsage.CostPerHour * newAggregateUsage.TotalBilledTime.TotalHours;
                    aggregatedMonthlyEc2Usages.Add(newAggregateUsage);
                }
                else
                {
                    getAggreg.TotalResources += 1;
                    getAggreg.TotalBilledTime += new TimeSpan(monthlyEc2InstanceUsage.GetTotalBillableHours(), 0, 0);
                    getAggreg.TotalUsedTime += monthlyEc2InstanceUsage.GetTotalUsageTime();
                    getAggreg.TotalAmount = getAggreg.TotalBilledTime.TotalHours * getAggreg.CostPerHour;
                }
            }
            return aggregatedMonthlyEc2Usages;
        }

        public void ApplyDiscount(string ec2InstanceId, int discountedHours)
        {
            //Find matching object of type MonthlyEc2InstanceUsage from MonthlyEc2InstanceUsages
            // and then call monthlyEc2InstanceUsage.ApplyDiscount(discountedHours)
        }

        public double GetTotalAmount(List<AggregatedMonthlyEc2Usage> usages)
        {
            double totalAmount = 0;
            foreach(var usage in usages)
            {
                totalAmount += usage.TotalAmount;
            }
            return Math.Round(totalAmount,4);
        }

        public double GetTotalDiscount()
        {
            //throw new System.NotImplementedException();
            return 0;
        }

        //public double GetAmountToBePaid()
        //{
        //    return GetTotalAmount() - GetTotalDiscount();
        //}

        //public List<MonthlyEc2InstanceUsage> GetFreeTierEligibleInstanceUsagesOfType(OperatingSystem operatingSystem)
        //{
        //    return MonthlyEc2InstanceUsages
        //        .Where(instanceUsage => instanceUsage.Ec2InstanceType.IsFreeTierEligible)
        //        .Where(instanceUsage => instanceUsage.Ec2InstanceType.OperatingSystem == operatingSystem)
        //        .ToList();
        //}
    }
}