using KronosData.Logic;
using KronosData.Model;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KronosUI.Controls
{
    public class WorkDayEditorViewModel : BindableBase
    {
        private string title;
        private bool pendingChanges = false;
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
            CurrentDay.WorkTime.Begin = new TimeSpan(9, 0, 0);
            CurrentDay.WorkTime.End = new TimeSpan(17, 0, 0);
            CurrentDay.BreakTime = new TimeSpan(0, 45, 0);
        }

        #region Command implementations

        public void InitializeCommands()
        {
            SaveChangesCommand = new DelegateCommand<Window>(SaveChanges, CanSaveChanges);
            RevokeChangesCommand = new DelegateCommand<Window>(RevokeChanges);
            AddWorkItemCommand = new DelegateCommand(AddWorkItem);
            RemoveWorkItemCommand = new DelegateCommand(RemoveWorkItem, CanRemoveWorkItem);
        }

        public void SaveChanges(Window window)
        {
            //TODO: Do save changes

            PendingChanges = false;
            window.DialogResult = true;
            window.Close();
        }

        public bool CanSaveChanges(Window window)
        {
            return PendingChanges;
        }

        public void RevokeChanges(Window window)
        {
            PendingChanges = false;
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

        public bool PendingChanges
        {
            get { return pendingChanges; }
            set
            {
                SetProperty(ref pendingChanges, value);
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        public TimeSpan BeginOfDay
        {
            get { return CurrentDay.WorkTime.Begin; }
            set
            {
                CurrentDay.WorkTime.Begin = value;
                RaisePropertyChanged(nameof(BeginOfDay));
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
                SaveChangesCommand.RaiseCanExecuteChanged();
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
