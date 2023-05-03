using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace KronosData.Model
{
    public class WorkTask : INotifyPropertyChanged
    {
        private string title;
        private string mappingId;
        private string assignedAccountNumber;

        /// <summary>
        /// Creates a new WorkTask object
        /// </summary>
        /// <param name="title">The title of the work task</param>
        /// <param name="assignedAccoun">The account this task is assigned to</param>
        public WorkTask(string title, Account assignedAccount, string mappingId)
        {
            Update(title, assignedAccount, mappingId);
        }

        public void Update(string title, Account assignedAccount, string mappingId)
        {
            Title = title;
            AssignedAccountNumber = assignedAccount == null ? string.Empty : assignedAccount.Number;
            MappingID = mappingId;
        }

        public void Update(string title, string accountNumber, string mappingId)
        {
            Title = title;
            AssignedAccountNumber = accountNumber;
            MappingID = mappingId;
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
            return $"{MappingID} - {Title}";
        }

        public override bool Equals(object obj)
        {
            return obj is WorkTask task && Title.Equals(task.Title) && AssignedAccountNumber.Equals(task.AssignedAccountNumber) && MappingID.Equals(task.MappingID);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, AssignedAccountNumber, MappingID);
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
        /// The assigned mapping ID
        /// </summary>
        public string MappingID
        {
            get { return mappingId; }
            set
            {
                mappingId = value;
                OnPropertyChanged(nameof(MappingID));
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
