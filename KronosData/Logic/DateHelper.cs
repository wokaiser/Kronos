using System;
using System.Globalization;

namespace KronosData.Logic
{
    public class DateHelper
    {
        /// <summary>
        /// Returns a string containging the calendar week of the given date
        /// </summary>
        /// <param name="date">The date to get the calendar week of of</param>
        /// <returns>A string containing the calendar week</returns>
        public static string GetCalenderWeekFromDate(DateTime date)
        {
            string prefix = "CW";

            var ci = CultureInfo.CurrentCulture;
            var week = ci.Calendar.GetWeekOfYear(date, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);

            if (CultureInfo.CurrentCulture.Equals(new CultureInfo("de-DE")))
            {
                prefix = "KW";
            }

            return prefix + week;
        }

        /// <summary>
        /// Returns an abbreviated month name, in accordance to the current culture
        /// </summary>
        /// <param name="date">The date to get the month name from</param>
        /// <returns>A string containing the abbreviated month name</returns>
        public static string GetMonthNameFromDate(DateTime date)
        {
            return date.ToString("MMM", CultureInfo.CurrentCulture);
        }
    }
}
