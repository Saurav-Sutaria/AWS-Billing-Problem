﻿namespace Enhancement1
{
    internal class MonthSession
    {
        public List<Session> sessions;

        public void PrintSession(MonthSession m)
        {
            foreach (Session s in m.sessions)
            {
                Console.WriteLine(s.startTime + " -- " + s.endTime + " -- " + s.totalTime);
            }
        }
    }
}