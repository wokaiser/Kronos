using KronosUI.Events;
using KronosUI.Model;
using KronosUI.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Windows;

namespace KronosUI.ViewModels
{
    public class NavigationViewModel : BindableBase
    {
        public enum ViewState { Configuration = 0, WeekListing, MonthListing, YearListing };
        public enum MonthName { Jan = 1, Feb, März, April, Mai, Juni, Juli, Aug, Sept, Okt, Nov, Dez }

        private bool canAppTerminate = true;
        private string calendarWeek;
        private string calendarMonth;
        private string calendarYear;

        private ViewState currentState;
        private IRegionManager regionManager;
        private IEventAggregator eventAggregator;

        public NavigationViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            currentState = ViewState.WeekListing;
            PopulateCommands();
            SetButtonTexts();
        }

        private void SetButtonTexts()
        {
            CalendarWeek = "KW" + (DateTime.Now.DayOfYear / 7).ToString();
            CalendarMonth = ((MonthName)DateTime.Now.Month).ToString();
            CalendarYear = DateTime.Now.Year.ToString();
        }

        #region Command functions

        void PopulateCommands()
        {
            SwitchToConfigurationViewCommand = new DelegateCommand(SwitchToConfigurationView, CanSwitchToConfigurationView);
            SwitchToWeekListingViewCommand = new DelegateCommand(SwitchToWeekListingView, CanSwitchToWeekListingView);
            SwitchToMonthListingViewCommand = new DelegateCommand(SwitchToMonthListingView, CanSwitchToMonthListingView);
            SwitchToYearListingViewCommand = new DelegateCommand(SwitchToYearListingView, CanSwitchToYearListingView);
            ExitCommand = new DelegateCommand(Exit, CanExit);
        }

        void SwitchToConfigurationView()
        {
            regionManager.RequestNavigate(RegionNames.DataRegion, ConfigurationView.ViewName);
            CurrentState = ViewState.Configuration;
            eventAggregator.GetEvent<UpdateStatusBarTextEvent>().Publish("Configuration page loaded");
        }

        bool CanSwitchToConfigurationView()
        {
            return CurrentState != ViewState.Configuration;
        }

        void SwitchToWeekListingView()
        {
            regionManager.RequestNavigate(RegionNames.DataRegion, WeekListingView.ViewName);
            CurrentState = ViewState.WeekListing;
            eventAggregator.GetEvent<UpdateStatusBarTextEvent>().Publish("Week listing loaded");
        }

        bool CanSwitchToWeekListingView()
        {
            return CurrentState != ViewState.WeekListing;
        }

        void SwitchToMonthListingView()
        {
            regionManager.RequestNavigate(RegionNames.DataRegion, MonthListingView.ViewName);
            CurrentState = ViewState.MonthListing;
            eventAggregator.GetEvent<UpdateStatusBarTextEvent>().Publish("Month listing loaded");
        }

        bool CanSwitchToMonthListingView()
        {
            return CurrentState != ViewState.MonthListing;
        }

        void SwitchToYearListingView()
        {
            regionManager.RequestNavigate(RegionNames.DataRegion, YearListingView.ViewName);
            CurrentState = ViewState.YearListing;
            eventAggregator.GetEvent<UpdateStatusBarTextEvent>().Publish("Year listing loaded");
        }

        bool CanSwitchToYearListingView()
        {
            return CurrentState != ViewState.YearListing;
        }

        void Exit()
        {
            Application.Current.Shutdown();
        }

        bool CanExit()
        {
            return CanAppTerminate;
        }

        #endregion

        #region Properties

        public string CalendarWeek
        {
            get { return calendarWeek; }
            set { SetProperty(ref calendarWeek, value); }
        }

        public string CalendarMonth
        {
            get { return calendarMonth; }
            set { SetProperty(ref calendarMonth, value); }
        }

        public string CalendarYear
        {
            get { return calendarYear; }
            set { SetProperty(ref calendarYear, value); }
        }

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

        public bool CanAppTerminate
        {
            get { return canAppTerminate; }
            set
            {
                SetProperty(ref canAppTerminate, value);
                ExitCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand SwitchToConfigurationViewCommand { get; private set; }

        public DelegateCommand SwitchToWeekListingViewCommand { get; private set; }

        public DelegateCommand SwitchToMonthListingViewCommand { get; private set; }

        public DelegateCommand SwitchToYearListingViewCommand { get; private set; }

        public DelegateCommand ExitCommand { get; private set; }

        #endregion
    }
}
