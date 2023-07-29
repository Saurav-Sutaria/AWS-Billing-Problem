using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution
{
    internal class Customer
    {
        [Name("Customer ID")]
        public string Id { get; set; }

        [Name("Customer Name")]
        public string Name { get; set; }

        public Dictionary<string,Instance> Instances;
    }
}
