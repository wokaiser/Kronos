using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosData.Model
{
    public class WorkDay
    {
        public enum ShiftType { X_Shift = 1, Y_Shift, C_Shift }

        public WorkDay()
        {
            Date = DateTime.Now;
            AssignedWorkItems = new List<WorkItem>();
        }

        public WorkDay(DateTime date)
        {
            Date = date;
            AssignedWorkItems = new List<WorkItem>();
        }

        #region Properties

        public DateTime Date { get; }

        public ShiftType Shift { get; set; }

        public List<WorkItem> AssignedWorkItems { get; }

        #endregion
    }
}
