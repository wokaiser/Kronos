using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KronosUI.ViewModels
{
    public class ControlViewModel : BindableBase
    {
        public ControlViewModel()
        {
            SwitchToConfigurationViewCommand = new DelegateCommand(SwitchToConfigurationView, CanSwitchToConfigurationView);
        }

        #region Command functions

        void SwitchToConfigurationView()
        {
            MessageBox.Show("Hello world");
        }

        bool CanSwitchToConfigurationView()
        {
            return true;
        }

        #endregion

        #region Properties

        public DelegateCommand SwitchToConfigurationViewCommand { get; private set; }

        #endregion
    }
}
