using BillingEngine.Models.Ec2;
using BillingEngine.Parsers.Models;
using System.Collections.Generic;

namespace BillingEngine.DomainModelGenerators
{
    internal class Ec2RegionDomainModelGenerator
    {
        public List<Ec2Region> GenerateEc2RegionTypes(List<ParsedEc2Region> parsedEc2RegionTypes)
        {
            // Convert each object of type ParsedEc2InstanceType to Ec2InstanceType
            List<Ec2Region> convertedEc2RegionTypes = new List<Ec2Region>();
            foreach (var data in parsedEc2RegionTypes)
            {
                Ec2Region convertedRegion = new Ec2Region(data.RegionName,data.FreeTierEligibleInstanceType);
                convertedEc2RegionTypes.Add(convertedRegion);
            }
            return convertedEc2RegionTypes;
        }
    }
}
