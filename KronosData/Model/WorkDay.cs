using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace KronosData.Model
{
    public class WorkDay
    {
        public enum DayTypeEnum { Default = 0, HomeOffice }
        public enum ShiftTypeEnum { None = 0, X_Shift, Y_Shift, Person_C }

        public WorkDay(ShiftTypeEnum shift, DayTypeEnum dayType)
        {
            WorkTime = new DateUnit();
            Breaks = new ObservableCollection<DateUnit>();
            Shift = shift;
            DayType = dayType;
            AssignedWorkItems = new ObservableCollection<WorkItem>();
            DailyWorkTime = new TimeSpan(7, 0, 0);
        }

        #region Properties

        public DateUnit WorkTime { get; set; }

        public ObservableCollection<DateUnit> Breaks { get; }

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

        public TimeSpan TotalWorkTime
        {
            get { return WorkTime.Duration + TotalBreakTime; }
        }

        public TimeSpan TotalBreakTime
        {
            get { return new TimeSpan(Breaks.Sum(d => d.Duration.Ticks)); }
        }

        /// <summary>
        /// The assigned work items of this day
        /// </summary>
        public ObservableCollection<WorkItem> AssignedWorkItems { get; }

        #endregion
    }
}
