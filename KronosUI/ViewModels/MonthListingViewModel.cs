using KronosData.Logic;
using KronosUI.Events;
using Prism.Ioc;
using Prism.Events;
using System;
using Prism.Regions;

namespace KronosUI.ViewModels
{
    public class MonthListingViewModel : ListingViewModelBase, INavigationAware
    {
        private readonly DataManager dataManager;

        public MonthListingViewModel()
        {
            dataManager = ContainerLocator.Container.Resolve<DataManager>();
            ContainerLocator.Container.Resolve<IEventAggregator>().GetEvent<TimeframeChangedEvent>().Subscribe(TimeFrameUpdatedEventHandler);
        }

        private void UpdateMonthListing()
        {

        }

        #region Eventhandler

        private void TimeFrameUpdatedEventHandler(DateTime newTimeFrame)
        {
            currentTimeFrame = newTimeFrame;
            PageTitle = DateHelper.GetMonthNameFromDate(currentTimeFrame, true);
        }

        #endregion

        #region Inherited method implementation and overrides

        protected override void Initialize()
        {

        }

        public override bool CanSwitchToPrevious()
        {
            return true;
        }

        public override bool CanSwitchToNext()
        {
            return true;
        }

        public override void SwitchToPrevious()
        {
            currentTimeFrame = currentTimeFrame.AddMonths(-1);
            base.SwitchToPrevious();
        }

        public override void SwitchToNext()
        {
            currentTimeFrame = currentTimeFrame.AddMonths(1);
            base.SwitchToNext();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            PageTitle = DateHelper.GetMonthNameFromDate(currentTimeFrame, true);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // Unused
        }

        #endregion

        #region Properties



        #endregion
    }
}
