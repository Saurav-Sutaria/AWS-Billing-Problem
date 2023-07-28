using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution
{
    internal class Instance
    {
        public string type;
        public double charge;
        public HashSet<string> ids;
        public List<string> sessions;
        public string totalTime;
        public string billedTime;
        public string amount;
    }
}
