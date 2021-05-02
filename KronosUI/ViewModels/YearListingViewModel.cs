namespace KronosUI.ViewModels
{
    public class YearListingViewModel : ControlViewModelBase
    {
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
    }
}
