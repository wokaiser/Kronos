using KronosData.Logic;
using KronosData.Model;
using KronosUI.Controls;
using KronosUI.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace KronosUI.ViewModels
{
    public class WeekListingViewModel : ListingViewModelBase, INavigationAware
    {
        private readonly DataManager dataManager;

        private ObservableCollection<WorkDay> currentWorkWeek;
        private WorkDay currentWorkDay;
        private bool pendingChanges;

        public WeekListingViewModel()
        {
            dataManager = ContainerLocator.Container.Resolve<DataManager>();
            ContainerLocator.Container.Resolve<IEventAggregator>().GetEvent<TimeframeChangedEvent>().Subscribe(TimeFrameUpdatedEventHandler);

            InitializeCommands();

            UpdateWeekListing();
        }

        private void UpdateWeekListing()
        {
            CurrentWorkWeek = new ObservableCollection<WorkDay>();

            var start = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            for (var i = start; i < start + 7; i++)
            {
                AddWorkDay((DayOfWeek)i);
            }

            UpdateSummary(dataManager.CurrentUser, CurrentWorkWeek.FirstOrDefault());
        }

        private void AddWorkDay(DayOfWeek dow)
        {
            var wDay = new WorkDay();
            wDay.WorkTime.DateOfWork = CalcDayOfWeek(currentTimeFrame, dow);

            var dayToAdd = dataManager.CurrentUser.AssignedWorkDays.FirstOrDefault(d => d.WorkTime.DateOfWork.Date.Equals(wDay.WorkTime.DateOfWork.Date)) ?? wDay;
            CurrentWorkWeek.Add(dayToAdd);

            if (dayToAdd.WorkTime.DateOfWork.Date.Equals(DateTime.Now.Date))
            {
                CurrentWorkDay = dayToAdd;
            }
        }

        private static DateTime CalcDayOfWeek(DateTime val, DayOfWeek reqDay)
        {
            return val.AddDays((int)reqDay - (val.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)val.DayOfWeek));
        }

        #region Evenhandler

        private void TimeFrameUpdatedEventHandler(DateTime newTimeframe)
        {
            currentTimeFrame = newTimeframe;
            PageTitle = DateHelper.GetCalenderWeekFromDate(currentTimeFrame);
            UpdateWeekListing();
        }

        #endregion

        #region Command implementations

        private void InitializeCommands()
        {
            EditItemCommand = new DelegateCommand(EditItem, CanEditItem);
            RevokeChangesCommand = new DelegateCommand(RevokeChanges, CanRevokeChanges);
            SaveChangesCommand = new DelegateCommand(SaveChanges, CanSaveChanges);
            DeleteItemCommand = new DelegateCommand(DeleteItem, CanDeleteItem);
        }

        public void EditItem()
        {
            if ((bool)new WorkDayEditor(CurrentWorkDay).ShowDialog())
            {
                UpdateWeekListing();
                PendingChanges = true;
            }
        }

        public bool CanEditItem()
        {
            return CurrentWorkDay != null;
        }

        public void DeleteItem()
        {
            if (!(bool)PictoMsgBox.ShowMessage("Arbeitstag löschen", "Sind Sie sich sicher den gewählten Arbeitsag zu entfernen?", PictoMsgBoxButton.YesNo))
            {
                return;
            }

            dataManager.CurrentUser.AssignedWorkDays.Remove(CurrentWorkDay);
            UpdateWeekListing();
            PendingChanges = true;
        }

        public bool CanDeleteItem()
        {
            if (CurrentWorkDay != null)
            {
                return CurrentWorkDay.WorkTime.Duration > TimeSpan.Zero || CurrentWorkDay.IsFreeDay ||CurrentWorkDay.IsSickDay;
            }

            return false;
        }

        public void RevokeChanges()
        {
            dataManager.LoadFromFile();
            UpdateWeekListing();

            PendingChanges = false;
        }

        public bool CanRevokeChanges()
        {
            return PendingChanges;
        }

        public void SaveChanges()
        {
            dataManager.SaveChanges();
            PendingChanges = false;
        }

        public bool CanSaveChanges()
        {
            return PendingChanges;
        }

        #endregion

        #region Inherited method implementation and overrides

        protected override void UpdateSummary(User currentUser, WorkDay wDay)
        {
            summaryInfo = wDay == null ? SummaryInfo.Zero : Summarizer.GetSummaryFromWeek(currentUser, wDay.WorkTime.DateOfWork);

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
            base.SwitchToPrevious();
        }

        public override void SwitchToCurrent()
        {
            base.SwitchToCurrent();
        }

        public override void SwitchToNext()
        {
            currentTimeFrame = currentTimeFrame.AddDays(7);
            base.SwitchToNext();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            UpdateWeekListing();
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

        public DelegateCommand EditItemCommand { get; private set; }

        public DelegateCommand RevokeChangesCommand { get; private set; }

        public DelegateCommand SaveChangesCommand { get; private set; }

        public DelegateCommand DeleteItemCommand { get; private set; }

        public ObservableCollection<WorkDay> CurrentWorkWeek
        {
            get { return currentWorkWeek; }
            set
            {
                SetProperty(ref currentWorkWeek, value);
            }
        }

        public WorkDay CurrentWorkDay
        {
            get { return currentWorkDay; }
            set
            {
                SetProperty(ref currentWorkDay, value);
                EditItemCommand.RaiseCanExecuteChanged();
                DeleteItemCommand.RaiseCanExecuteChanged();
            }
        }

        public bool PendingChanges
        {
            get { return pendingChanges; }
            set
            {
                SetProperty(ref pendingChanges, value);
                RevokeChangesCommand.RaiseCanExecuteChanged();
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion
    }
}
