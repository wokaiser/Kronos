using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosData.Model
{
    public class User : DB_Access
    {
        public User()
        {
            UserName = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            AssignedWorkItems = new List<WorkItem>();
        }

        public List<WorkItem> GetItemsOfDay(DateTime desiredDay)
        {
            return AssignedWorkItems.Where(d => d.Begin.Date.Equals(desiredDay.Date)).ToList();
        }

        public List<WorkItem> GetItemsOfMonth(DateTime desiredMonth)
        {
            return AssignedWorkItems.Where(d => d.Begin.Year == desiredMonth.Year && d.Begin.Month == desiredMonth.Month).ToList();
        }

        public List<WorkItem> GetItemsOfYear(DateTime desiredYear)
        {
            return AssignedWorkItems.Where(d => d.Begin.Year == desiredYear.Year).ToList();
        }

        #region Properties

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<WorkItem> AssignedWorkItems { get; }

        #endregion

    }
}
