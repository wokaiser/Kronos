using KronosUI.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using static KronosUI.ViewModels.NavigationViewModel;

namespace KronosUI.ViewModels
{
    public class MonthListingViewModel : BindableBase
    {
        private string calendarMonth;
        private DateTime currentTimeFrame;
        private IEventAggregator eventAggregator;

        public MonthListingViewModel()
        {
            eventAggregator = ContainerLocator.Container.Resolve<IEventAggregator>();
            currentTimeFrame = DateTime.Now;

            CalendarMonth = ((MonthName)currentTimeFrame.Month).ToString();

            PopulateCommands();
        }

        #region Command functions

        private void PopulateCommands()
        {
            SwitchToPreviousMonthCommand = new DelegateCommand(SwitchToPreviousMonth, CanSwitchToPreviousMonth);
            SwitchToCurrentMonthCommand = new DelegateCommand(SwitchToCurrentMonth, CanSwitchToCurrentMonth);
            SwitchToNextMonthCommand = new DelegateCommand(SwitchToNextMonth, CanSwitchToNextMonth);
        }

        public void SwitchToPreviousMonth()
        {
            currentTimeFrame = currentTimeFrame.AddMonths(-1);
            eventAggregator.GetEvent<TimeframeChangedEvent>().Publish(currentTimeFrame);
        }

        public void SwitchToCurrentMonth()
        {
            currentTimeFrame = DateTime.Now;
            eventAggregator.GetEvent<TimeframeChangedEvent>().Publish(currentTimeFrame);
        }

        public void SwitchToNextMonth()
        {
            currentTimeFrame = currentTimeFrame.AddMonths(1);
            eventAggregator.GetEvent<TimeframeChangedEvent>().Publish(currentTimeFrame);
        }

        public bool CanSwitchToPreviousMonth()
        {
            return true;
        }

        public bool CanSwitchToCurrentMonth()
        {
            return true;
        }

        public bool CanSwitchToNextMonth()
        {
            return true;
        }

        #endregion

        #region Properties

        public string CalendarMonth
        {
            get { return calendarMonth; }
            set { SetProperty(ref calendarMonth, value); }
        }

        public DelegateCommand SwitchToPreviousMonthCommand { get; private set; }

        public DelegateCommand SwitchToCurrentMonthCommand { get; private set; }

        public DelegateCommand SwitchToNextMonthCommand { get; private set; }

        #endregion
    }
}
