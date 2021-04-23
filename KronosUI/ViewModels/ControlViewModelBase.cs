using KronosUI.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;

namespace KronosUI.ViewModels
{
    public abstract class ControlViewModelBase : BindableBase
    {
        private string calendarValue;
        private IEventAggregator eventAggregator;

        protected DateTime currentTimeFrame;

        public ControlViewModelBase()
        {
            eventAggregator = ContainerLocator.Container.Resolve<IEventAggregator>();

            currentTimeFrame = DateTime.Now;

            Initialize();

            PopulateCommands();
        }

        public abstract void Initialize();

        #region Command functions

        private void PopulateCommands()
        {
            SwitchToPreviousCommand = new DelegateCommand(SwitchToPrevious, CanSwitchToPrevious);
            SwitchToCurrentCommand = new DelegateCommand(SwitchToCurrent, CanSwitchToCurrent);
            SwitchToNextCommand = new DelegateCommand(SwitchToNext, CanSwitchToNext);
        }

        public virtual void SwitchToPrevious()
        {
            eventAggregator.GetEvent<TimeframeChangedEvent>().Publish(currentTimeFrame);
        }

        public void SwitchToCurrent()
        {
            currentTimeFrame = DateTime.Now;
            eventAggregator.GetEvent<TimeframeChangedEvent>().Publish(currentTimeFrame);
        }

        public virtual void  SwitchToNext()
        {
            eventAggregator.GetEvent<TimeframeChangedEvent>().Publish(currentTimeFrame);
        }

        public abstract bool CanSwitchToPrevious();

        public bool CanSwitchToCurrent()
        {
            return true;
        }

        public abstract bool CanSwitchToNext();

        #endregion

        #region Properties

        public string CalendarValue
        {
            get { return calendarValue; }
            set { SetProperty(ref calendarValue, value); }
        }

        public DelegateCommand SwitchToPreviousCommand { get; private set; }

        public DelegateCommand SwitchToCurrentCommand { get; private set; }

        public DelegateCommand SwitchToNextCommand { get; private set; }

        #endregion
    }
}
