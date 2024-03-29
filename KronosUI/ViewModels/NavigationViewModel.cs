﻿using KronosData.Logic;
using KronosUI.Controls;
using KronosUI.Events;
using KronosUI.Model;
using KronosUI.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Windows;

namespace KronosUI.ViewModels
{
    public class NavigationViewModel : BindableBase
    {
        public enum ViewState { Configuration = 0, WeekListing, MonthListing, YearListing };

        private bool canAppTerminate = true;
        private string calendarWeek;
        private string calendarMonth;
        private string calendarYear;

        private ViewState currentState;
        private IRegionManager regionManager;
        private IEventAggregator eventAggregator;

        public NavigationViewModel()
        {
            regionManager = ContainerLocator.Container.Resolve<IRegionManager>();
            eventAggregator = ContainerLocator.Container.Resolve<IEventAggregator>();

            currentState = ViewState.WeekListing;
            eventAggregator.GetEvent<TimeframeChangedEvent>().Subscribe(TimeframeChangedEventHandler);

            PopulateCommands();
            SetButtonTexts(DateTime.Now);
        }

        private void SetButtonTexts(DateTime date)
        {
            CalendarWeek = DateHelper.GetCalenderWeekFromDate(date);
            CalendarMonth = DateHelper.GetMonthNameFromDate(date);
            CalendarYear = date.Year.ToString();
        }

        #region Event handling

        private void TimeframeChangedEventHandler(DateTime timeFrame)
        {
            SetButtonTexts(timeFrame);
        }

        #endregion

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
            regionManager.RequestNavigate(RegionNames.ControlRegion, ConfigurationControlView.ViewName);
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
            regionManager.RequestNavigate(RegionNames.ControlRegion, WeekListingControlView.ViewName);
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
            regionManager.RequestNavigate(RegionNames.ControlRegion, MonthListingControlView.ViewName);
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
            regionManager.RequestNavigate(RegionNames.ControlRegion, YearListingControlView.ViewName);
            CurrentState = ViewState.YearListing;
            eventAggregator.GetEvent<UpdateStatusBarTextEvent>().Publish("Year listing loaded");
        }

        bool CanSwitchToYearListingView()
        {
            return CurrentState != ViewState.YearListing;
        }

        void Exit()
        {            
            if ((bool)PictoMsgBox.ShowMessage("Möchten Sie wirklich das Programm beenden?", "Beenden", PictoMsgBoxButton.YesNo))
            {
                Application.Current.Shutdown();
            }
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
