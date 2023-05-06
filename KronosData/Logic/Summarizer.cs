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

        /// <summary>
        /// Extracts the SummaryInfo for the specified day
        /// </summary>
        /// <param name="user">The user to look in</param>
        /// <param name="date">The day to look for</param>
        /// <returns>A SummaryInfo object</returns>
        public static SummaryInfo GetSummaryFromDay(User user, DateTime date)
        {
            return GetSummary(user, date, TimeFrame.Day);
        }

        /// <summary>
        /// Extracts the SummaryInfo for the specified week
        /// </summary>
        /// <param name="user">The user to look in</param>
        /// <param name="date">A day within the week to look for</param>
        /// <returns>A SummaryInfo object</returns>
        public static SummaryInfo GetSummaryFromWeek(User user, DateTime date)
        {
            return GetSummary(user, date, TimeFrame.Week);
        }

        /// <summary>
        /// Extracts the SummaryInfo for the specified day
        /// </summary>
        /// <param name="user">The user to look in</param>
        /// <param name="date">A day within the month to look for</param>
        /// <returns>A SummaryInfo object</returns>
        public static SummaryInfo GetSummaryFromMonth(User user, DateTime date)
        {
            return GetSummary(user, date, TimeFrame.Month);
        }

        /// <summary>
        /// Extracts the SummaryInfo for the specified day
        /// </summary>
        /// <param name="user">The user to look in</param>
        /// <param name="date">A day within the year to look for</param>
        /// <returns>A SummaryInfo object</returns>
        public static SummaryInfo GetSummaryFromYear(User user, DateTime date)
        {
            return GetSummary(user, date, TimeFrame.Year);
        }

        public static Dictionary<WorkTask, TimeSpan> GetTaskOverviewFromMonth(User user, DateTime date) 
        {
            var retVal = new Dictionary<WorkTask, TimeSpan>();

            currentUser = user;
            targetDate = date;

            var wDays = GetRange(TimeFrame.Month);

            foreach (var wDay in wDays)
            {
                foreach (var task in wDay.AssignedWorkItems)
                {
                    if (retVal.ContainsKey(task.AssignedWorkTask))
                    {
                        retVal[task.AssignedWorkTask] += task.Duration;
                    }
                    else
                    {
                        retVal.Add(task.AssignedWorkTask, task.Duration);
                    }
                }
            }

            return retVal;
        }

        #region Private methods

        private static SummaryInfo GetSummary(User user, DateTime date, TimeFrame timeFrame)
        {
            currentUser = user;
            targetDate = date;

            return new SummaryInfo(GetTotalWorkHours(timeFrame), GetRequiredWorkHours(timeFrame), GetTotalOvertime(timeFrame), GetTotalAccountedHours(timeFrame), GetTotalMobileDays(timeFrame), GetTotalFreeDays(timeFrame), GetTotalSickDays(timeFrame));
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

        private static int GetTotalMobileDays(TimeFrame timeframe)
        {
            return GetRange(timeframe).Where(d => d.IsMobileDay).Count();
        }

        private static int GetTotalFreeDays(TimeFrame timeframe)
        {
            return GetRange(timeframe).Where(d => d.IsFreeDay).Count();
        }

        private static int GetTotalSickDays(TimeFrame timeframe)
        {
            return GetRange(timeframe).Where(d => d.IsSickDay).Count();
        }

        /// <summary>
        /// Returns the range for the given TimeFrame
        /// </summary>
        /// <param name="timeframe">The timeframe to look for</param>
        /// <returns>All days found wihtin the given TimeFrame</returns>
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

        /// <summary>
        /// Extracts work week from specified TimeFrame
        /// </summary>
        /// <returns>All days found within given TimeFrame</returns>
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
