namespace Enhancement1
{
    internal class Resource
    {
        public string resourceType;
        public string region;
        public int totalResources;
        public TimeSpan totalTime;
        public TimeSpan billedTime;
        public double ratePerHour;
        public double totalAmount;

        public void PrintResource(Resource r)
        {
            Console.WriteLine(r.region + " , " + r.resourceType + " , " + r.totalResources + " , " + Helper.ConvertToHour(r.totalTime) + " , " + Helper.ConvertToHour(r.billedTime) + " , " + r.ratePerHour + " , " + r.totalAmount);
        }
    }
}
