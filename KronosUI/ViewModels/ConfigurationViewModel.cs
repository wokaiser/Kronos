using KronosData.Logic;
using KronosData.Model;
using KronosUI.Controls;
using KronosUI.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace KronosUI.ViewModels
{
    public class ConfigurationViewModel : BindableBase
    {
        private ObservableCollection<Account> currentAccounts;
        private object selectedItem;
        private bool pendingChanges;

        private DataManager dataManger;

        public ConfigurationViewModel()
        {
            dataManger = ContainerLocator.Container.Resolve<DataManager>();
            CurrentAccounts = dataManger.Accounts;
            selectedItem = null;
            pendingChanges = false;

            InitializeCommands();
        }

        private void PublishStatusMessage(string message)
        {
            ContainerLocator.Container.Resolve<IEventAggregator>().GetEvent<UpdateStatusBarTextEvent>().Publish(message);
        }

        private bool RemoveAccount(Account account)
        {
            if (dataManger.IsAccountInUse(account))
            {
                MessageBox.Show("Die Kontierung: '" + account.ToString() + "' wird aktuell noch verwendet. Bitte zuvor alle Verweise entfernen.", "Kontierung in Verwendung", MessageBoxButton.OK, MessageBoxImage.Warning);

                return false;
            }

            if (MessageBox.Show("Die Kontierung: '" + account.ToString() + "' und deren Tasks wirklich löschen?", "Kontierung löschen", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var tmp = account.ToString();
                dataManger.Accounts.Remove(account);

                PublishStatusMessage(tmp + " gelöscht");
            }

            return true;
        }

        #region Command functions

        private void InitializeCommands()
        {
            ItemSelectionChangedCommand = new DelegateCommand<object>(ItemSelectionChanged);
            AddItemCommand = new DelegateCommand(AddItem, CanAddItem);
            EditItemCommand = new DelegateCommand(EditItem, CanEditItem);
            RemoveItemCommand = new DelegateCommand(RemoveItem, CanRemoveItem);
            SaveChangesCommand = new DelegateCommand(SaveChanges, CanSaveChanges);
        }

        private void ItemSelectionChanged(object param)
        {
            SelectedItem = param;
        }

        private void AddItem()
        {
            var editor = new AccountEditor(AccountEditor.EditorStyle.Add, SelectedItem);

            if ((bool)editor.ShowDialog())
            {
                PendingChanges = true;
            }
        }

        private bool CanAddItem()
        {
            return true;
        }

        private void EditItem()
        {
            var editor = new AccountEditor(AccountEditor.EditorStyle.Edit, SelectedItem);

            if ((bool)editor.ShowDialog())
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
        }

        private bool CanRemoveItem()
        {
            return SelectedItem != null;
        }

        private void SaveChanges()
        {
            PendingChanges = false;
            dataManger.SaveChanges();
            PublishStatusMessage("Changes successfully saved.");
        }

        private bool CanSaveChanges()
        {
            return PendingChanges;
        }

        #endregion

        #region Properties

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
                AddItemCommand.RaiseCanExecuteChanged();
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
            }
        }

        public ICommand ItemSelectionChangedCommand { get; private set; }

        public DelegateCommand AddItemCommand { get; private set; }

        public DelegateCommand EditItemCommand { get; private set; }

        public DelegateCommand RemoveItemCommand { get; private set; }

        public DelegateCommand SaveChangesCommand { get; private set; }

        #endregion
    }
}
