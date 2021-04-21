using KronosUI.Model;
using KronosUI.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
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
        private IRegionManager regionManager;

        public ControlViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            PopulateCommands();
        }

        #region Command functions

        void PopulateCommands()
        {
            SwitchToConfigurationViewCommand = new DelegateCommand(SwitchToConfigurationView, CanSwitchToConfigurationView);
            SwitchToWeekListingViewCommand = new DelegateCommand(SwitchToWeekListingView, CanSwitchToWeekListingView);
            SwitchToMonthListingViewCommand = new DelegateCommand(SwitchToMonthListingView, CanSwitchToMonthListingView);
            SwitchToYearListingViewCommand = new DelegateCommand(SwitchToYearListingView, CanSwitchToYearListingView);
        }

        void SwitchToConfigurationView()
        {
            regionManager.RequestNavigate(RegionNames.DataRegion, ConfigurationView.ViewName);
            CurrentState = ViewState.Configuration;
        }

        bool CanSwitchToConfigurationView()
        {
            return CurrentState != ViewState.Configuration;
        }

        void SwitchToWeekListingView()
        {
            regionManager.RequestNavigate(RegionNames.DataRegion, WeekListingView.ViewName);
            CurrentState = ViewState.WeekListing;
        }

        bool CanSwitchToWeekListingView()
        {
            return CurrentState != ViewState.WeekListing;
        }

        void SwitchToMonthListingView()
        {
            regionManager.RequestNavigate(RegionNames.DataRegion, MonthListingView.ViewName);
            CurrentState = ViewState.MonthListing;
        }

        bool CanSwitchToMonthListingView()
        {
            return CurrentState != ViewState.MonthListing;
        }

        void SwitchToYearListingView()
        {
            regionManager.RequestNavigate(RegionNames.DataRegion, YearListingView.ViewName);
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
