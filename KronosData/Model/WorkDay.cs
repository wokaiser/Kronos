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

        public DateTime Date { get; }

        public DayTypeEnum DayType  { get; set; }

        public ShiftTypeEnum Shift { get; set; }

        public TimeSpan DailyWorkTime { get; set; }

        public List<WorkItem> AssignedWorkItems { get; }

        #endregion
    }
}
