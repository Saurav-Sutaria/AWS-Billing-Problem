using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution
{
    internal class Bill
    {
        public string fileName;
        public string customerName;
        public string month;
        public string year;
        public double totalAmount;
        public Dictionary<string,Resource> resourceUsedInfo;
    }
}