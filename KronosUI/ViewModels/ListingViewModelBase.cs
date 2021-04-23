namespace KronosUI.ViewModels
{
    public abstract class ListingViewModelBase : ControlViewModelBase
    {
        private string pageTitle;

        #region Properties

        public string PageTitle
        {
            get { return pageTitle; }
            set { SetProperty(ref pageTitle, value); }
        }

        #endregion
    }
}
