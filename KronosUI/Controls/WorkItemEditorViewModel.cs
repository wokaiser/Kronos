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
        private WorkTask selectedItem;
        private WorkItem currentWorkItem;
        private ObservableCollection<Account> currentAccounts;

        public WorkItemEditorViewModel(WorkItem selectedItem)
        {
            dataManager = ContainerLocator.Container.Resolve<DataManager>();

            Initialize();
            
            CurrentWorkItem = selectedItem;
            SelectedItem = CurrentWorkItem.AssignedWorkTask;

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
                SelectedItem = param as WorkTask;
            }
        }

        private void SaveChanges(Window window)
        {
            //TODO: Do save changes

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
            return hasChanged;
        }

        #endregion

        #region Properties

        public DelegateCommand<Window> SaveChangesCommand { get; private set; }

        public DelegateCommand<Window> RevokeChangesCommand { get; private set; }

        public ICommand ItemSelectionChangedCommand { get; private set; }

        public WorkItem CurrentWorkItem
        {
            get { return currentWorkItem; }
            set { SetProperty(ref currentWorkItem, value); }
        }

        public ObservableCollection<Account> CurrentAccounts
        {
            get { return currentAccounts; }
            set { SetProperty(ref currentAccounts, value); }
        }

        public WorkTask SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (value is WorkTask)
                {
                    hasChanged = true;
                    SetProperty(ref selectedItem, value);
                    RaisePropertyChanged(nameof(SelectedWorkTask));
                    SaveChangesCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string SelectedWorkTask
        {
            get { return string.Format("{0}\n{1}", SelectedItem.AssignedAccountNumber, SelectedItem.Title); }
        }

        public TimeSpan Duration
        {
            get { return CurrentWorkItem.Duration; }
            set
            {
                hasChanged = true;
                CurrentWorkItem.Duration = value;
                RaisePropertyChanged(nameof(CurrentWorkItem));
                RaisePropertyChanged(nameof(Duration));
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion
    }
}
