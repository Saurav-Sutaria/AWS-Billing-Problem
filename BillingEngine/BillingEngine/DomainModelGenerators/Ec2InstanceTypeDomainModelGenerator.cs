using BillingEngine.Models.Ec2;
using BillingEngine.Parsers.Models;
using System.Collections.Generic;

namespace BillingEngine.DomainModelGenerators
{
    public class Ec2InstanceTypeDomainModelGenerator
    {

        public List<Ec2InstanceType> GenerateEc2InstanceTypes(List<ParsedEc2InstanceType> parsedEc2InstanceTypes)
        {
            // Convert each object of type ParsedEc2InstanceType to Ec2InstanceType
            List<Ec2InstanceType> convertedEc2InstanceTypes = new List<Ec2InstanceType>();
            foreach (var data in parsedEc2InstanceTypes)
            {
                Ec2InstanceType newEc2InstanceType = new Ec2InstanceType(data.Ec2InstanceType, double.Parse(data.CostPerHourOnDemand[1..]),data.RegionName);
                convertedEc2InstanceTypes.Add(newEc2InstanceType);
            }
            return convertedEc2InstanceTypes;
        }
    }
}