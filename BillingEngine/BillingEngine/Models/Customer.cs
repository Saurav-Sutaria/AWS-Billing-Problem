using BillingEngine.DomainModelGenerators;
using BillingEngine.Models.Billing;
using BillingEngine.Models.Ec2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BillingEngine.Models
{
    public class Customer
    {
        public string CustomerId { get; }
        public string CustomerName { get; }

        public List<Ec2Instance> Ec2Instances { get; }

        public Customer()
        {
            Ec2Instances = new List<Ec2Instance>();
        }

        public Customer(string customerId, string customerName, List<Ec2Instance> ec2Instances)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            Ec2Instances = ec2Instances;
        }

        public List<MonthYear> GetDistinctMonthYears()
        {
            //return all possible month year combination for a customer
            List<MonthYear> monthYears = new List<MonthYear>();
            foreach (var ec2Instance in Ec2Instances)
            {
                foreach (var usage in ec2Instance.Usages)
                {
                   
                    if (usage.UsedFrom == usage.UsedUntil) continue; //if start time and end time is same
                    int startMonth = usage.UsedFrom.Month;
                    int startYear = usage.UsedFrom.Year;
                    int endMonth = usage.UsedUntil.Month;
                    int endYear = usage.UsedUntil.Year;
                    for(int year = startYear;year<=endYear;year++)
                    {
                        int monthStart = (year == startYear)? startMonth : 1;
                        int monthEnd = (year == endYear)? endMonth : 12;
                        for(int month = monthStart; month <= monthEnd; month++)
                        {
                            var currMonthYear = new MonthYear(month, year);
                            var monthYear = monthYears.FindMonthYear(month, year);
                            if(monthYear == null)
                            {
                                monthYears.Add(currMonthYear);
                            }
                        }
                    }
                }
            }
            return monthYears;
        }

        public List<MonthlyEc2InstanceUsage> GetMonthlyEc2InstanceUsagesForMonth(MonthYear monthYear)
        {
            // Using List<Ec2Instance> , construct  List<MonthlyEc2InstanceUsage> by calling ec2Instance.GetUsageInMonth(monthYear)
            List<MonthlyEc2InstanceUsage> monthlyEc2InstanceUsages = new List<MonthlyEc2InstanceUsage>();
            foreach(var ec2Instance in Ec2Instances)
            {
                monthlyEc2InstanceUsages.Add(ec2Instance.GetMonthlyEc2InstanceUsageForMonth(monthYear));   
            }
           
            return monthlyEc2InstanceUsages;
        }

        public DateTime GetJoiningDate()
        {
            return Ec2Instances
                .Select(instance => instance.GetMinimumValueOfUsedFrom())
                .Min();
        }
    }
}