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
using System.Windows.Controls;

namespace KronosUI.Controls
{
    public class WorkDayEditorViewModel : BindableBase
    {
        private enum WorkdayTypes { Office = 0, Mobile, Vacation, Sick };

        private readonly DataManager dataManager;

        private WorkDay currentDay;
        private WorkItem selectedWorkItem;
        private ObservableCollection<WorkItem> workItems;
        private string title;
        private WorkdayTypes previousSelected;
        private bool hasChanged;
        private bool allowTimeSelection;

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
            previousSelected = WorkdayTypes.Office;

            hasChanged = false;
            allowTimeSelection = true;

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
                WorkItems = CurrentDay.AssignedWorkItems;

                return;
            }

            CurrentDay.WorkTime.Begin = dataManager.CurrentUser.UserSettings.DefaultBeginOfWork;
            CurrentDay.WorkTime.End = dataManager.CurrentUser.UserSettings.DefaultEndOfWork;
            CurrentDay.BreakTime = dataManager.CurrentUser.UserSettings.DefaultBreakTime;
            WorkItems = CurrentDay.AssignedWorkItems;

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
            RaisePropertyChanged(nameof(BeginOfDay));
            RaisePropertyChanged(nameof(EndOfDay));
            RaisePropertyChanged(nameof(BreakTime));
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
            RadioButtonSelectedCommand = new DelegateCommand<object>(RadioButtonSelectedCommandExecute);
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
            window.DialogResult = false;
            window.Close();
        }

        private void AddWorkItem()
        {
            WorkItem newItem;

            if (!WorkItemEditor.AddWorkItem(out newItem))
            {
                return;
            }

            var existing = workItems.Where(w => w.AssignedWorkTask.Equals(newItem.AssignedWorkTask)).FirstOrDefault();
            if (existing == null)
            {
                WorkItems.Add(newItem);
            }
            else
            {
                existing.Duration += newItem.Duration;
            }

            RaisePropertiesChanged();
        }

        private void EditWorkItem()
        {
            var changedItem = SelectedWorkItem;

            if (WorkItemEditor.EditWorkItem(ref changedItem))
            {
                SelectedWorkItem.Update(changedItem);
                RaisePropertyChanged(nameof(SelectedWorkItem));
                RaisePropertiesChanged();
            }
        }

        private bool CanEditWorkItem()
        {
            return SelectedWorkItem != null;
        }

        private void RemoveWorkItem()
        {
            if ((bool)PictoMsgBox.ShowMessage("Arbeitspaket löschen", "Sind Sie sich sicher das gewählte Arbeitspaket zu entfernen?", PictoMsgBoxButton.YesNo))
            {
                CurrentDay.AssignedWorkItems.Remove(SelectedWorkItem);
                RaisePropertiesChanged();
            }
        }

        private bool CanRemoveWorkItem()
        {
            return SelectedWorkItem != null;
        }

        private void RadioButtonSelectedCommandExecute(object sender)
        {
            var workType = (WorkdayTypes)Enum.Parse(typeof(WorkdayTypes), (sender as RadioButton).Name);

            switch(workType)
            {
                case WorkdayTypes.Office:
                case WorkdayTypes.Mobile:
                    AllowTimeSelection = true;
                    if (previousSelected == WorkdayTypes.Vacation || previousSelected == WorkdayTypes.Sick)
                    {
                        BeginOfDay = dataManager.CurrentUser.UserSettings.DefaultBeginOfWork;
                        EndOfDay = dataManager.CurrentUser.UserSettings.DefaultEndOfWork;
                        BreakTime = dataManager.CurrentUser.UserSettings.DefaultBreakTime;
                        DailyWorkTime = dataManager.CurrentUser.UserSettings.DefaultDailyWorkTime;
                    }
                    previousSelected = workType;
                    break;

                case WorkdayTypes.Vacation:
                case WorkdayTypes.Sick:
                    AllowTimeSelection = false;
                    BeginOfDay = TimeSpan.Zero;
                    EndOfDay = TimeSpan.Zero;
                    BreakTime = TimeSpan.Zero;
                    DailyWorkTime = TimeSpan.Zero;
                    previousSelected = workType;
                    break;

                default:
                    return;
            }
        }

        #endregion

        #region Properties

        public DelegateCommand<Window> SaveChangesCommand { get; private set; }

        public DelegateCommand<Window> RevokeChangesCommand { get; private set; }

        public DelegateCommand AddWorkItemCommand { get; private set; }

        public DelegateCommand EditWorkItemCommand { get; private set; }

        public DelegateCommand RemoveWorkItemCommand { get; private set; }

        public DelegateCommand<object> RadioButtonSelectedCommand { get; private set; }

        public WorkDay CurrentDay
        {
            get { return currentDay; }
            set { SetProperty(ref currentDay, value); }
        }

        public ObservableCollection<WorkItem> WorkItems
        {
            get { return workItems; }
            set { SetProperty(ref workItems, value); }
        }

        public WorkItem SelectedWorkItem
        {
            get { return selectedWorkItem; }
            set 
            {
                SetProperty(ref selectedWorkItem, value);
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

        public bool IsSickDay
        {
            get { return CurrentDay.IsSickDay; }
            set
            {
                CurrentDay.IsSickDay = value;
                RaisePropertiesChanged();
            }
        }

        public bool IsFreeDay
        {
            get { return CurrentDay.IsFreeDay;}
            set
            {
                CurrentDay.IsFreeDay = value;
                RaisePropertiesChanged();
            }
        }

        public bool AllowTimeSelection
        {
            get { return allowTimeSelection; }
            set { SetProperty(ref allowTimeSelection, value); }
        }

        public string TotalWorkHours
        {
            get
            {
                var wHours = EndOfDay - BeginOfDay - BreakTime;
                return string.Format(" {0} h", wHours.ToString(@"hh\:mm"));
            }
        }

        public string TotalOvertime
        {
            get
            {
                var prefix = CurrentDay.TotalOverTime < TimeSpan.Zero ? "-" : " ";
                return string.Format("{0}{1} h", prefix, CurrentDay.TotalOverTime.ToString(@"hh\:mm"));
            }
        }

        public string UnaccountedHours
        {
            get
            {
                var uaHours = EndOfDay - BeginOfDay - BreakTime - GetAccountedTime();
                var retVal = string.Format("{0} h", uaHours.ToString(@"hh\:mm"));
                return uaHours < TimeSpan.Zero ? "-" + retVal : " " + retVal;
            }
        }

        public string ZeroHours
        {
            get
            {
                return " 00:00 h";
            }
        }

        #endregion
    }
}
