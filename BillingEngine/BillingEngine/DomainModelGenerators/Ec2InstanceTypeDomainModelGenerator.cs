using BillingEngine.Models.Ec2;
using BillingEngine.Parsers.Models;
using System;
using System.Collections.Generic;
using OperatingSystem = BillingEngine.Models.Ec2.OperatingSystem;

namespace BillingEngine.DomainModelGenerators
{
    public class Ec2InstanceTypeDomainModelGenerator
    {

        public List<Ec2InstanceType> GenerateEc2InstanceTypes(List<ParsedEc2InstanceType> parsedEc2InstanceTypes, List<Ec2Region> ec2Regions)
        {
            // Convert each object of type ParsedEc2InstanceType to Ec2InstanceType
            List<Ec2InstanceType> convertedEc2InstanceTypes = new List<Ec2InstanceType>();
            foreach (var data in parsedEc2InstanceTypes)
            {
                var ec2InstaceType = data.Ec2InstanceType;
                var costOnDemand = double.Parse(data.CostPerHourOnDemand[1..]);
                var costReserved = double.Parse(data.CostPerHourReserved[1..]);
                var region = ec2Regions.FindRegion(data.RegionName);
                bool freeTierEligible = region.isFreeTierEligible(ec2InstaceType);

                //generate all possible combination of OS & Billing type
                //combination 1 : windows , onDemand
                OperatingSystem os = OperatingSystem.Windows;
                BillingType billingType = BillingType.OnDemand;
                Ec2InstanceType newEc2InstanceType = new Ec2InstanceType(ec2InstaceType,costOnDemand,region,os,billingType,freeTierEligible);
                convertedEc2InstanceTypes.Add(newEc2InstanceType);

                //combination 2 : linux , ondemand
                os = OperatingSystem.Linux;
                newEc2InstanceType = new Ec2InstanceType(ec2InstaceType, costOnDemand, region, os, billingType,freeTierEligible);
                convertedEc2InstanceTypes.Add(newEc2InstanceType);

                //combination 3 : windows, reserved
                freeTierEligible = false;
                os = OperatingSystem.Windows;
                billingType = BillingType.Reserved;
                newEc2InstanceType = new Ec2InstanceType(ec2InstaceType, costReserved, region, os, billingType, freeTierEligible);
                convertedEc2InstanceTypes.Add(newEc2InstanceType);

                //combination 4 : linux, reserved
                os = OperatingSystem.Linux;
                newEc2InstanceType = new Ec2InstanceType(ec2InstaceType, costReserved, region, os, billingType, freeTierEligible);
                convertedEc2InstanceTypes.Add(newEc2InstanceType);
            }
            return convertedEc2InstanceTypes;
        }
    }
}