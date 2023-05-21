using KronosData.Logic;
using KronosData.Model;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace KronosUI.Controls
{
    public class WorkItemEditorViewModel : BindableBase
    {
        private readonly DataManager dataManager;

        private bool hasChanged;
        private TimeSpan duration;
        private WorkItem selectedItem;
        private WorkTask selectedTask;
        private ObservableCollection<Account> currentAccounts;

        public WorkItemEditorViewModel(ref WorkItem selectedItem)
        {
            dataManager = ContainerLocator.Container.Resolve<DataManager>();

            Initialize();

            this.selectedItem = selectedItem;
            SelectedTask = selectedItem.AssignedWorkTask;
            Duration = selectedItem.Duration;

            hasChanged = false;
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
            selectedItem.Update(new WorkItem(Duration, SelectedTask));

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
            return hasChanged && !string.IsNullOrWhiteSpace(SelectedTaskTitle) && Duration > TimeSpan.Zero;
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
            get { return string.Format($"{SelectedTask.MappingID}\n{SelectedTask.Title}"); }
        }

        #endregion
    }
}
