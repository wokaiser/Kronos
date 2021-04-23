using KronosUI.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;

namespace KronosUI.ViewModels
{
    public class WeekListingViewModel : BindableBase
    {
        private string calendarWeek;
        private DateTime currentTimeFrame;
        private IEventAggregator eventAggregator;

        public WeekListingViewModel()
        {
            eventAggregator = ContainerLocator.Container.Resolve<IEventAggregator>();
            currentTimeFrame = DateTime.Now;

            CalendarWeek = "KW" + (DateTime.Now.DayOfYear / 7).ToString();

            PopulateCommands();
        }

        #region Command functions

        private void PopulateCommands()
        {
            SwitchToPreviousWeekCommand = new DelegateCommand(SwitchToPreviousWeek, CanSwitchToPreviousWeek);
            SwitchToCurrentWeekCommand = new DelegateCommand(SwitchToCurrentWeek, CanSwitchToCurrentWeek);
            SwitchToNextWeekCommand = new DelegateCommand(SwitchToNextWeek, CanSwitchToNextWeek);
        }

        public void SwitchToPreviousWeek()
        {
            currentTimeFrame = currentTimeFrame.AddDays(-7);
            eventAggregator.GetEvent<TimeframeChangedEvent>().Publish(currentTimeFrame);
        }

        public void SwitchToCurrentWeek()
        {
            currentTimeFrame = DateTime.Now;
            eventAggregator.GetEvent<TimeframeChangedEvent>().Publish(currentTimeFrame);
        }

        public void SwitchToNextWeek()
        {
            currentTimeFrame = currentTimeFrame.AddDays(7);
            eventAggregator.GetEvent<TimeframeChangedEvent>().Publish(currentTimeFrame);
        }

        public bool CanSwitchToPreviousWeek()
        {
            return true;
        }

        public bool CanSwitchToCurrentWeek()
        {
            return true;
        }

        public bool CanSwitchToNextWeek()
        {
            return true;
        }

        #endregion

        #region Properties

        public string CalendarWeek
        {
            get { return calendarWeek; }
            set { SetProperty(ref calendarWeek, value); }
        }

        public DelegateCommand SwitchToPreviousWeekCommand { get; private set; }

        public DelegateCommand SwitchToCurrentWeekCommand { get; private set; }

        public DelegateCommand SwitchToNextWeekCommand { get; private set; }

        #endregion
    }
}
