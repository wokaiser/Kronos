using KronosData.Logic;
using KronosData.Model;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace KronosUI.Controls
{
    public class WorkItemEditorViewModel : BindableBase
    {
        private readonly DataManager dataManager;

        private bool hasChanged;
        private bool addItem;
        private TimeSpan duration;
        private WorkTask selectedTask;
        private WorkDay currentWorkDay;
        private WorkItem selecteWorkItem;
        private ObservableCollection<Account> currentAccounts;

        public WorkItemEditorViewModel(WorkDay currentDay, WorkItem selectedItem)
        {
            dataManager = ContainerLocator.Container.Resolve<DataManager>();

            Initialize();

            currentWorkDay = currentDay;
            selecteWorkItem = selectedItem;
            SelectedTask = selectedItem.AssignedWorkTask;
            Duration = selectedItem.Duration;

            hasChanged = false;
            addItem = selectedItem.Equals(WorkItem.Empty);
        }

        private void Initialize()
        {
            InitializeCommands();

            CurrentAccounts = dataManager.Accounts;
        }

        #region Commands

        private void InitializeCommands()
        {
            ItemSelectionChangedCommand = new DelegateCommand<object>(ItemSelectionChanged);
            SaveChangesCommand = new DelegateCommand<Window>(SaveChanges, CanSaveChanges);
            RevokeChangesCommand = new DelegateCommand<Window>(RevokeChanges);
        }

        private void ItemSelectionChanged(object param)
        {
            if (param is WorkTask)
            {
                SelectedTask = param as WorkTask;
            }
        }

        private void SaveChanges(Window window)
        {
            if (addItem)
            {
                currentWorkDay.AssignedWorkItems.Add(new WorkItem(Duration, SelectedTask));
            }
            else
            {
                currentWorkDay.AssignedWorkItems.First(d => d.Equals(selecteWorkItem)).Update(new WorkItem(Duration, SelectedTask));
            }

            dataManager.CurrentUser.AssignedWorkDays.First(d => d.WorkTime.DateOfWork.Date.Equals(currentWorkDay.WorkTime.DateOfWork.Date)).Update(currentWorkDay);

            window.DialogResult = true;
            window.Close();
        }

        private void RevokeChanges(Window window)
        {
            window.DialogResult = false;
            window.Close();
        }

        private bool CanSaveChanges(Window window)
        {
            return hasChanged && SelectedTaskTitle != string.Empty;
        }

        #endregion

        #region Properties

        public DelegateCommand<Window> SaveChangesCommand { get; private set; }

        public DelegateCommand<Window> RevokeChangesCommand { get; private set; }

        public ICommand ItemSelectionChangedCommand { get; private set; }

        public ObservableCollection<Account> CurrentAccounts
        {
            get { return currentAccounts; }
            set { SetProperty(ref currentAccounts, value); }
        }

        public WorkTask SelectedTask
        {
            get { return selectedTask; }
            set
            {
                if (value is WorkTask)
                {
                    hasChanged = true;
                    SetProperty(ref selectedTask, value);
                    RaisePropertyChanged(nameof(SelectedTaskTitle));
                    SaveChangesCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public TimeSpan Duration
        {
            get { return duration; }
            set
            {
                hasChanged = true;
                SetProperty(ref duration, value);
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        public string SelectedTaskTitle
        {
            get { return string.Format("{0}\n{1}", SelectedTask.AssignedAccountNumber, SelectedTask.Title); }
        }

        #endregion
    }
}
