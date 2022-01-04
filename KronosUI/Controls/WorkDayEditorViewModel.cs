using KronosData.Logic;
using KronosData.Model;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace KronosUI.Controls
{
    public class WorkDayEditorViewModel : BindableBase
    {
        private string title;
        private WorkDay currentDay;
        private WorkItem currentWorkItem;
        private readonly DataManager dataManager;
        private bool hasChanged;

        public WorkDayEditorViewModel(WorkDay selectedItem)
        {
            dataManager = ContainerLocator.Container.Resolve<DataManager>();

            InitializeEditor(selectedItem);
        }

        private void InitializeEditor(WorkDay selectedItem)
        {
            Title = string.Format("{0}, den {1} bearbeiten",
                CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(selectedItem.WorkTime.DateOfWork.DayOfWeek),
                selectedItem.WorkTime.DateOfWork.Date.ToShortDateString());

            hasChanged = false;

            SetupCurrentWorkDay(selectedItem);

            InitializeCommands();
        }

        private void SetupCurrentWorkDay(WorkDay workDay)
        {
            CurrentDay = new WorkDay(workDay.WorkTime.DateOfWork);
            var tmp = dataManager.CurrentUser.AssignedWorkDays.FirstOrDefault(d => d.WorkTime.DateOfWork.Date.Equals(workDay.WorkTime.DateOfWork.Date));

            if (tmp != null)
            {
                CurrentDay.Update(tmp);

                return;
            }

            CurrentDay.WorkTime.Begin = dataManager.CurrentUser.UserSettings.DefaultBeginOfWork;
            CurrentDay.WorkTime.End = dataManager.CurrentUser.UserSettings.DefaultEndOfWork;
            CurrentDay.BreakTime = dataManager.CurrentUser.UserSettings.DefaultBreakTime;

            hasChanged = true;
        }

        private TimeSpan GetAccountedTime()
        {
            var retVal = new TimeSpan(0);

            foreach (var wItem in CurrentDay.AssignedWorkItems)
            {
                retVal += wItem.Duration;
            }

            return retVal;
        }

        private void RaisePropertiesChanged()
        {
            hasChanged = true;
            RaisePropertyChanged(nameof(DailyWorkTime));
            RaisePropertyChanged(nameof(TotalWorkHours));
            RaisePropertyChanged(nameof(TotalOvertime));
            RaisePropertyChanged(nameof(UnaccountedHours));
            RaisePropertyChanged(nameof(WorkItems));
            RaisePropertyChanged(nameof(IsMobileOffice));
            SaveChangesCommand.RaiseCanExecuteChanged();
            EditWorkItemCommand.RaiseCanExecuteChanged();
        }

        #region Command implementations

        public void InitializeCommands()
        {
            SaveChangesCommand = new DelegateCommand<Window>(SaveChanges, CanSaveChanges);
            RevokeChangesCommand = new DelegateCommand<Window>(RevokeChanges);
            AddWorkItemCommand = new DelegateCommand(AddWorkItem);
            EditWorkItemCommand = new DelegateCommand(EditWorkItem, CanEditWorkItem);
            RemoveWorkItemCommand = new DelegateCommand(RemoveWorkItem, CanRemoveWorkItem);
        }

        private void SaveChanges(Window window)
        {
            var tmp = dataManager.CurrentUser.AssignedWorkDays.FirstOrDefault(d => d.WorkTime.DateOfWork.Date.Equals(CurrentDay.WorkTime.DateOfWork.Date));

            if (tmp != null)
            {
                tmp.Update(CurrentDay);
            }
            else
            {
                dataManager.CurrentUser.AssignedWorkDays.Add(CurrentDay);
            }

            window.DialogResult = true;
            window.Close();
        }

        private bool CanSaveChanges(Window windowd)
        {
            return hasChanged;
        }

        private void RevokeChanges(Window window)
        {
            hasChanged = false;
            window.DialogResult = false;
            window.Close();
        }

        private void AddWorkItem()
        {
            if (WorkItemEditor.AddWorkItem(CurrentDay))
            {
                InitializeEditor(CurrentDay);
                RaisePropertiesChanged();
            }
        }

        private void EditWorkItem()
        {
            if (WorkItemEditor.EditWorkItem(CurrentDay, CurrentWorkItem))
            {
                InitializeEditor(CurrentDay);
                RaisePropertiesChanged();
            }
        }

        private bool CanEditWorkItem()
        {
            return CurrentWorkItem != null;
        }

        private void RemoveWorkItem()
        {
            if ((bool)PictoMsgBox.ShowMessage("Remove WorkItem", "Are you sure to remove the selected work item?", PictoMsgBoxButton.YesNo))
            {
                CurrentDay.AssignedWorkItems.Remove(CurrentWorkItem);
                RaisePropertiesChanged();
            }
        }

        private bool CanRemoveWorkItem()
        {
            return CurrentWorkItem != null;
        }

        #endregion

        #region Properties

        public DelegateCommand<Window> SaveChangesCommand { get; private set; }

        public DelegateCommand<Window> RevokeChangesCommand { get; private set; }

        public DelegateCommand AddWorkItemCommand { get; private set; }

        public DelegateCommand EditWorkItemCommand { get; private set; }

        public DelegateCommand RemoveWorkItemCommand { get; private set; }

        public WorkDay CurrentDay
        {
            get { return currentDay; }
            set { SetProperty(ref currentDay, value); }
        }

        public WorkItem CurrentWorkItem
        {
            get { return currentWorkItem; }
            set 
            {
                SetProperty(ref currentWorkItem, value);
                RemoveWorkItemCommand.RaiseCanExecuteChanged();
                EditWorkItemCommand.RaiseCanExecuteChanged();
            }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public TimeSpan BeginOfDay
        {
            get { return CurrentDay.WorkTime.Begin; }
            set
            {
                CurrentDay.WorkTime.Begin = value;
                RaisePropertiesChanged();
            }
        }

        public TimeSpan EndOfDay
        {
            get { return CurrentDay.WorkTime.End; }
            set
            {
                CurrentDay.WorkTime.End = value;
                RaisePropertiesChanged();
            }
        }

        public TimeSpan BreakTime
        {
            get { return CurrentDay.BreakTime; }
            set
            {
                CurrentDay.BreakTime = value;
                RaisePropertiesChanged();
            }
        }

        public TimeSpan DailyWorkTime
        {
            get { return CurrentDay.DailyWorkTime; }
            set
            {
                CurrentDay.DailyWorkTime = value;
                RaisePropertiesChanged();
            }
        }

        public bool IsMobileOffice
        {
            get { return CurrentDay.IsMobileDay; }
            set
            {
                CurrentDay.IsMobileDay = value;
                RaisePropertiesChanged();
            }
        }

        public string TotalWorkHours
        {
            get
            {
                var wHours = EndOfDay - BeginOfDay - BreakTime;
                return string.Format("{0} h", wHours.ToString(@"hh\:mm"));
            }
        }

        public string TotalOvertime
        {
            get
            {
                return string.Format("{0} h", CurrentDay.TotalOverTime.ToString(@"hh\:mm"));
            }
        }

        public string UnaccountedHours
        {
            get
            {
                var uaHours = EndOfDay - BeginOfDay - BreakTime - GetAccountedTime();
                var retVal = string.Format("{0} h", uaHours.ToString(@"hh\:mm"));
                return uaHours < TimeSpan.Zero ? "-" + retVal : retVal;
            }
        }

        public ObservableCollection<WorkItem> WorkItems
        {
            get
            {
                return CurrentDay.AssignedWorkItems;
            }
        }

        #endregion
    }
}
