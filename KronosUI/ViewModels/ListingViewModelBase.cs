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
            var hours = Math.Abs(tSpan.Days * 24 + tSpan.Hours);
            var minutes = Math.Abs(tSpan.Minutes);
            var prefix = tSpan < TimeSpan.Zero ? "-" : string.Empty;
            var suffix = tSpan < TimeSpan.Zero ? " " : string.Empty;

            return string.Format($"{prefix}{hours:00}:{minutes:00}{suffix}");
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

        public virtual string SummaryTotalOvertime
        {
            get
            {
                return summaryInfo != null ? ToHoursMinutesString(summaryInfo.TotalOvertime) : string.Empty;
            }
        }

        public string SummaryTotalMobileDays
        {
            get
            {
                return summaryInfo != null ? summaryInfo.TotalMobileDays.ToString() : string.Empty;
            }
        }

        public string SummaryTotalSickDays
        {
            get
            {
                return summaryInfo != null ? summaryInfo.TotalSickDays.ToString() : string.Empty;
            }
        }

        public string SummaryTotalFreeDays
        {
            get
            {
                return summaryInfo != null ? summaryInfo.TotalFreeDays.ToString() : string.Empty;
            }
        }

        #endregion
    }
}
