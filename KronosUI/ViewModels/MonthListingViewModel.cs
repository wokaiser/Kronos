using static KronosUI.ViewModels.NavigationViewModel;

namespace KronosUI.ViewModels
{
    public class MonthListingViewModel : ControlViewModelBase
    {
        protected override void Initialize()
        {
            CalendarValue = ((MonthName)currentTimeFrame.Month).ToString();
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
    }
}
