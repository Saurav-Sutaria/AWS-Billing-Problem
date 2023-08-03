using BillingEngine.DomainModelGenerators;
using BillingEngine.Models.Billing;
using BillingEngine.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BillingEngine.Billing
{
    public class BillingService
    {
        private readonly CustomerCsvParser _customerCsvParser;
        private readonly Ec2ResourceUsageTypeEventParser _resourceUsageTypeEventParser;
        private readonly Ec2InstanceTypeCsvParser _instanceTypeCsvParser;
        private readonly Ec2RegionCsvParser _regionCsvParser;
        private readonly CustomerDomainModelGenerator _customerDomainModelGenerator;
        private readonly CustomerLevelMonthlyBillingService _customerLevelMonthlyBillingService;


        public BillingService()
        {
            _customerCsvParser = new CustomerCsvParser();
            _resourceUsageTypeEventParser = new Ec2ResourceUsageTypeEventParser();
            _instanceTypeCsvParser = new Ec2InstanceTypeCsvParser();
            _regionCsvParser = new Ec2RegionCsvParser();

            _customerDomainModelGenerator = new CustomerDomainModelGenerator();

            _customerLevelMonthlyBillingService = new CustomerLevelMonthlyBillingService();
        }

        public List<MonthlyBill> GenerateMonthlyBills(
            string customerCsvPath,
            string resourceTypeCsvPath,
            string resourceUsageCsvPath,
            string regionCsvPath)
        {
            //parse each input file and store its data
            var parsedCustomerRecords = _customerCsvParser.Parse(customerCsvPath);

            var parsedEc2ResourceUsageEventRecords = _resourceUsageTypeEventParser.Parse(resourceUsageCsvPath);
            
            var parsedEc2InstanceTypes = _instanceTypeCsvParser.Parse(resourceTypeCsvPath);
  
            var parsedEc2Regions = _regionCsvParser.Parse(regionCsvPath);

            //generates customer object
            var customers = _customerDomainModelGenerator.GenerateCustomerModels(
                parsedCustomerRecords,
                parsedEc2InstanceTypes,
                parsedEc2Regions,
                parsedEc2ResourceUsageEventRecords);
            
            //generate monthly bills and returns list of monthly bills
            return customers
                .SelectMany(_customerLevelMonthlyBillingService.GenerateMonthlyBillsForCustomer)
                .ToList();
        }
    }
}