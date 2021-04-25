using KronosData.Logic;
using KronosData.Model;
using Prism.Ioc;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace KronosUI.ViewModels
{
    public class ConfigurationViewModel : BindableBase
    {
        private ObservableCollection<Account> currentAccounts;

        private DataManager dataManger;

        public ConfigurationViewModel()
        {
            dataManger = ContainerLocator.Container.Resolve<DataManager>();
            CurrentAccounts = dataManger.Accounts;
        }

        #region Properties

        public ObservableCollection<Account> CurrentAccounts
        {
            get { return currentAccounts; }
            set
            {
                SetProperty(ref currentAccounts, value);
            }
        }

        #endregion
    }
}
