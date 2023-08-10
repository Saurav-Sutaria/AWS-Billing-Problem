namespace BillingEngine.Models.Ec2
{
    public class Ec2Region
    {
        public string Name { get; }
        public string FreeTierEligibleEc2Instance { get; }
        public Ec2Region(string name)
        {
            this.Name = name;
        }
        public Ec2Region(string regionName,string freeTierEligibleEc2Instance)
        {
            Name = regionName;
            FreeTierEligibleEc2Instance = freeTierEligibleEc2Instance;
        }
        public bool isFreeTierEligible(string ec2InstanceType)
        {
            return FreeTierEligibleEc2Instance == ec2InstanceType;
        }
    }
}