using KronosData.Logic;
using KronosData.Model;
using KronosUI.Events;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using System;

namespace KronosUI.ViewModels
{
    public class YearListingViewModel : ListingViewModelBase, INavigationAware
    {
        private readonly DataManager dataManager;

        public YearListingViewModel()
        {
            dataManager = ContainerLocator.Container.Resolve<DataManager>();
            ContainerLocator.Container.Resolve<IEventAggregator>().GetEvent<TimeframeChangedEvent>().Subscribe(TimeFrameUpdatedEventHandler);
        }

        private void UpdateYearListing()
        {

        }

        #region Eventhandler

        private void TimeFrameUpdatedEventHandler(DateTime newTimeFrame)
        {
            currentTimeFrame = newTimeFrame;
            PageTitle = currentTimeFrame.Year.ToString();
        }

        #endregion

        #region Inherited method implementation and overrides

        protected override void UpdateSummary(User currentUser, WorkDay wDay)
        {
            if (wDay != null)
            {
                summaryInfo = Summarizer.GetSummaryFromYear(currentUser, wDay.WorkTime.DateOfWork);
                RaisePropertyChanged(nameof(SummaryTotalHours));
                RaisePropertyChanged(nameof(SummaryTotalRequired));
                RaisePropertyChanged(nameof(SummaryTotalAccounted));
                RaisePropertyChanged(nameof(SummaryTotalOvertime));
            }
        }

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
            currentTimeFrame = currentTimeFrame.AddYears(-1);
            base.SwitchToPrevious();
        }

        public override void SwitchToNext()
        {
            currentTimeFrame = currentTimeFrame.AddYears(1);
            base.SwitchToNext();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            PageTitle = currentTimeFrame.Year.ToString();
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
