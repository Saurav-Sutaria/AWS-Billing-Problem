using System;
using System.Globalization;

namespace Solution
{
    internal class Helper
    {
        public static string ConvertToHour(TimeSpan t)
        {
            int hours = (int)t.TotalHours;
            return hours.ToString() + "." + t.Minutes.ToString() + "." + t.Seconds.ToString();
        }
        public static TimeSpan RoundToNearestNextHour(TimeSpan time)
        {
            // Get the total second in the TimeSpan
            double totalSeconds = time.TotalSeconds;
            int newHour = (int)(Math.Ceiling(totalSeconds / 3600.0));
            // Convert back to TimeSpan
            TimeSpan roundedTime = new TimeSpan(0, newHour, 0, 0);
            return roundedTime;
        }
        public static string GetFullMonthName(string monthInitial)
        {
            // Example with the current culture
            CultureInfo currentCulture = CultureInfo.CurrentCulture;

            // Get the array of full month names in the current culture
            string[] fullMonthNames = currentCulture.DateTimeFormat.MonthNames;

            // Find the index of the month initial (ignoring case)
            int index = Array.FindIndex(fullMonthNames, name => name.StartsWith(monthInitial, StringComparison.OrdinalIgnoreCase));

            // Return the full month name if found, otherwise return an empty string
            return index >= 0 ? fullMonthNames[index] : string.Empty;
        }

    }
}
