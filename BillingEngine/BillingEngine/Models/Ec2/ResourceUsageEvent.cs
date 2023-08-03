using System;
using System.Globalization;

namespace BillingEngine.Models.Ec2
{
    public class ResourceUsageEvent
    {
        public DateTime UsedFrom { get; set; }

        public DateTime UsedUntil { get; set; }

        public ResourceUsageEvent() { }
        public ResourceUsageEvent(DateTime UsedFrom, DateTime UsedUntil)
        {
            this.UsedFrom = UsedFrom;
            this.UsedUntil = UsedUntil;
        }

        public int GetBillableHours()
        {
            var usedHours = UsedUntil.Subtract(UsedFrom).TotalHours;
            return Convert.ToInt32(Math.Ceiling(usedHours));
        }
        public TimeSpan GetTotalTime()
        {
            return UsedUntil.Subtract(UsedFrom);
        }
    }
}