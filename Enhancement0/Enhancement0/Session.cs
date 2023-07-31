using System.Globalization;

namespace Enhancement0
{
    internal class Session
    {
        public string startTime;
        public string endTime;
        public TimeSpan totalTime;
        public static void GenerateMontlySessions(Dictionary<string, Customer> customerMap)
        {
            foreach (KeyValuePair<string, Customer> kvp in customerMap)
            {
                Customer c = kvp.Value;
                foreach (Instance i in c.Instances.Values)
                {
                    foreach (Session s in i.sessions)
                    {
                        s.GenerateSession(s, i);
                    }
                }
            }
        }
        public void GenerateSession(Session s, Instance i)
        {
            //parse time string
            DateTime startDate = DateTime.Parse(s.startTime);
            DateTime endDate = DateTime.Parse(s.endTime);
            //Console.WriteLine(startDate + " " + endDate);
            while (startDate < endDate)
            {
                string monthId = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(startDate.Month).Substring(0, 3).ToUpper() + "-" + startDate.Year;

                //check if the month id already exists or not
                if (!i.monthlySessions.ContainsKey(monthId))
                {
                    //month not exists -> add new month id and its month session
                    MonthSession newMonthSession = new MonthSession();
                    newMonthSession.sessions = new List<Session>();
                    i.monthlySessions.Add(monthId, newMonthSession);

                }
                DateTime endOfMonth = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month), 23, 59, 59);
                //Console.WriteLine(monthId);
                if (endOfMonth <= endDate)
                {
                    Session newSession = new Session();

                    newSession.startTime = $"{startDate}";
                    newSession.endTime = $"{endOfMonth}";
                    //start time is 23:59:59 of the last day of last month
                    if (startDate == endOfMonth) newSession.totalTime = new TimeSpan(0, 0, 1);
                    else newSession.totalTime = endOfMonth - startDate;
                    if (newSession.totalTime.Minutes == 59 && newSession.totalTime.Seconds == 59)
                    {
                        newSession.totalTime = new TimeSpan(newSession.totalTime.Days, newSession.totalTime.Hours + 1, 0, 0);
                    }
                    startDate = endOfMonth.AddSeconds(1);
                    //Console.WriteLine(newSession.startTime + " -- " + newSession.endTime + " -- " + newSession.totalTime);
                    i.monthlySessions[monthId].sessions.Add(newSession);
                }
                else
                {
                    Session newSession = new Session();
                    newSession.startTime = $"{startDate}";
                    newSession.endTime = $"{endDate}";
                    newSession.totalTime = endDate - startDate;
                    // Console.WriteLine(newSession.startTime + " -- " + newSession.endTime + " -- " + newSession.totalTime);
                    i.monthlySessions[monthId].sessions.Add(newSession);
                    break;
                }
            }
        }
    }
}
