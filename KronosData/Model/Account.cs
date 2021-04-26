using System;
using System.Collections.ObjectModel;

namespace KronosData.Model
{
    public class Account : DB_Access
    {
        /// <summary>
        /// Creates a new Account object
        /// </summary>
        /// <param name="number">The account number</param>
        public Account(string number)
        {
            Number = number;
            Title = string.Empty;
            AssignedTasks = new ObservableCollection<WorkTask>();
        }

        public void Update(string number, string title)
        {
            Number = number;
            Title = title;
        }

        #region Overrides

        public override string ToString()
        {
            return string.Format("{0}: \"{1}\"", Number, Title);
        }

        public override bool Equals(object obj)
        {
            return obj is Account account && Number.Equals(account.Number);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Number);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The account number
        /// </summary>
        public string Number { get; private set; }

        /// <summary>
        /// The title of the account
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A collection of the tasks assigned to this account
        /// </summary>
        public ObservableCollection<WorkTask> AssignedTasks { get; }

        #endregion
    }
}
