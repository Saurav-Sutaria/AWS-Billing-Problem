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
        public static Ec2Instance FindEc2Instance(this List<Ec2Instance> ec2Instances, string instanceId,string OSType)
        {
            return ec2Instances.Where(instance => instance.InstanceId == instanceId && instance.InstanceType.OperatingSystem.ToString() == OSType).FirstOrDefault();
        }

        //extension method to find ec2 instance type from instance type name and region
        public static Ec2InstanceType FindEc2InstanceType(this List<Ec2InstanceType> ec2InstanceTypes, string instanceType,string regionName,string OS)
        {
            return ec2InstanceTypes.Where(instance => (instance.InstanceType == instanceType && instance.Region.Name == regionName && instance.OperatingSystem.ToString() == OS)).First();
        }

        //extension method to find the monthyear object
        public static MonthYear FindMonthYear(this List<MonthYear> monthYears,int month,int year)
        {
            return monthYears.Where(monthYear => (monthYear.Month == month) && (monthYear.Year == year)).FirstOrDefault();
        }

        //extension method to find the aggregated ec2 instance based on instance type and region
        public static AggregatedMonthlyEc2Usage FindAggregatedMonthlyEc2Usage(this List<AggregatedMonthlyEc2Usage> list,Ec2InstanceType ec2InstanceType)
        {
            return list.Where(record => record.ResourceType == ec2InstanceType.InstanceType && record.RegionName == ec2InstanceType.Region.Name).FirstOrDefault();
        }
        //extension method to find the aggregated ec2 instance based on id
        public static AggregatedMonthlyEc2Usage FindInstanceId(this List<AggregatedMonthlyEc2Usage> list,string instanceId)
        {
            return list.Where(record => record.ids.Contains(instanceId)).FirstOrDefault();
        }
        //extension method to find monthly ec2 instance usage
        public static MonthlyEc2InstanceUsage FindMonthlyEc2InstanceUsage(this List<MonthlyEc2InstanceUsage> list,string instanceId)
        {
            return list.Where(record => record.Ec2InstanceId == instanceId).FirstOrDefault();
        }
        //extension method to find ec2 region
        public static Ec2Region FindRegion(this List<Ec2Region> ec2Regions,string regionName)
        {
            return ec2Regions.Where(region => region.Name == regionName).First();
        }
    }
}