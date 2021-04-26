using System;

namespace KronosData.Model
{
    public class WorkTask : DB_Access
    {
        /// <summary>
        /// Creates a new WorkTask object
        /// </summary>
        /// <param name="title">The title of the work task</param>
        /// <param name="assignedAccoun">The account this task is assigned to</param>
        public WorkTask(string title, Account assignedAccount)
        {
            Title = title;
            AssignedAccountNumber = assignedAccount.Number;
        }

        #region Overrides

        public override string ToString()
        {
            return Title;
        }

        public override bool Equals(object obj)
        {
            return obj is WorkTask task && Title.Equals(task.Title) && AssignedAccountNumber.Equals(task.AssignedAccountNumber);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, AssignedAccountNumber);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The title of the work task
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The assigned account number
        /// </summary>
        public string AssignedAccountNumber { get; private set; }

        #endregion
    }
}
