using KronosData.Logic;
using KronosData.Model;
using Prism.Ioc;
using Prism.Mvvm;
using System.Collections.Generic;

namespace KronosUI.ViewModels
{
    public class ConfigurationViewModel : BindableBase
    {
        private List<Account> currentAccounts;
        private List<WorkTask> currentTasks;

        private DataManager dataManger;

        public ConfigurationViewModel()
        {
            dataManger = ContainerLocator.Container.Resolve<DataManager>();

            Initialize();
        }

        private void Initialize()
        {
            CurrentAccounts = dataManger.GetAllAccounts();
            CurrentTasks = dataManger.GetAllTasks();
        }

        #region Properties

        public List<Account> CurrentAccounts
        {
            get { return currentAccounts; }
            set
            {
                SetProperty(ref currentAccounts, value);
            }
        }

        public List<WorkTask> CurrentTasks
        {
            get { return currentTasks; }
            set
            {
                SetProperty(ref currentTasks, value);
            }
        }

        #endregion
    }
}
