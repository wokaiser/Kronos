using System;

namespace KronosData.Model
{
    public class WorkTask : DB_Access
    {
        /// <summary>
        /// Creates a new WorkTask object
        /// </summary>
        /// <param name="assignedAccount">The account to where this object is assigned to</param>
        public WorkTask(string title)
        {
            Title = title;
        }

        #region Overrides

        public override string ToString()
        {
            return Title;
        }

        public override bool Equals(object obj)
        {
            return obj is WorkTask task && Title.Equals(task.Title);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The title of the work task
        /// </summary>
        public string Title { get; set; }

        #endregion
    }
}
