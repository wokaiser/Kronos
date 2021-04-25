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
            dataManger = ContainerLocator.Container.Resolve<DataManager>();
            CurrentAccounts = dataManger.Accounts;

            InitializeCommands();
        }

        #region Command functions

        private void InitializeCommands()
        {
            ItemSelectionChangedCommand = new DelegateCommand<object>(ItemSelectionChanged);
        }

        private void ItemSelectionChanged(object param)
        {
            if (param is Account)
            {
                MessageBox.Show("Account Nr: " + (param as Account).Number + " selected");
            }
            else if (param is WorkTask)
            {
                MessageBox.Show("Task: " + (param as WorkTask).Title + " selected");
            }
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
            }
        }

        public ICommand ItemSelectionChangedCommand { get; private set; }

        #endregion
    }
}
