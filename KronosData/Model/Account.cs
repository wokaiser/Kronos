using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace KronosData.Model
{
    public class Account : INotifyPropertyChanged
    {
        private string number;
        private string title;
        private ObservableCollection<WorkTask> assignedTasks;

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
            if (!Number.Equals(number))
            {
                Number = number;

                foreach (var item in AssignedTasks)
                {
                    item.Update(item.Title, this, item.MappingID);
                }
            }

            Title = title;
        }

        #region INotifyPropertyChanged implementation

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
        public string Number
        {
            get { return number; }
            private set
            {
                number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

        /// <summary>
        /// The title of the account
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
        /// A collection of the tasks assigned to this account
        /// </summary>
        public ObservableCollection<WorkTask> AssignedTasks
        {
            get { return assignedTasks; }
            set
            {
                assignedTasks = value;
                OnPropertyChanged(nameof(AssignedTasks));
            }
        }

        #endregion
    }
}
