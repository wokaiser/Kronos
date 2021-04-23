using KronosData.Logic;

namespace KronosUI.ViewModels
{
    public class WeekListingViewModel : ListingViewModelBase
    {


        #region Inherited method implementation and overrides

        protected override void Initialize()
        {
            CalendarValue = DateHelper.GetCalenderWeekFromDate(currentTimeFrame);
            PageTitle = DateHelper.GetCalenderWeekFromDate(currentTimeFrame);
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
            currentTimeFrame = currentTimeFrame.AddDays(-7);
            PageTitle = DateHelper.GetCalenderWeekFromDate(currentTimeFrame);
            base.SwitchToPrevious();
        }

        public override void SwitchToCurrent()
        {
            base.SwitchToCurrent();
            PageTitle = DateHelper.GetCalenderWeekFromDate(currentTimeFrame);
        }

        public override void SwitchToNext()
        {
            currentTimeFrame = currentTimeFrame.AddDays(7);
            PageTitle = DateHelper.GetCalenderWeekFromDate(currentTimeFrame);
            base.SwitchToNext();
        }

        #endregion
    }
}
