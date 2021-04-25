using KronosData.Logic;
using KronosData.Model;
using Prism.Commands;
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

        private DataManager dataManger;

        public ConfigurationViewModel()
        {
            InitializeCommands();

            dataManger = ContainerLocator.Container.Resolve<DataManager>();
            CurrentAccounts = dataManger.Accounts;
            SelectedItem = null;
        }

        #region Command functions

        private void InitializeCommands()
        {
            ItemSelectionChangedCommand = new DelegateCommand<object>(ItemSelectionChanged);
            AddItemCommand = new DelegateCommand(AddItem, CanAddItem);
            EditItemCommand = new DelegateCommand(EditItem, CanEditItem);
            RemoveItemCommand = new DelegateCommand(RemoveItem, CanRemoveItem);
        }

        private void ItemSelectionChanged(object param)
        {
            SelectedItem = param;
        }

        private void AddItem()
        {

        }

        private bool CanAddItem()
        {
            return true;
        }

        private void EditItem()
        {

        }

        private bool CanEditItem()
        {
            return SelectedItem != null;
        }

        private void RemoveItem()
        {

        }

        private bool CanRemoveItem()
        {
            return SelectedItem != null;
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

        public ICommand ItemSelectionChangedCommand { get; private set; }

        public DelegateCommand AddItemCommand { get; private set; }

        public DelegateCommand EditItemCommand { get; private set; }

        public DelegateCommand RemoveItemCommand { get; private set; }

        #endregion
    }
}
