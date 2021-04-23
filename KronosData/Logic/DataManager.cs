using KronosData.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KronosData.Logic
{
    public class DataManager
    {
        private static readonly string savePath = @"C:\temp\test.json";

        /// <summary>
        /// Creates a DataManager instance and loads the default user file
        /// </summary>
        public DataManager()
        {
            CurrentUser = User.DeserializeFromFile(savePath);
        }

        /// <summary>
        /// Loads a user into the data manager
        /// </summary>
        /// <param name="user">The user instance to load</param>
        public void LoadUser(User user)
        {
            CurrentUser = user;
        }

        /// <summary>
        /// Loads a user into the data manager
        /// </summary>
        /// <param name="path">The path to the user file to load</param>
        public void LoadUser(string path)
        {
            CurrentUser = User.DeserializeFromFile(path);
        }

        /// <summary>
        /// Gets all items of the desired day
        /// </summary>
        /// <param name="desiredDay">The day to lookup</param>
        /// <returns>All work items created at the given day</returns>
        public List<WorkItem> GetItemsOfDay(DateTime desiredDay)
        {
            if (CurrentUser == null)
            {
                return null;
            }

            var day = CurrentUser.AssignedWorkDays.Find(d => d.Date.Date.Equals(desiredDay.Date));

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

            foreach (var item in CurrentUser.AssignedWorkDays.Where(d => d.Date.Year == desiredMonth.Year && d.Date.Month == desiredMonth.Month))
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

            foreach (var item in CurrentUser.AssignedWorkDays.Where(d => d.Date.Year == desiredYear.Year))
            {
                retVal.AddRange(item.AssignedWorkItems);
            }

            return retVal;
        }

        /// <summary>
        /// Calculates the overtime of the given day
        /// </summary>
        /// <param name="desiredDay">The day to check the overtime of</param>
        /// <returns>The total overtime. Note that the overtime can be negative</returns>
        public TimeSpan GetOvertimeOfDay(DateTime desiredDay)
        {
            var day = CurrentUser.AssignedWorkDays.Find(d => d.Date.Date.Equals(desiredDay.Date));

            if (day == null)
            {
                return new TimeSpan(0);
            }

            return day.GetTotalWorkTime().Subtract(day.DailyWorkTime);
        }

        /// <summary>
        /// Returns all the accounts used by the loaded user
        /// </summary>
        /// <returns>A list containing all accounts</returns>
        public List<Account> GetAllAccounts()
        {
            var retVal = new List<Account>();

            foreach (var workDay in CurrentUser.AssignedWorkDays)
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

        /// <summary>
        /// Returns all the task used by the loaded user
        /// </summary>
        /// <returns>A list containing all tasks</returns>
        public List<WorkTask> GetAllTasks()
        {
            var retVal = new List<WorkTask>();

            foreach (var workDay in CurrentUser.AssignedWorkDays)
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

        #region Properties

        /// <summary>
        /// The current user loaded into the data manager
        /// </summary>
        public User CurrentUser { get; private set; }

        #endregion

    }
}
