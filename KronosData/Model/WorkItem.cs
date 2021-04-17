using System;

namespace KronosData.Model
{
    public class WorkItem
    {
        /// <summary>
        /// Creats a new work item object
        /// </summary>
        /// <param name="begin">The datetime the user began with the work</param>
        /// <param name="end">The datetime the user ended with the work</param>
        /// <param name="assignedWorkTask">The work task being worked on</param>
        public WorkItem(DateTime begin, DateTime end, WorkTask assignedWorkTask)
        {
            Begin = begin;
            End = end;
            AssignedWorkTask = assignedWorkTask;
        }

        #region Overrides

        public override string ToString()
        {
            return string.Format("{0} - {1}: {2}", Begin, End, AssignedWorkTask);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The datetime where the work started
        /// </summary>
        public DateTime Begin { get; }

        /// <summary>
        /// The datetemime where the work ended 
        /// </summary>
        public DateTime End { get; }

        /// <summary>
        /// The duration of the work item
        /// </summary>
        public TimeSpan Duration
        {
            get { return End - Begin; }
        }

        /// <summary>
        /// The assigned work task
        /// </summary>
        public WorkTask AssignedWorkTask { get; }

        #endregion
    }
}
