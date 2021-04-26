using System;
using System.Collections.Generic;

namespace KronosData.Model
{
    public class WorkDay
    {
        public enum DayTypeEnum { Default = 0, HomeOffice }
        public enum ShiftTypeEnum { None = 0, X_Shift, Y_Shift, Person_C }

        public WorkDay(DateTime date, ShiftTypeEnum shift, DayTypeEnum dayType)
        {
            Date = date;
            Shift = shift;
            DayType = dayType;
            AssignedWorkItems = new List<WorkItem>();
            DailyWorkTime = new TimeSpan(7, 0, 0);
        }

        public TimeSpan GetTotalWorkTime()
        {
            var retVal = new TimeSpan(0);

            foreach (var item in AssignedWorkItems)
            {
                retVal += item.Duration;
            }

            if (DayType == DayTypeEnum.Default)
            {
                retVal -= new TimeSpan(0, 15, 0);
            }

            return retVal;
        }

        #region Properties

        /// <summary>
        /// The date of the work day
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// The type of workday
        /// </summary>
        public DayTypeEnum DayType  { get; set; }

        /// <summary>
        /// The type of shift of the workday
        /// </summary>
        public ShiftTypeEnum Shift { get; set; }

        /// <summary>
        /// The total time of work to be done at a day
        /// </summary>
        public TimeSpan DailyWorkTime { get; set; }

        /// <summary>
        /// The assigned work items of this day
        /// </summary>
        public List<WorkItem> AssignedWorkItems { get; }

        #endregion
    }
}
