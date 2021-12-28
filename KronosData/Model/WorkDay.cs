﻿using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace KronosData.Model
{
    public class WorkDay
    {
        public WorkDay(bool isMobileDay)
        {
            WorkTime = new DateUnit();
            BreakTime = new TimeSpan(0);
            IsMobileDay = isMobileDay;
            AssignedWorkItems = new ObservableCollection<WorkItem>();
            DailyWorkTime = new TimeSpan(7, 0, 0);
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
