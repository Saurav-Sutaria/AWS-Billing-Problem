using BillingEngine.Models;
using BillingEngine.Models.Billing;
using BillingEngine.Models.Ec2;
using BillingEngine.Parsers.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BillingEngine.DomainModelGenerators
{
    public static class DomainModelGeneratorExtensions
    {
        // This is an extension method (used to extend the functionality of some existing class)
        public static List<ParsedEc2ResourceUsageEventRecord> FindRecordsForCustomer(
            this List<ParsedEc2ResourceUsageEventRecord> parsedEc2ResourceUsageEventRecords,
            string customerId)
        {
            return parsedEc2ResourceUsageEventRecords
                .Where(record => record.CustomerId == customerId)
                .ToList();
        }

        //extension method to find ec2 instance from instance id
        public static Ec2Instance FindEc2Instance(this List<Ec2Instance> ec2Instances, string instanceId)
        {
            return ec2Instances.Where(instance => instance.InstanceId == instanceId).FirstOrDefault();
        }

        //extension method to find ec2 instance type from instance type name and region
        public static Ec2InstanceType FindEc2InstanceType(this List<Ec2InstanceType> ec2InstanceTypes, string instanceType,string regionName)
        {
            return ec2InstanceTypes.Where(instance => (instance.InstanceType == instanceType && instance.Region.Name == regionName)).First();
        }

        //extension method to find the monthyear object
        public static MonthYear FindMonthYear(this List<MonthYear> monthYears,int month,int year)
        {
            return monthYears.Where(monthYear => (monthYear.Month == month) && (monthYear.Year == year)).FirstOrDefault();
        }

        //extension method to find the aggregated ec2 instance
        public static AggregatedMonthlyEc2Usage FindAggregatedMonthlyEc2Usage(this List<AggregatedMonthlyEc2Usage> list,Ec2InstanceType ec2InstanceType)
        {
            return list.Where(record => record.ResourceType == ec2InstanceType.InstanceType && record.RegionName == ec2InstanceType.Region.Name).FirstOrDefault();
        }
    }
}