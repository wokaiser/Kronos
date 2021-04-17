namespace KronosData.Model
{
    public class WorkTask : DB_Access
    {
        /// <summary>
        /// Creates a new WorkTask object
        /// </summary>
        /// <param name="assignedAccount">The account to where this object is assigned to</param>
        public WorkTask(Account assignedAccount)
        {
            Title = string.Empty;
            AssignedAccount = assignedAccount;
        }

        #region Overrides

        public override string ToString()
        {
            return Title;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The title of the work task
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The account to where this task is assigned to
        /// </summary>
        public Account AssignedAccount { get; }

        #endregion
    }
}
