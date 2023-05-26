using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json.Serialization;

namespace KronosData.Model
{
    public class WorkDay
    {
        public WorkDay()
        {
            WorkTime = new WorkDate();
            BreakTime = new TimeSpan(0);
            IsMobileDay = false;
            AssignedWorkItems = new ObservableCollection<WorkItem>();
            DailyWorkTime = new TimeSpan(7, 0, 0);
        }

        public WorkDay(DateTime currentDate)
        {
            WorkTime = new WorkDate(currentDate);
            BreakTime = new TimeSpan(0);
            IsMobileDay = false;
            AssignedWorkItems = new ObservableCollection<WorkItem>();
            DailyWorkTime = new TimeSpan(7, 0, 0);
        }

        /// <summary>
        /// Updates this workday with the info of another one
        /// </summary>
        /// <param name="update">The workday to update from</param>
        public void Update(WorkDay update)
        {
            WorkTime.Begin = update.WorkTime.Begin;
            WorkTime.End = update.WorkTime.End;
            WorkTime.DateOfWork = update.WorkTime.DateOfWork;
            BreakTime = update.BreakTime;
            IsMobileDay = update.IsMobileDay;
            IsSickDay = update.IsSickDay;
            IsFreeDay = update.IsFreeDay;
            DailyWorkTime = update.DailyWorkTime;

            AssignedWorkItems.Clear();
            foreach(var item in update.AssignedWorkItems)
            {
                AssignedWorkItems.Add(item);
            }
        }

        public override string ToString()
        {
            return $"{WorkTime.DateOfWork.ToShortDateString()} - {TotalWorkTime}";
        }

        #region Properties

        /// <summary>
        /// The worktime of the day
        /// </summary>
        public WorkDate WorkTime { get; set; }

        /// <summary>
        /// The sum of breaks taken this day
        /// </summary>
        public TimeSpan BreakTime { get; set; }

        /// <summary>
        /// Whether the day is spend mobile
        /// </summary>
        public bool IsMobileDay { get; set; }

        /// <summary>
        /// Whether the day is spend sick
        /// </summary>
        public bool IsSickDay { get; set; }

        /// <summary>
        /// Whether the day is free
        /// </summary>
        public bool IsFreeDay { get; set; }

        /// <summary>
        /// The total time of work to be done at a day
        /// </summary>
        public TimeSpan DailyWorkTime { get; set; }

        /// <summary>
        /// The assigned work items of this day
        /// </summary>
        public ObservableCollection<WorkItem> AssignedWorkItems { get; }

        /// <summary>
        /// Returns the total time of work
        /// </summary>
        [JsonIgnore]
        public TimeSpan TotalWorkTime
        {
            get { return WorkTime.Duration - BreakTime; }
        }

        /// <summary>
        /// Returns the total overtime
        /// </summary>
        [JsonIgnore]
        public TimeSpan TotalOverTime
        {
            get
            {
                if (TotalWorkTime > TimeSpan.Zero)
                {
                    return TotalWorkTime - DailyWorkTime;
                }

                return new TimeSpan(0);
            }
        }

        /// <summary>
        /// Returns the total accounted time for the day
        /// </summary>
        [JsonIgnore]
        public TimeSpan TotalAccountedTime
        {
            get
            {
                var retVal = TimeSpan.Zero;

                foreach (var item in AssignedWorkItems)
                {
                    retVal = retVal.Add(item.Duration);
                }

                return retVal;
            }
        }

        /// <summary>
        /// Returns true, whether the day is on a weekend
        /// </summary>
        [JsonIgnore]
        public bool IsWeekend
        {
            get
            {
                return WorkTime.DateOfWork.DayOfWeek == DayOfWeek.Saturday || WorkTime.DateOfWork.DayOfWeek == DayOfWeek.Sunday;
            }
        }

        /// <summary>
        /// Returns a tooltip value to show in listings
        /// </summary>
        [JsonIgnore]
        public string ToolTipText
        {
            get
            {
                var sb = new StringBuilder();

                foreach (var item in AssignedWorkItems)
                {
                    sb.AppendFormat("{0,4}[{1:00.00}h] - {2}\n", item.AssignedWorkTask.MappingID, item.Duration.TotalHours, item.AssignedWorkTask.Title);
                }

                if (sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1); // Remove trailing line break
                }
                else
                {
                    if (IsSickDay || IsFreeDay)
                    {
                        return IsSickDay ? "Krank" : "Urlaub";
                    }

                    return "Keine Eintragungen vorhanden";
                }

                return sb.ToString();
            }
        }

        #endregion
    }
}
