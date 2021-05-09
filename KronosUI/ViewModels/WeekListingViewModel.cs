using KronosData.Logic;
using KronosData.Model;
using KronosUI.Controls;
using Prism.Commands;
using Prism.Ioc;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace KronosUI.ViewModels
{
    public class WeekListingViewModel : ListingViewModelBase
    {
        private ObservableCollection<WorkDay> currentWorkWeek;
        private WorkDay currentWorkDay;
        private bool pendingChanges;

        private readonly DataManager dataManager;

        public WeekListingViewModel()
        {
            dataManager = ContainerLocator.Container.Resolve<DataManager>();

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
            var wDay = new WorkDay(0, 0);
            wDay.WorkTime.Date = CalcDayOfWeek(currentTimeFrame, dow);

            var dayToAdd = dataManager.CurrentUser.AssignedWorkDays.FirstOrDefault(d => d.WorkTime.Date.Date.Equals(wDay.WorkTime.Date.Date)) ?? wDay;
            CurrentWorkWeek.Add(dayToAdd);

            if (dayToAdd.WorkTime.Date.Date.Equals(DateTime.Now.Date))
            {
                CurrentWorkDay = dayToAdd;
            }
        }

        private static DateTime CalcDayOfWeek(DateTime val, DayOfWeek reqDay)
        {
            return val.AddDays((int)reqDay - (val.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)val.DayOfWeek));
        }

        #region Command implementations

        private void InitializeCommands()
        {
            EditItemCommand = new DelegateCommand(EditItem, CanEditItem);
            RevokeChangesCommand = new DelegateCommand(RevokeChanges, CanRevokeChanges);
            SaveChangesCommand = new DelegateCommand(SaveChanges, CanSaveChanges);
        }

        public void EditItem()
        {
            if ((bool)new WorkDayEditor(CurrentWorkDay).ShowDialog())
            {
                PendingChanges = true;
            }
        }

        public bool CanEditItem()
        {
            return CurrentWorkDay != null;
        }

        public void RevokeChanges()
        {
            PendingChanges = false;
        }

        public bool CanRevokeChanges()
        {
            return PendingChanges;
        }

        public void SaveChanges()
        {
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

        #endregion

        #region Properties

        public DelegateCommand EditItemCommand { get; private set; }

        public DelegateCommand RevokeChangesCommand { get; private set; }

        public DelegateCommand SaveChangesCommand { get; private set; }

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
