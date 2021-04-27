using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace KronosData.Model
{
    public class WorkTask : DB_Access, INotifyPropertyChanged
    {
        private string title;
        private string assignedAccountNumber;

        /// <summary>
        /// Creates a new WorkTask object
        /// </summary>
        /// <param name="title">The title of the work task</param>
        /// <param name="assignedAccoun">The account this task is assigned to</param>
        public WorkTask(string title, Account assignedAccount)
        {
            Update(title, assignedAccount);
        }

        public void Update(string title, Account assignedAccount)
        {
            Title = title;
            AssignedAccountNumber = assignedAccount == null ? string.Empty : assignedAccount.Number;
        }

        #region IPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

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
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        /// <summary>
        /// The assigned account number
        /// </summary>
        [JsonProperty]
        public string AssignedAccountNumber
        {
            get { return assignedAccountNumber; }
            private set
            {
                assignedAccountNumber = value;
                OnPropertyChanged(nameof(AssignedAccountNumber));
            }
        }

        #endregion
    }
}
