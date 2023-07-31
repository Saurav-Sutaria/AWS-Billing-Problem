namespace Enhancement0
{
    internal class Resource
    {
        public string resourceType;
        public int totalResources;
        public TimeSpan totalTime;
        public TimeSpan billedTime;
        public double ratePerHour;
        public double totalAmount;

        public void PrintResource(Resource r)
        {
            Console.WriteLine(r.resourceType + " , " + r.totalResources + " , " + Helper.ConvertToHour(r.totalTime) + " , " + Helper.ConvertToHour(r.billedTime) + " , " + r.ratePerHour + " , " + r.totalAmount);
        }
    }
}
