using System.Collections.Generic;
using System.Linq;
using BillingEngine.Models.Ec2;
using BillingEngine.Parsers.Models;

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
        public static Ec2Instance FindEc2Instance(this List<Ec2Instance> ec2Instances,string instanceId)
        {
            return ec2Instances.Where(instance => instance.InstanceId == instanceId).FirstOrDefault();
        }

        //extension method to find ec2 instance type from instance type name
        public static Ec2InstanceType FindEc2InstanceType(this List<Ec2InstanceType> ec2InstanceTypes,string instanceType) {
            return ec2InstanceTypes.Where(instance => instance.InstanceType == instanceType).First();
        }
    }
}