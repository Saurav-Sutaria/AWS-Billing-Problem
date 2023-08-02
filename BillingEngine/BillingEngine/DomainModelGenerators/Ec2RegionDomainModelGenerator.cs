using BillingEngine.Models.Ec2;
using BillingEngine.Parsers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Ec2Region convertedRegion = new Ec2Region(data.RegionName);
                convertedEc2RegionTypes.Add(convertedRegion);
            }
            return convertedEc2RegionTypes;
        }
    }
}
