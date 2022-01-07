using KronosData.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace KronosData.Logic
{
    public static class Summarizer
    {
        private enum TimeFrame { Day, Week, Month, Year}
        private static User currentUser;
        private static DateTime targetDate;

        public static SummaryInfo GetSummaryFromDay(User user, DateTime date)
        {
            return GetSummary(user, date, TimeFrame.Day);
        }

        public static SummaryInfo GetSummaryFromWeek(User user, DateTime date)
        {
            return GetSummary(user, date, TimeFrame.Week);
        }

        public static SummaryInfo GetSummaryFromMonth(User user, DateTime date)
        {
            return GetSummary(user, date, TimeFrame.Month);
        }

        public static SummaryInfo GetSummaryFromYear(User user, DateTime date)
        {
            return GetSummary(user, date, TimeFrame.Year);
        }

        #region Private methods

        private static SummaryInfo GetSummary(User user, DateTime date, TimeFrame timeFrame)
        {
            currentUser = user;
            targetDate = date;
            var retVal = new SummaryInfo();

            retVal.TotalWorkHours = GetTotalWorkHours(timeFrame);
            retVal.RequiredWorkHours = GetRequiredWorkHours(timeFrame);
            retVal.TotalOvertime = GetTotalOvertime(timeFrame);
            retVal.TotalAccountedHours = GetTotalAccountedHours(timeFrame);

            return retVal;
        }

        private static TimeSpan GetTotalWorkHours(TimeFrame timeframe)
        {
            var retVal = TimeSpan.Zero;
            IEnumerable<WorkDay> wDays = GetRange(timeframe);

            foreach (var wDay in wDays)
            {
                retVal = retVal.Add(wDay.TotalWorkTime);
            }

            return retVal;
        }

        private static TimeSpan GetRequiredWorkHours(TimeFrame timeframe)
        {
            var retVal = TimeSpan.Zero;
            IEnumerable<WorkDay> wDays = GetRange(timeframe);

            foreach (var wDay in wDays)
            {
                retVal = retVal.Add(wDay.DailyWorkTime);
            }

            return retVal;
        }

        private static TimeSpan GetTotalOvertime(TimeFrame timeframe)
        {
            var retVal = TimeSpan.Zero;
            IEnumerable<WorkDay> wDays = GetRange(timeframe);

            foreach (var wDay in wDays)
            {
                retVal = retVal.Add(wDay.TotalOverTime);
            }

            return retVal;
        }

        private static TimeSpan GetTotalAccountedHours(TimeFrame timeframe)
        {
            var retVal = TimeSpan.Zero;
            IEnumerable<WorkDay> wDays = GetRange(timeframe);

            foreach (var wDay in wDays)
            {
                retVal = retVal.Add(wDay.TotalAccountedTime);
            }

            return retVal;
        }

        private static IEnumerable<WorkDay> GetRange(TimeFrame timeframe)
        {
            switch (timeframe)
            {
                case TimeFrame.Day:
                    return currentUser.AssignedWorkDays.Where(d => d.WorkTime.DateOfWork == targetDate);
                case TimeFrame.Week:
                    return GetWorkWeek();
                case TimeFrame.Month:
                    return currentUser.AssignedWorkDays.Where(d => d.WorkTime.DateOfWork.Month == targetDate.Month && d.WorkTime.DateOfWork.Year == targetDate.Year);
                case TimeFrame.Year:
                    return currentUser.AssignedWorkDays.Where(d => d.WorkTime.DateOfWork.Year == targetDate.Year);
                default:
                    return null;
            }
        }

        private static IEnumerable<WorkDay> GetWorkWeek()
        {
            var ci = new CultureInfo("de-DE");
            var cwr = ci.DateTimeFormat.CalendarWeekRule;
            var fdow = ci.DateTimeFormat.FirstDayOfWeek;
            var cal = ci.Calendar;

            var retVal = new List<WorkDay>();

            foreach (var wDay in currentUser.AssignedWorkDays)
            {
                if (cal.GetWeekOfYear(targetDate, cwr, fdow) == cal.GetWeekOfYear(wDay.WorkTime.DateOfWork, cwr, fdow) && targetDate.Year == wDay.WorkTime.DateOfWork.Year)
                {
                    retVal.Add(wDay);
                }
            }

            return retVal;
        }

        #endregion
    }
}
