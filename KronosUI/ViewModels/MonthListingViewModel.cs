using KronosData.Logic;
using KronosData.Model;
using KronosUI.Controls;
using KronosUI.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KronosUI.ViewModels
{
    public class MonthListingViewModel : ListingViewModelBase, INavigationAware
    {
        private readonly DataManager dataManager;

        private Dictionary<WorkTask, TimeSpan> workByTasks;
        private string workByAccounts;

        public MonthListingViewModel()
        {
            dataManager = ContainerLocator.Container.Resolve<DataManager>();
            ContainerLocator.Container.Resolve<IEventAggregator>().GetEvent<TimeframeChangedEvent>().Subscribe(TimeFrameUpdatedEventHandler);

            UpdateMonthListing();
        }

        private void UpdateMonthListing()
        {
            var firstDay = dataManager.CurrentUser.AssignedWorkDays.FirstOrDefault(d => d.WorkTime.DateOfWork.Date.Year == currentTimeFrame.Year && d.WorkTime.DateOfWork.Date.Month == currentTimeFrame.Month);

            UpdateSummary(dataManager.CurrentUser, firstDay);
        }

        private bool CanUploadToMapping()
        {
            return !string.IsNullOrWhiteSpace(WorkByTasks);
        }

        private void UploadToMappingExecute()
        {
            if (!(bool)PictoMsgBox.ShowMessage("Die Stunden dieses Monats in Mapping eintragen?", "WARNUNG: Bereits in Mapping eingetragene Werte für diesen Monat werden überschrieben!", PictoMsgBoxButton.YesNo))
            {
                return;
            }

            var sb = new StringBuilder();

            foreach (var task in workByTasks)
            {
                sb.Append(UploadTaskToMapping(task.Key, task.Value));
            }

            if (sb.Length != 0)
            {
                PictoMsgBox.ShowMessage("Fehler beim hochladen", sb.ToString(), PictoMsgBoxButton.OK);
            }
            else
            {
                PictoMsgBox.ShowMessage("Hochladen", "Hochladen erfolgreich", PictoMsgBoxButton.OK);
            }
        }

        private string UploadTaskToMapping(WorkTask task, TimeSpan duration)
        {
            var uploader = new MappingUploader(dataManager.CurrentUser.UserSettings.MappingUrl, dataManager.CurrentUser.UserSettings.MappingToken);

            if (!uploader.UploadTask(task.MappingID, duration, currentTimeFrame))
            {
                return $"Task [{task}] konnte nicht hochgeladen werden.\n";
            }

            return string.Empty;
        }

        private void UpdateAccountsSummary(IEnumerable<WorkDay> workMonth)
        {
            var sb = new StringBuilder();

            double totMin = (double)(workMonth.Sum(wd => wd.TotalAccountedTime.Minutes) % 60) / 60 * 100;
            double totHrs = Math.Floor(workMonth.Sum(wd => wd.TotalAccountedTime.TotalHours));

            sb.AppendLine($"===== Total[{totHrs:000},{totMin:00}h] ======");

            foreach (var workDay in workMonth.OrderBy(d => d.WorkTime.DateOfWork))
            {
                if (workDay.TotalWorkTime == TimeSpan.Zero)
                {
                    continue;
                }

                totMin = ((double)workDay.TotalAccountedTime.Minutes / 60) * 100;
                totHrs = Math.Floor(workDay.TotalAccountedTime.TotalHours);
                sb.AppendLine($"=== {workDay.WorkTime.DateOfWork.ToShortDateString()} [{totHrs:00},{totMin:00}h] ===");
                var dict = new Dictionary<string, TimeSpan>();
                foreach (var work in workDay.AssignedWorkItems)
                {
                    var key = work.AssignedWorkTask.AssignedAccountNumber;
                    if (dict.ContainsKey(key))
                    {
                        dict[key] += work.Duration;
                    }
                    else
                    {
                        dict.Add(key, work.Duration);
                    }
                }
                foreach (var account in dict.OrderBy(i => i.Key))
                {
                    double min = (double)account.Value.Minutes / 60 * 100;
                    double hrs = Math.Floor(account.Value.TotalHours);

                    sb.AppendFormat("{0,18} [{1:00},{2:00}h]\n", account.Key, hrs, min);
                }
                sb.AppendFormat("\n");
            }

            WorkByAccounts = sb.ToString();
        }

        #region Eventhandler

        private void TimeFrameUpdatedEventHandler(DateTime newTimeFrame)
        {
            currentTimeFrame = newTimeFrame;
            PageTitle = DateHelper.GetMonthNameFromDate(currentTimeFrame, true);
            UpdateMonthListing();
        }

        #endregion

        #region Inherited method implementation and overrides

        protected override void UpdateSummary(User currentUser, WorkDay wDay)
        {
            summaryInfo = wDay == null ? SummaryInfo.Zero : Summarizer.GetSummaryFromMonth(currentUser, wDay.WorkTime.DateOfWork);
            workByTasks = wDay == null ? new Dictionary<WorkTask, TimeSpan>() : Summarizer.GetTaskOverviewFromMonth(currentUser, wDay.WorkTime.DateOfWork);

            if (wDay != null)
            {
                UpdateAccountsSummary(Summarizer.GetAccountOverviewFromMonth(currentUser, wDay.WorkTime.DateOfWork));
            }
            else
            {
                WorkByAccounts = string.Empty;
            }

            RaisePropertyChanged(nameof(SummaryTotalHours));
            RaisePropertyChanged(nameof(SummaryTotalRequired));
            RaisePropertyChanged(nameof(SummaryTotalAccounted));
            RaisePropertyChanged(nameof(SummaryTotalOvertime));
            RaisePropertyChanged(nameof(SummaryTotalMobileDays));
            RaisePropertyChanged(nameof(SummaryTotalFreeDays));
            RaisePropertyChanged(nameof(SummaryTotalSickDays));
            RaisePropertyChanged(nameof(WorkByTasks));

            UploadToMappingCommand.RaiseCanExecuteChanged();
            
        }

        protected override void Initialize()
        {
            PageTitle = DateHelper.GetMonthNameFromDate(currentTimeFrame, true);
            UploadToMappingCommand = new DelegateCommand(UploadToMappingExecute, CanUploadToMapping);
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

        public override void SwitchToCurrent()
        {
            base.SwitchToCurrent();
        }

        public override void SwitchToNext()
        {
            currentTimeFrame = currentTimeFrame.AddMonths(1);
            base.SwitchToNext();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            UpdateMonthListing();
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

        public DelegateCommand UploadToMappingCommand { get; private set; }

        public string WorkByTasks
        {
            get
            {
                var sb = new StringBuilder();

                foreach (var item in workByTasks.OrderByDescending(i => i.Value))
                {
                    double min = ((double)item.Value.Minutes / 60) * 100;
                    double hrs = Math.Floor(item.Value.TotalHours);

                    sb.AppendFormat("{0,4} [{1:00},{2:00}h] {3}\n", item.Key.MappingID, hrs, min, item.Key.Title);
                }
                return sb.ToString();
            }
        }

        public string WorkByAccounts
        {
            get { return workByAccounts; }
            set { SetProperty(ref workByAccounts, value); }
        }

        #endregion
    }
}
