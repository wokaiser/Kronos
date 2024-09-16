using KronosData.Logic;
using KronosData.Model;
using KronosUI.Controls;
using KronosUI.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace KronosUI.ViewModels
{
    public class ConfigurationViewModel : BindableBase, INavigationAware
    {
        private ObservableCollection<Account> currentAccounts;
        private object selectedItem;
        private bool pendingChanges;
        private Settings userSettings;

        private readonly DataManager dataManger;

        public ConfigurationViewModel()
        {
            InitializeCommands();

            dataManger = ContainerLocator.Container.Resolve<DataManager>();

            InitializeProperties();
        }

        private void InitializeProperties()
        {
            CurrentAccounts = dataManger.Accounts;
            userSettings = dataManger.CurrentUser.UserSettings;

            SelectedItem = null;
            PendingChanges = false;

            RaisePropertyChanged(nameof(DefaultBeginOfWork));
            RaisePropertyChanged(nameof(DefaultEndOfWork));
            RaisePropertyChanged(nameof(DefaultDailyWorkTime));
            RaisePropertyChanged(nameof(DefaultBreakTime));
            RaisePropertyChanged(nameof(MappingToken));
            RaisePropertyChanged(nameof(MappingUrl));
        }

        private void PublishStatusMessage(string message)
        {
            ContainerLocator.Container.Resolve<IEventAggregator>().GetEvent<UpdateStatusBarTextEvent>().Publish(message);
        }

        private bool RemoveAccount(Account account)
        {
            if (dataManger.IsAccountInUse(account))
            {
                PictoMsgBox.ShowMessage("Kontierung in Verwendung", "Die Kontierung: '" + account.ToString() + "' wird aktuell noch verwendet. Bitte zuvor alle Verweise entfernen.");

                return false;
            }

            if ((bool)PictoMsgBox.ShowMessage("Kontierung löschen", "Die Kontierung: '" + account.ToString() + "' und deren Tasks wirklich löschen?", PictoMsgBoxButton.YesNo))
            {
                var tmp = account.ToString();
                dataManger.Accounts.Remove(account);

                PublishStatusMessage(tmp + " gelöscht");

                return true;
            }

            return false;
        }

        private bool RemoveTask(WorkTask task)
        {
            if (dataManger.IsTaskInUse(task))
            {
                PictoMsgBox.ShowMessage("Arbeitspaket in Verwendung", "Das Arbeitspaket: '" + task.ToString() + "' wird aktuell noch verwendet. Bitte zuvor alle Verweise entfernen.");

                return false;
            }

            if ((bool)PictoMsgBox.ShowMessage("Kontierung löschen", "Das Arbeitspaket: '" + task.ToString() + "' und deren Tasks wirklich löschen?", PictoMsgBoxButton.YesNo))
            {
                dataManger.FindCorrespondingAccount(task).AssignedTasks.Remove(task);

                PublishStatusMessage(task.ToString() + " gelöscht");

                return true;
            }

            return false;
        }

        #region Command functions

        private void InitializeCommands()
        {
            ItemSelectionChangedCommand = new DelegateCommand<object>(ItemSelectionChanged);
            AddAccountCommand = new DelegateCommand(AddAccount, CanAddAccount);
            AddTaskCommand = new DelegateCommand(AddTask, CanAddTask);
            EditItemCommand = new DelegateCommand(EditItem, CanEditItem);
            RemoveItemCommand = new DelegateCommand(RemoveItem, CanRemoveItem);
            SaveChangesCommand = new DelegateCommand(SaveChanges, CanSaveChanges);
            RevokeChangesCommand = new DelegateCommand(RevokeChanges, CanRevokeChanges);
        }

        private void ItemSelectionChanged(object param)
        {
            SelectedItem = param;
        }

        private void AddAccount()
        {
            if ((bool)new AccountEditor(AccountEditorViewModel.EditorStyle.Add, null).ShowDialog())
            {
                PendingChanges = true;
            }
        }

        private bool CanAddAccount()
        {
            return true;
        }

        private void AddTask()
        {
            if ((bool)new AccountEditor(AccountEditorViewModel.EditorStyle.Add, SelectedItem).ShowDialog())
            {
                PendingChanges = true;
            }
        }

        private bool CanAddTask()
        {
            return SelectedItem != null;
        }

        private void EditItem()
        {
            if ((bool)new AccountEditor(AccountEditorViewModel.EditorStyle.Edit, SelectedItem).ShowDialog())
            {
                PendingChanges = true;
            }
        }

        private bool CanEditItem()
        {
            return SelectedItem != null;
        }

        private void RemoveItem()
        {
            if (SelectedItem is Account)
            {
                if (RemoveAccount(SelectedItem as Account))
                {
                    PendingChanges = true;
                }
            }

            if (SelectedItem is WorkTask)
            {
                if (RemoveTask(SelectedItem as WorkTask))
                {
                    PendingChanges = true;
                }
            }
        }

        private bool CanRemoveItem()
        {
            return SelectedItem != null;
        }

        private void SaveChanges()
        {
            PendingChanges = false;

            dataManger.SaveChanges();

            PublishStatusMessage("Änderungen erfolgreich gespeichert");
        }

        private bool CanSaveChanges()
        {
            return PendingChanges;
        }

        private void RevokeChanges()
        {
            dataManger.LoadFromFile();

            InitializeProperties();

            PublishStatusMessage("Änderungen verworfen");
        }

        private bool CanRevokeChanges()
        {
            return pendingChanges;
        }

        #endregion

        #region INavigationAware implementation

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            SelectedItem = null;
        }

        #endregion

        #region Properties

        public string MappingToken
        {
            get {  return userSettings.MappingToken; }
            set
            {
                userSettings.MappingToken = value;
                RaisePropertyChanged(nameof(MappingToken));
                PendingChanges = true;
            }
        }

        public string MappingUrl
        {
            get { return userSettings.MappingUrl; }
            set
            {
                userSettings.MappingUrl = value;
                RaisePropertyChanged(nameof(MappingUrl));
                PendingChanges = true;
            }
        }

        public ObservableCollection<Account> CurrentAccounts
        {
            get { return currentAccounts; }
            set
            {
                SetProperty(ref currentAccounts, value);
            }
        }

        public object SelectedItem
        {
            get { return selectedItem; }
            set
            {
                SetProperty(ref selectedItem, value);
                AddTaskCommand.RaiseCanExecuteChanged();
                EditItemCommand.RaiseCanExecuteChanged();
                RemoveItemCommand.RaiseCanExecuteChanged();
            }
        }

        public bool PendingChanges
        {
            get { return pendingChanges; }
            set
            {
                SetProperty(ref pendingChanges, value);
                SaveChangesCommand.RaiseCanExecuteChanged();
                RevokeChangesCommand.RaiseCanExecuteChanged();
            }
        }

        public TimeSpan DefaultBeginOfWork
        {
            get { return userSettings.DefaultBeginOfWork; }
            set
            {
                userSettings.DefaultBeginOfWork = value;
                RaisePropertyChanged();
                PendingChanges = true;
            }
        }

        public TimeSpan DefaultEndOfWork
        {
            get { return userSettings.DefaultEndOfWork; }
            set
            {
                userSettings.DefaultEndOfWork = value;
                RaisePropertyChanged();
                PendingChanges = true;
            }
        }

        public TimeSpan DefaultDailyWorkTime
        {
            get { return userSettings.DefaultDailyWorkTime; }
            set
            {
                userSettings.DefaultDailyWorkTime = value;
                RaisePropertyChanged();
                PendingChanges = true;
            }
        }

        public TimeSpan DefaultBreakTime
        {
            get { return userSettings.DefaultBreakTime; }
            set
            {
                userSettings.DefaultBreakTime = value;
                RaisePropertyChanged();
                PendingChanges = true;
            }
        }

        public ICommand ItemSelectionChangedCommand { get; private set; }

        public DelegateCommand AddAccountCommand { get; private set; }

        public DelegateCommand AddTaskCommand { get; private set; }

        public DelegateCommand EditItemCommand { get; private set; }

        public DelegateCommand RemoveItemCommand { get; private set; }

        public DelegateCommand SaveChangesCommand { get; private set; }

        public DelegateCommand RevokeChangesCommand { get; private set; }

        #endregion
    }
}
