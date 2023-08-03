using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingEngine.Helper
{
    internal class HelperMethods
    {
        public static string ConvertToHour(TimeSpan t)
        {
            int hours = (int)t.TotalHours;
            return hours.ToString() + "." + t.Minutes.ToString() + "." + t.Seconds.ToString();
        }
    }
}
