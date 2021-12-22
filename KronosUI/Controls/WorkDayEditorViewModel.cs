using KronosData.Logic;
using KronosData.Model;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace KronosUI.Controls
{
    public class WorkDayEditorViewModel : BindableBase
    {
        private string title;
        private WorkDay currentDay;
        private readonly DataManager dataManager;

        public WorkDayEditorViewModel(WorkDay selectedItem)
        {
            dataManager = ContainerLocator.Container.Resolve<DataManager>();

            InitializeCommands();
            InitializeEditor(selectedItem);
        }

        private void InitializeEditor(WorkDay selectedItem)
        {
            Title = string.Format("{0}, den {1} bearbeiten",
                CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(selectedItem.WorkTime.Date.DayOfWeek),
                selectedItem.WorkTime.Date.Date.ToShortDateString());

            SetupCurrentWorkDay(selectedItem);
        }

        private void SetupCurrentWorkDay(WorkDay workDay)
        {
            var tmp = dataManager.CurrentUser.AssignedWorkDays.FirstOrDefault(d => d.WorkTime.Date.Date.Equals(workDay.WorkTime.Date.Date));

            if (tmp != null)
            {
                CurrentDay = tmp;

                return;
            }

            CurrentDay = new WorkDay(WorkDay.ShiftTypeEnum.None, WorkDay.DayTypeEnum.Default);
            CurrentDay.WorkTime.Begin = dataManager.CurrentUser.UserSettings.DefaultBeginOfWork;
            CurrentDay.WorkTime.End = dataManager.CurrentUser.UserSettings.DefaultEndOfWork;
            CurrentDay.BreakTime = new TimeSpan(0, 45, 0);
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

        #region Command implementations

        public void InitializeCommands()
        {
            SaveChangesCommand = new DelegateCommand<Window>(SaveChanges);
            RevokeChangesCommand = new DelegateCommand<Window>(RevokeChanges);
            AddWorkItemCommand = new DelegateCommand(AddWorkItem);
            RemoveWorkItemCommand = new DelegateCommand(RemoveWorkItem, CanRemoveWorkItem);
        }

        public void SaveChanges(Window window)
        {
            //TODO: Do save changes

            window.DialogResult = true;
            window.Close();
        }

        public void RevokeChanges(Window window)
        {
            window.DialogResult = false;
            window.Close();
        }

        public void AddWorkItem()
        {

        }

        public void RemoveWorkItem()
        {

        }

        public bool CanRemoveWorkItem()
        {
            return false;
        }

        #endregion

        #region Properties

        public DelegateCommand<Window> SaveChangesCommand { get; private set; }

        public DelegateCommand<Window> RevokeChangesCommand { get; private set; }

        public DelegateCommand AddWorkItemCommand { get; private set; }

        public DelegateCommand RemoveWorkItemCommand { get; private set; }

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
                RaisePropertyChanged(nameof(BeginOfDay));
                RaisePropertyChanged(nameof(TotalWorkHours));
                RaisePropertyChanged(nameof(UnaccountedHours));
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        public TimeSpan EndOfDay
        {
            get { return CurrentDay.WorkTime.End; }
            set
            {
                CurrentDay.WorkTime.End = value;
                RaisePropertyChanged(nameof(EndOfDay));
                RaisePropertyChanged(nameof(TotalWorkHours));
                RaisePropertyChanged(nameof(UnaccountedHours));
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        public TimeSpan BreakTime
        {
            get { return CurrentDay.BreakTime; }
            set
            {
                CurrentDay.BreakTime = value;
                RaisePropertyChanged(nameof(BreakTime));
                RaisePropertyChanged(nameof(TotalWorkHours));
                RaisePropertyChanged(nameof(UnaccountedHours));
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        public TimeSpan DailyWorkTime
        {
            get { return CurrentDay.DailyWorkTime; }
            set
            {
                CurrentDay.DailyWorkTime = value;
                RaisePropertyChanged(nameof(DailyWorkTime));
                RaisePropertyChanged(nameof(TotalWorkHours));
                RaisePropertyChanged(nameof(UnaccountedHours));
                SaveChangesCommand.RaiseCanExecuteChanged();
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

        public string UnaccountedHours
        {
            get
            {
                var uaHours = EndOfDay - BeginOfDay - BreakTime - GetAccountedTime();
                var retVal = string.Format("{0} h", uaHours.ToString(@"hh\:mm"));
                return uaHours < TimeSpan.Zero ? "-" + retVal : retVal;
            }
        }

        public WorkDay CurrentDay
        {
            get { return currentDay; }
            set { SetProperty(ref currentDay, value); }
        }

        #endregion
    }
}
