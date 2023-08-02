using System.Collections.Generic;
using BillingEngine.Models.Ec2;
using BillingEngine.Parsers.Models;

namespace BillingEngine.DomainModelGenerators
{
    public class Ec2InstanceDomainModelGenerator
    {
        public List<Ec2Instance> GenerateEc2InstanceModels(
            List<ParsedEc2ResourceUsageEventRecord> parsedEc2ResourceUsageTypeEventRecords,
            List<Ec2InstanceType> ec2InstanceTypes)
        {
            List<Ec2Instance> ec2Instances = new List<Ec2Instance>();
            foreach(var record in parsedEc2ResourceUsageTypeEventRecords)
            {
                var ec2Instance = ec2Instances.FindEc2Instance(record.Ec2InstanceId);
                //instance not exists
                if(ec2Instance == null)
                {
                    Ec2Instance ec = new Ec2Instance(record.Ec2InstanceId,ec2InstanceTypes.FindEc2InstanceType(record.Ec2InstanceType));
                    ResourceUsageEvent resourceUsage = new ResourceUsageEvent(record.UsedFrom, record.UsedUntil);
                    ec.Usages.Add(resourceUsage);
                    ec2Instances.Add(ec);
                }
                else
                {
                    ResourceUsageEvent resourceUsage = new ResourceUsageEvent(record.UsedFrom, record.UsedUntil);
                    ec2Instance.Usages.Add(resourceUsage);
                }
            }
            return ec2Instances;
        }
    }
}