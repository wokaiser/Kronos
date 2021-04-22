using KronosData.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KronosData.Logic
{
    public class DataManager
    {
        private User currentUser;

        public DataManager(User user)
        {
            currentUser = user;
        }

        /// <summary>
        /// Gets all items of the desired day
        /// </summary>
        /// <param name="desiredDay">The day to lookup</param>
        /// <returns>All work items created at the given day</returns>
        public List<WorkItem> GetItemsOfDay(DateTime desiredDay)
        {
            if (currentUser == null)
            {
                return null;
            }

            var day = currentUser.AssignedWorkDays.Find(d => d.Date.Date.Equals(desiredDay.Date));

            if (day == null)
            {
                return null;
            }

            return day.AssignedWorkItems;
        }

        /// <summary>
        /// Gets all items of the desired month
        /// </summary>
        /// <param name="desiredMonth">The month to lookup</param>
        /// <returns>All work items created at the given month</returns>
        public List<WorkItem> GetItemsOfMonth(DateTime desiredMonth)
        {
            var retVal = new List<WorkItem>();

            foreach (var item in currentUser.AssignedWorkDays.Where(d => d.Date.Year == desiredMonth.Year && d.Date.Month == desiredMonth.Month))
            {
                retVal.AddRange(item.AssignedWorkItems);
            }

            return retVal;
        }

        /// <summary>
        /// Gets all items of the desired year
        /// </summary>
        /// <param name="desiredYear">The year to look up</param>
        /// <returns>All work items created at the given year</returns>
        public List<WorkItem> GetItemsOfYear(DateTime desiredYear)
        {
            var retVal = new List<WorkItem>();

            foreach (var item in currentUser.AssignedWorkDays.Where(d => d.Date.Year == desiredYear.Year))
            {
                retVal.AddRange(item.AssignedWorkItems);
            }

            return retVal;
        }

        public TimeSpan GetOvertimeOfDay(DateTime desiredDay)
        {
            var day = currentUser.AssignedWorkDays.Find(d => d.Date.Date.Equals(desiredDay.Date));

            if (day == null)
            {
                return new TimeSpan(0);
            }

            return day.GetTotalWorkTime().Subtract(day.DailyWorkTime);
        }

        public List<Account> GetAllAccounts()
        {
            var retVal = new List<Account>();

            foreach (var workDay in currentUser.AssignedWorkDays)
            {
                foreach (var workItem in workDay.AssignedWorkItems)
                {
                    if (retVal.Contains(workItem.AssignedWorkTask.AssignedAccount))
                    {
                        continue;
                    }

                    retVal.Add(workItem.AssignedWorkTask.AssignedAccount);
                }
            }

            return retVal;
        }

        public List<WorkTask> GetAllTasks()
        {
            var retVal = new List<WorkTask>();

            foreach (var workDay in currentUser.AssignedWorkDays)
            {
                foreach (var workItem in workDay.AssignedWorkItems)
                {
                    if (retVal.Contains(workItem.AssignedWorkTask))
                    {
                        continue;
                    }

                    retVal.Add(workItem.AssignedWorkTask);
                }
            }

            return retVal;
        }
    }
}
