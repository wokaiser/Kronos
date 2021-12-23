using KronosData.Logic;
using KronosData.Model;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace KronosUI.Controls
{
    public class WorkItemEditorViewModel : BindableBase
    {
        private readonly DataManager dataManager;

        private object selectedItem;
        private WorkItem currentWorkItem;
        private ObservableCollection<Account> currentAccounts;

        public WorkItemEditorViewModel(WorkItem selectedItem)
        {
            dataManager = ContainerLocator.Container.Resolve<DataManager>();
            CurrentWorkItem = selectedItem;

            Initialize();
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
            SaveChangesCommand = new DelegateCommand<Window>(SaveChanges);
            RevokeChangesCommand = new DelegateCommand<Window>(RevokeChanges);
        }

        private void ItemSelectionChanged(object param)
        {
            SelectedItem = param;
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

        public object SelectedItem
        {
            get { return selectedItem; }
            set { SetProperty(ref selectedItem, value); }
        }

        #endregion
    }
}
