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
        public enum ViewState { Configuration = 0, WeekListing, MonthListing, YearListing };

        private ViewState currentState = ViewState.Configuration;

        public ControlViewModel()
        {
            SwitchToConfigurationViewCommand = new DelegateCommand(SwitchToConfigurationView, CanSwitchToConfigurationView);
            SwitchToWeekListingViewCommand = new DelegateCommand(SwitchToWeekListingView, CanSwitchToWeekListingView);
            SwitchToMonthListingViewCommand = new DelegateCommand(SwitchToMonthListingView, CanSwitchToMonthListingView);
            SwitchToYearListingViewCommand = new DelegateCommand(SwitchToYearListingView, CanSwitchToYearListingView);
        }

        #region Command functions

        void SwitchToConfigurationView()
        {
            CurrentState = ViewState.Configuration;
        }

        bool CanSwitchToConfigurationView()
        {
            return CurrentState != ViewState.Configuration;
        }

        void SwitchToWeekListingView()
        {
            CurrentState = ViewState.WeekListing;
        }

        bool CanSwitchToWeekListingView()
        {
            return CurrentState != ViewState.WeekListing;
        }

        void SwitchToMonthListingView()
        {
            CurrentState = ViewState.MonthListing;
        }

        bool CanSwitchToMonthListingView()
        {
            return CurrentState != ViewState.MonthListing;
        }

        void SwitchToYearListingView()
        {
            CurrentState = ViewState.YearListing;
        }

        bool CanSwitchToYearListingView()
        {
            return CurrentState != ViewState.YearListing;
        }

        #endregion

        #region Properties

        public ViewState CurrentState
        {
            get { return currentState; }
            set
            {
                SetProperty(ref currentState, value);
                SwitchToConfigurationViewCommand.RaiseCanExecuteChanged();
                SwitchToWeekListingViewCommand.RaiseCanExecuteChanged();
                SwitchToMonthListingViewCommand.RaiseCanExecuteChanged();
                SwitchToYearListingViewCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand SwitchToConfigurationViewCommand { get; private set; }

        public DelegateCommand SwitchToWeekListingViewCommand { get; private set; }

        public DelegateCommand SwitchToMonthListingViewCommand { get; private set; }

        public DelegateCommand SwitchToYearListingViewCommand { get; private set; }

        #endregion
    }
}
