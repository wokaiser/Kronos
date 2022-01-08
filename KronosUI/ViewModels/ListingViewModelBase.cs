using KronosData.Model;
using System;

namespace KronosUI.ViewModels
{
    public abstract class ListingViewModelBase : ControlViewModelBase
    {
        private string pageTitle;

        protected SummaryInfo summaryInfo;

        protected abstract void UpdateSummary(User currentUser, WorkDay wDay);

        protected static string ToHoursMinutesString(TimeSpan tSpan)
        {
            var hours = tSpan.Days * 24 + tSpan.Hours;
            var minutes = tSpan.Minutes;

            return string.Format("{0:00}:{1:00}", hours, minutes);
        }

        #region Properties

        public string PageTitle
        {
            get { return pageTitle; }
            set { SetProperty(ref pageTitle, value); }
        }

        public string SummaryTotalHours
        {
            get
            {
                return summaryInfo != null ? ToHoursMinutesString(summaryInfo.TotalWorkHours) : string.Empty;
            }
        }

        public string SummaryTotalRequired
        {
            get
            {
                return summaryInfo != null ? ToHoursMinutesString(summaryInfo.RequiredWorkHours) : string.Empty;
            }
        }

        public string SummaryTotalAccounted
        {
            get
            {
                return summaryInfo != null ? ToHoursMinutesString(summaryInfo.TotalAccountedHours) : string.Empty;
            }
        }

        public string SummaryTotalOvertime
        {
            get
            {
                return summaryInfo != null ? ToHoursMinutesString(summaryInfo.TotalOvertime) : string.Empty;
            }
        }

        #endregion
    }
}
