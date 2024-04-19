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

        /// <summary>
        /// Gets a overview of each task for the given user in the given month
        /// </summary>
        /// <param name="user">The user to look for</param>
        /// <param name="date">The month to look for</param>
        /// <returns>The amount of work for each task</returns>
        public static Dictionary<WorkTask, TimeSpan> GetTaskOverviewFromMonth(User user, DateTime date) 
        {
            var retVal = new Dictionary<WorkTask, TimeSpan>();
            var wDays = GetRange(user, date, TimeFrame.Month);

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

        /// <summary>
        /// Gets a overview of each account for the given user in the given year
        /// </summary>
        /// <param name="user">The user to look for</param>
        /// <param name="date">The month to look for</param>
        /// <returns>The amount of work for each account</returns>
        public static IEnumerable<WorkDay> GetAccountOverviewFromMonth(User user, DateTime date)
        {
            return GetRange(user, date, TimeFrame.Month);
        }

        #region Private methods

        private static SummaryInfo GetSummary(User user, DateTime date, TimeFrame timeFrame)
        {
            return new SummaryInfo(GetTotalWorkHours(user, date, timeFrame),
                GetRequiredWorkHours(user, date, timeFrame), 
                GetTotalOvertime(user, date, timeFrame), 
                GetTotalAccountedHours(user, date, timeFrame), 
                GetTotalMobileDays(user, date, timeFrame), 
                GetTotalFreeDays(user, date, timeFrame),
                GetTotalSickDays(user, date, timeFrame));
        }

        private static TimeSpan GetTotalWorkHours(User user, DateTime date, TimeFrame timeframe)
        {
            var retVal = TimeSpan.Zero;
            IEnumerable<WorkDay> wDays = GetRange(user, date, timeframe);

            foreach (var wDay in wDays)
            {
                retVal = retVal.Add(wDay.TotalWorkTime);
            }

            return retVal;
        }

        private static TimeSpan GetRequiredWorkHours(User user, DateTime date, TimeFrame timeframe)
        {
            var retVal = TimeSpan.Zero;
            IEnumerable<WorkDay> wDays = GetRange(user, date, timeframe);

            foreach (var wDay in wDays)
            {
                retVal = retVal.Add(wDay.DailyWorkTime);
            }

            return retVal;
        }

        private static TimeSpan GetTotalOvertime(User user, DateTime date, TimeFrame timeframe)
        {
            var retVal = TimeSpan.Zero;
            IEnumerable<WorkDay> wDays = GetRange(user, date, timeframe);

            foreach (var wDay in wDays)
            {
                retVal = retVal.Add(wDay.TotalOverTime);
            }

            return retVal;
        }

        private static TimeSpan GetTotalAccountedHours(User user, DateTime date, TimeFrame timeframe)
        {
            var retVal = TimeSpan.Zero;
            IEnumerable<WorkDay> wDays = GetRange(user, date, timeframe);

            foreach (var wDay in wDays)
            {
                retVal = retVal.Add(wDay.TotalAccountedTime);
            }

            return retVal;
        }

        private static int GetTotalMobileDays(User user, DateTime date, TimeFrame timeframe)
        {
            return GetRange(user, date, timeframe).Where(d => d.IsMobileDay).Count();
        }

        private static int GetTotalFreeDays(User user, DateTime date, TimeFrame timeframe)
        {
            return GetRange(user, date, timeframe).Where(d => d.IsFreeDay).Count();
        }

        private static int GetTotalSickDays(User user, DateTime date, TimeFrame timeframe)
        {
            return GetRange(user, date, timeframe).Where(d => d.IsSickDay).Count();
        }

        /// <summary>
        /// Returns the range for the given TimeFrame
        /// </summary>
        /// <param name="user">The user to look in</param>
        /// <param name="date">The day to look for</param>
        /// <param name="timeframe">The timeframe to look for</param>
        /// <returns>All days found wihtin the given TimeFrame</returns>
        private static IEnumerable<WorkDay> GetRange(User user, DateTime date, TimeFrame timeframe)
        {
            switch (timeframe)
            {
                case TimeFrame.Day:
                    return user.AssignedWorkDays.Where(d => d.WorkTime.DateOfWork == date);

                case TimeFrame.Week:
                    return GetWorkWeek(user, date);

                case TimeFrame.Month:
                    return user.AssignedWorkDays.Where(d => d.WorkTime.DateOfWork.Month == date.Month && d.WorkTime.DateOfWork.Year == date.Year);

                case TimeFrame.Year:
                    return user.AssignedWorkDays.Where(d => d.WorkTime.DateOfWork.Year == date.Year);

                default:
                    return null;
            }
        }

        /// <summary>
        /// Extracts work week from specified TimeFrame
        /// </summary>
        /// <param name="user">The user to look in</param>
        /// <param name="date">The day to look for</param>
        /// <returns>All days found within given TimeFrame</returns>
        private static IEnumerable<WorkDay> GetWorkWeek(User user, DateTime date)
        {
            var ci = new CultureInfo("de-DE");
            var cwr = ci.DateTimeFormat.CalendarWeekRule;
            var fdow = ci.DateTimeFormat.FirstDayOfWeek;
            var cal = ci.Calendar;

            var retVal = new List<WorkDay>();

            foreach (var wDay in user.AssignedWorkDays)
            {
                if (cal.GetWeekOfYear(date, cwr, fdow) == cal.GetWeekOfYear(wDay.WorkTime.DateOfWork, cwr, fdow) && date.Year == wDay.WorkTime.DateOfWork.Year)
                {
                    retVal.Add(wDay);
                }
            }

            return retVal;
        }

        #endregion
    }
}
