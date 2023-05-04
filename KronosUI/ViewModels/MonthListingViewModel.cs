﻿using KronosData.Logic;
using KronosData.Model;
using KronosUI.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KronosUI.ViewModels
{
    public class MonthListingViewModel : ListingViewModelBase, INavigationAware
    {
        private readonly DataManager dataManager;

        private Dictionary<string, TimeSpan> workByTasks;

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

        private void UploadToMappingExecute()
        {
            foreach (var task in workByTasks)
            {
                UploadTaskToMapping(task.Key, task.Value);
            }
        }

        private void UploadTaskToMapping(string id, TimeSpan duration)
        {
            var uploader = new MappingUploader(dataManager.CurrentUser.UserSettings.MappingUrl, dataManager.CurrentUser.UserSettings.MappingToken);

            if (!uploader.UploadTask(id, duration, currentTimeFrame))
            {
                MessageBox.Show("Upload will be added later.", "Not implemented yet", MessageBoxButton.OK, MessageBoxImage.Information);
            }
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
            workByTasks = wDay == null ? new Dictionary<string, TimeSpan>() : Summarizer.GetTaskOverviewFromMonth(currentUser, wDay.WorkTime.DateOfWork);

            RaisePropertyChanged(nameof(SummaryTotalHours));
            RaisePropertyChanged(nameof(SummaryTotalRequired));
            RaisePropertyChanged(nameof(SummaryTotalAccounted));
            RaisePropertyChanged(nameof(SummaryTotalOvertime));
            RaisePropertyChanged(nameof(SummaryTotalMobileDays));
            RaisePropertyChanged(nameof(SummaryTotalFreeDays));
            RaisePropertyChanged(nameof(SummaryTotalSickDays));
        }

        protected override void Initialize()
        {
            PageTitle = DateHelper.GetMonthNameFromDate(currentTimeFrame, true);
            UploadToMapping = new DelegateCommand(UploadToMappingExecute);
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

        public DelegateCommand UploadToMapping { get; private set; }

        #endregion
    }
}
