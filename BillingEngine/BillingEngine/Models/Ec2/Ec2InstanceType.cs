namespace BillingEngine.Models.Ec2
{
    public class Ec2InstanceType
    {
        public string InstanceType { get; }
        public double CostPerHourOnDemand { get; }
        public Ec2Region Region { get; }
        public OperatingSystem OperatingSystem { get; }
        public BillingType BillingType { get; }
        public bool IsFreeTierEligible { get; }

        public Ec2InstanceType(string InstanceType, double CostPerHour,string RegionName)
        {
            this.InstanceType = InstanceType;
            this.CostPerHourOnDemand = CostPerHour;
            this.Region = new Ec2Region(RegionName);
            this.OperatingSystem = OperatingSystem.Linux;
            this.BillingType = BillingType.OnDemand;
            this.IsFreeTierEligible = false;
        }
    }
}