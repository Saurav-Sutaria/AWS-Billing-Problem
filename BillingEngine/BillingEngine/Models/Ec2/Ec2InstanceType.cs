namespace BillingEngine.Models.Ec2
{
    public class Ec2InstanceType
    {
        public string InstanceType { get; }
        public double CostPerHour { get; }
        public Ec2Region Region { get; }
        public OperatingSystem OperatingSystem { get; }
        public BillingType BillingType { get; }
        public bool IsFreeTierEligible { get; }

        public Ec2InstanceType(string InstanceType, double CostPerHour,Ec2Region region,OperatingSystem os,BillingType billingType,bool freeTierEligible)
        {
            this.InstanceType = InstanceType;
            this.CostPerHour = CostPerHour;
            this.Region = region;
            this.BillingType = billingType;
            this.OperatingSystem = os;
            IsFreeTierEligible = freeTierEligible;
        }
    }
}