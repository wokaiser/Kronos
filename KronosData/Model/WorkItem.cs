using System;

namespace KronosData.Model
{
    public class WorkItem
    {
        /// <summary>
        /// Creats a new work item object
        /// </summary>
        /// <param name="duration">The duration of the work task</param>
        /// <param name="assignedWorkTask">The work task being worked on</param>
        public WorkItem(TimeSpan duration, WorkTask assignedWorkTask)
        {
            Duration = duration;
            AssignedWorkTask = assignedWorkTask;
        }

        /// <summary>
        /// Returns an empty WorkItem
        /// </summary>
        public static WorkItem Empty
        {
            get
            {
                return new WorkItem(TimeSpan.Zero, new WorkTask(string.Empty, new Account(string.Empty)));
            }
        }

        #region Overrides

        public override string ToString()
        {
            return Duration.ToString(@"hh\:mm");
        }

        public override bool Equals(object obj)
        {
            return obj is WorkItem item && Duration.Equals(item.Duration) && AssignedWorkTask.Equals(item.AssignedWorkTask);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Duration, AssignedWorkTask);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The duration of the work item
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// The assigned work task
        /// </summary>
        public WorkTask AssignedWorkTask { get; }

        #endregion
    }
}
