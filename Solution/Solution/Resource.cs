using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution
{
    internal class Resource
    {
        public string resourceType;
        public int totalResources;
        public TimeSpan totalTime;
        public TimeSpan billedTime;
        public double ratePerHour;
        public double totalAmount;
    }
}
