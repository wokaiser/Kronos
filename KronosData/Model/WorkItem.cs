using System;
using System.ComponentModel;

namespace KronosData.Model
{
    public class WorkItem : INotifyPropertyChanged
    {
        private TimeSpan duration;
        private WorkTask assignedWorkTask;

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
        /// Updates this work item with the info of another one
        /// </summary>
        /// <param name="update">The work item to update from</param>
        public void Update(WorkItem update)
        {
            Duration = update.Duration;
            AssignedWorkTask = update.AssignedWorkTask;
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
            return Duration.ToString(@"hh\:mm");
        }

        /*public override bool Equals(object obj)
        {
            return obj is WorkItem item && Duration.Equals(item.Duration) && AssignedWorkTask.Equals(item.AssignedWorkTask);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Duration, AssignedWorkTask);
        }*/

        #endregion

        #region Properties

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

        /// <summary>
        /// The duration of the work item
        /// </summary>
        public TimeSpan Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }

        /// <summary>
        /// The assigned work task
        /// </summary>
        public WorkTask AssignedWorkTask
        {
            get { return assignedWorkTask; }
            private set
            {
                assignedWorkTask = value;
                OnPropertyChanged(nameof(AssignedWorkTask));
            }
        }

        #endregion
    }
}
