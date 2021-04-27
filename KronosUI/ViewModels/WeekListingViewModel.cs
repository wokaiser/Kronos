using KronosData.Logic;
using KronosData.Model;
using Prism.Ioc;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace KronosUI.ViewModels
{
    public class WeekListingViewModel : ListingViewModelBase
    {
        private ObservableCollection<WorkDay> currentWorkWeek;

        private readonly DataManager dataManager;

        public WeekListingViewModel()
        {
            dataManager = ContainerLocator.Container.Resolve<DataManager>();

            FillWorkWeek();
        }

        private void FillWorkWeek()
        {
            CurrentWorkWeek = new ObservableCollection<WorkDay>();

            for (int i = 1; i < 6; i++)
            {
                AddWorkDay((DayOfWeek)i);
            }
        }

        private void AddWorkDay(DayOfWeek dow)
        {
            var wDay = new WorkDay(0, 0);
            wDay.WorkTime.Begin = CalcDayOfWeek(currentTimeFrame, dow);
            wDay.WorkTime.End = wDay.WorkTime.Begin;

            CurrentWorkWeek.Add(dataManager.CurrentUser.AssignedWorkDays.FirstOrDefault(d => d.WorkTime.Begin.Date.Equals(wDay.WorkTime.Begin.Date)) ?? wDay);            
        }

        private static DateTime CalcDayOfWeek(DateTime val, DayOfWeek reqDay)
        {
            return val.AddDays(reqDay - val.DayOfWeek);
        }

        #region Inherited method implementation and overrides

        protected override void Initialize()
        {
            CalendarValue = DateHelper.GetCalenderWeekFromDate(currentTimeFrame);
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

        public ObservableCollection<WorkDay> CurrentWorkWeek
        {
            get { return currentWorkWeek; }
            set
            {
                SetProperty(ref currentWorkWeek, value);
            }
        }

        #endregion
    }
}
