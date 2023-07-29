using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace Solution
{
    internal class ResourceUsage
    {
        [Name("Customer ID")]
        public string Id { get; set; }

        [Name("EC2 Instance ID")]
        public string InstanceID { get; set; }

        [Name("EC2 Instance Type")]
        public string InstanceType { get; set; }

        [Name("Used From")]
        public string StartTime { get; set; }

        [Name("Used Until")]
        public string EndTime { get; set; } 

    }
}
