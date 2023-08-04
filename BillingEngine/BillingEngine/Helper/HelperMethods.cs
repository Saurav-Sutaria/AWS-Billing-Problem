using System;
using System.Collections.Generic;
using System.Globalization;
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
            return hours.ToString() + ":" + t.Minutes.ToString() + ":" + t.Seconds.ToString();
        }
        public static string GetMonthName(int month)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

        }
        public static string GetMonthInitial(int month)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month).ToUpper();
        }
    }
}
