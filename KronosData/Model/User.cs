using System;
using System.Collections.Generic;
using System.Linq;

namespace KronosData.Model
{
    public class User : DB_Access
    {
        /// <summary>
        /// Creates a new user object
        /// </summary>
        /// <param name="username">The username for the user</param>
        public User(string username)
        {
            UserName = username;
            FirstName = string.Empty;
            LastName = string.Empty;
            AssignedWorkItems = new List<WorkItem>();
        }

        /// <summary>
        /// Gets all items of the desired day
        /// </summary>
        /// <param name="desiredDay">The day to lookup</param>
        /// <returns>All work items created at the given day</returns>
        public List<WorkItem> GetItemsOfDay(DateTime desiredDay)
        {
            return AssignedWorkItems.Where(d => d.Begin.Date.Equals(desiredDay.Date)).ToList();
        }

        /// <summary>
        /// Gets all items of the desired month
        /// </summary>
        /// <param name="desiredMonth">The month to lookup</param>
        /// <returns>All work items created at the given month</returns>
        public List<WorkItem> GetItemsOfMonth(DateTime desiredMonth)
        {
            return AssignedWorkItems.Where(d => d.Begin.Year == desiredMonth.Year && d.Begin.Month == desiredMonth.Month).ToList();
        }

        /// <summary>
        /// Gets all items of the desired year
        /// </summary>
        /// <param name="desiredYear">The year to look up</param>
        /// <returns>All work items created at the given year</returns>
        public List<WorkItem> GetItemsOfYear(DateTime desiredYear)
        {
            return AssignedWorkItems.Where(d => d.Begin.Year == desiredYear.Year).ToList();
        }

        #region Overrides

        public override string ToString()
        {
            return string.Format("{0}: {1}, {2}", UserName, LastName, FirstName);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The assigned username by the system
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// The first name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The work items assigned to the user
        /// </summary>
        public List<WorkItem> AssignedWorkItems { get; }

        #endregion

    }
}
