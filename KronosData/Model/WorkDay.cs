﻿using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace KronosData.Model
{
    public class WorkDay
    {
        public WorkDay()
        {
            WorkTime = new DateUnit();
            BreakTime = new TimeSpan(0);
            IsMobileDay = false;
            AssignedWorkItems = new ObservableCollection<WorkItem>();
            DailyWorkTime = new TimeSpan(7, 0, 0);
        }

        public WorkDay(DateTime currentDate)
        {
            WorkTime = new DateUnit(currentDate);
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
            WorkTime.Date = update.WorkTime.Date;
            BreakTime = update.BreakTime;
            IsMobileDay = update.IsMobileDay;
            DailyWorkTime = update.DailyWorkTime;

            AssignedWorkItems.Clear();
            foreach(var item in update.AssignedWorkItems)
            {
                AssignedWorkItems.Add(item);
            }
        }

        #region Properties

        /// <summary>
        /// The worktime of the day
        /// </summary>
        public DateUnit WorkTime { get; set; }

        /// <summary>
        /// The sum of breaks taken this day
        /// </summary>
        public TimeSpan BreakTime { get; set; }

        /// <summary>
        /// Whether the day is spend mobile
        /// </summary>
        public bool IsMobileDay { get; set; }

        /// <summary>
        /// The total time of work to be done at a day
        /// </summary>
        public TimeSpan DailyWorkTime { get; set; }

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
                    var overTime = TotalWorkTime - DailyWorkTime;
                    return overTime < TimeSpan.Zero ? new TimeSpan(0) : overTime;
                }

                return new TimeSpan(0);
            }
        }

        /// <summary>
        /// The assigned work items of this day
        /// </summary>
        public ObservableCollection<WorkItem> AssignedWorkItems { get; }

        #endregion
    }
}
