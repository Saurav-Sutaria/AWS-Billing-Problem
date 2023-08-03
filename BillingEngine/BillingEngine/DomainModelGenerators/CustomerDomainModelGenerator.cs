using BillingEngine.Models;
using BillingEngine.Models.Ec2;
using BillingEngine.Parsers;
using BillingEngine.Parsers.Models;
using System.Collections.Generic;
using System.Linq;

namespace BillingEngine.DomainModelGenerators
{
    public class CustomerDomainModelGenerator
    {
        private readonly Ec2InstanceTypeDomainModelGenerator _ec2InstanceTypeDomainModelGenerator;
        private readonly Ec2RegionDomainModelGenerator _ec2RegionDomainModelGenerator;
        private readonly Ec2InstanceDomainModelGenerator _ec2InstanceDomainModelGenerator;

        public CustomerDomainModelGenerator()
        {
            _ec2InstanceDomainModelGenerator = new Ec2InstanceDomainModelGenerator();
            _ec2InstanceTypeDomainModelGenerator = new Ec2InstanceTypeDomainModelGenerator();
            _ec2RegionDomainModelGenerator = new Ec2RegionDomainModelGenerator();
        }

        public List<Customer> GenerateCustomerModels(
            List<ParsedCustomerRecord> parsedCustomerRecords,
            List<ParsedEc2InstanceType> parsedEc2InstanceTypes,
            List<ParsedEc2Region> parsedEc2Regions,
            List<ParsedEc2ResourceUsageEventRecord> parsedEc2ResourceUsageEventRecords)
        {
            //converts ParsedEc2InstanceType -> Ec2InstanceType
            List<Ec2InstanceType> ec2InstanceTypes = _ec2InstanceTypeDomainModelGenerator
                .GenerateEc2InstanceTypes(parsedEc2InstanceTypes);

            //Generate Ec2Region instances by defining Ec2RegionDomainModelGenerator
            //converts ParsedEc2Region -> Ec2Region
            List<Ec2Region> ec2Regions = _ec2RegionDomainModelGenerator.GenerateEc2RegionTypes(parsedEc2Regions);

            return parsedCustomerRecords.Select(parsedCustomerRecord =>
                    GenerateCustomerModel(
                        parsedCustomerRecord,
                        parsedEc2ResourceUsageEventRecords.FindRecordsForCustomer(parsedCustomerRecord.CustomerId),
                        ec2InstanceTypes,
                        ec2Regions)
                )
                .ToList();
        }

        private Customer GenerateCustomerModel(
            ParsedCustomerRecord parsedCustomerRecord,
            List<ParsedEc2ResourceUsageEventRecord> ec2ResourceUsageEventsForCustomer,
            List<Ec2InstanceType> ec2InstanceTypes,
            List<Ec2Region> ec2Regions)
        {
            // Build customer object as well as associated composite objects, e.g. Ec2Instance, 
            //throw new System.NotImplementedException();
            //generate ec2Instances model for a customer
            List<Ec2Instance> ec2Instances = _ec2InstanceDomainModelGenerator.GenerateEc2InstanceModels(
                ec2ResourceUsageEventsForCustomer, ec2InstanceTypes);

            Customer newCustomer = new Customer(
                parsedCustomerRecord.CustomerId,
                parsedCustomerRecord.CustomerName,
                ec2Instances);
            return newCustomer;
        }
    }
}