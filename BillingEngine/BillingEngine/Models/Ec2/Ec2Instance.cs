using BillingEngine.Models.Billing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BillingEngine.Models.Ec2
{
    public class Ec2Instance
    {
        public string InstanceId { get; }

        public Ec2InstanceType InstanceType { get; }

        public List<ResourceUsageEvent> Usages { get; set; }

        public Ec2Instance(string instanceId, Ec2InstanceType instanceType)
        {
            InstanceId = instanceId;
            InstanceType = instanceType;
            Usages = new List<ResourceUsageEvent>();
        }

        public MonthlyEc2InstanceUsage GetMonthlyEc2InstanceUsageForMonth(MonthYear monthYear)
        {
            // Creates an instance of MonthlyEc2InstanceUsage by capturing usage events, applicable for a given month and year
            // For example, if Usages contain
            // 2021-05-10 to 2021-05-12 and
            // 2021-05-15 to 2021-05-17 and
            // 2021-05-25 to 2021-06-04 and
            // 2021-06-15 to 2021-06-17

            // Then newly constructed MonthlyEc2InstanceUsage for month 05/2021 (passed as argument) would include
            // 2021-05-10 to 2021-05-12 and
            // 2021-05-15 to 2021-05-17 and
            // 2021-05-25 to 2021-05-31
            MonthlyEc2InstanceUsage monthlyEc2InstanceUsage = new MonthlyEc2InstanceUsage(InstanceId,InstanceType);
            foreach(var usage in  Usages)
            {                
                int currStart = usage.UsedFrom.Year * 100 + usage.UsedFrom.Month;
                int currEnd = usage.UsedUntil.Year*100 + usage.UsedUntil.Month;
                int givenData = monthYear.Year*100 + monthYear.Month;
                
                //case 1: e.g 10-5-2021 to 20-5-2021 for 5-2021
                if(givenData == currStart && givenData == currEnd)
                {
                    ResourceUsageEvent newUsage = new ResourceUsageEvent(usage.UsedFrom, usage.UsedUntil);
                    monthlyEc2InstanceUsage.AddEc2UsageEvent(newUsage);
                }
                //case 2: e.g 10-5-2021 to 10-6-2021 for 5-2021 -> 10-5-21 to month end
                else if(givenData == currStart && givenData <  currEnd)
                {
                    int currMonth = usage.UsedFrom.Month + 1;
                    int currYear = usage.UsedFrom.Year;
                    if(currMonth == 13)
                    {
                        currMonth = 1;
                        currYear++;
                    }
                    DateTime newUsedFrom = usage.UsedFrom;
                    DateTime newUsedUntil = new DateTime(currYear, currMonth, 1, 0, 0, 0);
                    ResourceUsageEvent newUsage = new ResourceUsageEvent(newUsedFrom,newUsedUntil);
                    monthlyEc2InstanceUsage.AddEc2UsageEvent(newUsage);
                }
                //case 3: 10-4-21 to 10-5-21 for 5-21 -> month start to 10-5-21
                else if(givenData > currStart && givenData == currEnd)
                {
                    DateTime usedUntil = usage.UsedUntil;
                    ResourceUsageEvent newUsage = new ResourceUsageEvent(new DateTime(usedUntil.Year, usedUntil.Month, 1, 0, 0, 0),usedUntil);
                    monthlyEc2InstanceUsage.AddEc2UsageEvent(newUsage);
                }
                //case 4 : 10-4-21 to 10-6-21 for 5-21 -> month start to month end
                else if(givenData > currStart && givenData < currEnd)
                {
                    int currMonth = monthYear.Month, currYear = monthYear.Year;
                    
                    int endMonth = currMonth + 1, endYear = currYear ;
                    if (endMonth == 13)
                    {
                        endMonth = 1;
                        endYear++;
                    }
                    ResourceUsageEvent newUsage = new ResourceUsageEvent(
                        new DateTime(currYear, currMonth, 1, 0, 0, 0),
                        new DateTime(endYear, endMonth, 1, 0, 0, 0));
                    monthlyEc2InstanceUsage.AddEc2UsageEvent(newUsage);
                }
            }
            return monthlyEc2InstanceUsage;
        }

        public DateTime GetMinimumValueOfUsedFrom()
        {
            return Usages.Select(usage => usage.UsedFrom).Min();
        }
    }
}