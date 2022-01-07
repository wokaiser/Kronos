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
        private ObservableCollection<WorkDay> currentWorkWeek;
        private SummaryInfo summaryInfo;
        private WorkDay currentWorkDay;
        private bool pendingChanges;

        private readonly DataManager dataManager;

        public WeekListingViewModel()
        {
            dataManager = ContainerLocator.Container.Resolve<DataManager>();
            ContainerLocator.Container.Resolve<IEventAggregator>().GetEvent<TimeframeChangedEvent>().Subscribe(TimeFrameUpdatedEventHandler);

            InitializeCommands();
            FillWorkWeek();
        }

        private void FillWorkWeek()
        {
            CurrentWorkWeek = new ObservableCollection<WorkDay>();

            var start = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            for (var i =  start; i < start + 7; i++)
            {
                AddWorkDay((DayOfWeek)i);
            }
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

        private void UpdateSummary(WorkDay wDay)
        {
            if (wDay != null)
            {
                summaryInfo = Summarizer.GetSummaryFromWeek(dataManager.CurrentUser, wDay.WorkTime.DateOfWork);
                RaisePropertyChanged(nameof(SummaryTotalHours));
                RaisePropertyChanged(nameof(SummaryTotalRequired));
                RaisePropertyChanged(nameof(SummaryTotalAccounted));
                RaisePropertyChanged(nameof(SummaryOvertime));
            }
        }

        private static DateTime CalcDayOfWeek(DateTime val, DayOfWeek reqDay)
        {
            return val.AddDays((int)reqDay - (val.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)val.DayOfWeek));
        }

        private static string ToHoursMinutesString(TimeSpan tSpan)
        {
            var hours = tSpan.Days * 24 + tSpan.Hours;
            var minutes = tSpan.Minutes;

            return string.Format("{0:00}:{1:00}", hours, minutes);
        }

        #region Evenhandler

        private void TimeFrameUpdatedEventHandler(DateTime newTimeframe)
        {
            currentTimeFrame = newTimeframe;
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
                FillWorkWeek();
                PendingChanges = true;
            }
        }

        public bool CanEditItem()
        {
            return CurrentWorkDay != null;
        }

        public void DeleteItem()
        {
            if (!(bool)PictoMsgBox.ShowMessage("Delete workday", "Are you sure to delete the selected workday?", PictoMsgBoxButton.YesNo))
            {
                return;
            }

            dataManager.CurrentUser.AssignedWorkDays.Remove(CurrentWorkDay);
            FillWorkWeek();
            PendingChanges = true;
        }

        public bool CanDeleteItem()
        {
            if (CurrentWorkDay != null)
            {
                return CurrentWorkDay.WorkTime.Duration > TimeSpan.Zero;
            }

            return false;
        }

        public void RevokeChanges()
        {
            dataManager.LoadFromFile();
            FillWorkWeek();

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
            PageTitle = DateHelper.GetCalenderWeekFromDate(currentTimeFrame);
            base.SwitchToPrevious();
            FillWorkWeek();
        }

        public override void SwitchToCurrent()
        {
            base.SwitchToCurrent();
            PageTitle = DateHelper.GetCalenderWeekFromDate(currentTimeFrame);
            FillWorkWeek();
        }

        public override void SwitchToNext()
        {
            currentTimeFrame = currentTimeFrame.AddDays(7);
            PageTitle = DateHelper.GetCalenderWeekFromDate(currentTimeFrame);
            base.SwitchToNext();
            FillWorkWeek();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            PageTitle = DateHelper.GetCalenderWeekFromDate(currentTimeFrame);
            FillWorkWeek();
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

        public string SummaryTotalHours
        {
            get
            {
                return summaryInfo != null ? ToHoursMinutesString(summaryInfo.TotalWorkHours) : string.Empty;
            }
        }

        public string SummaryTotalRequired
        {
            get { return ""; }
        }

        public string SummaryTotalAccounted
        {
            get { return ""; }
        }

        public string SummaryOvertime
        {
            get { return ""; }
        }

        #endregion
    }
}
