using KronosData.Logic;

namespace KronosUI.ViewModels
{
    public class WeekListingViewModel : ControlViewModelBase
    {
        protected override void Initialize()
        {
            CalendarValue = DateHelper.GetCalenderWeekFromDate(currentTimeFrame);
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
            base.SwitchToPrevious();
        }

        public override void SwitchToNext()
        {
            currentTimeFrame = currentTimeFrame.AddDays(7);
            base.SwitchToNext();
        }
    }
}
