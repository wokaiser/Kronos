using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KronosUI.ViewModels
{
    public class WeekListingViewModel : BindableBase
    {
        private string calendarWeek;

        public WeekListingViewModel()
        {
            CalendarWeek = "KW" + (DateTime.Now.DayOfYear / 7).ToString();

            PopulateCommands();
        }

        #region Command functions

        private void PopulateCommands()
        {
            SwitchToPreviousWeekCommand = new DelegateCommand(SwitchToPreviousWeek, CanSwitchToPreviousWeek);
            SwitchToCurrentWeekCommand = new DelegateCommand(SwitchToCurrentWeek, CanSwitchToCurrentWeek);
            SwitchToNextWeekCommand = new DelegateCommand(SwitchToNextWeek, CanSwitchToNextWeek);
        }

        public void SwitchToPreviousWeek()
        {
            MessageBox.Show("Previous");
        }

        public void SwitchToCurrentWeek()
        {
            MessageBox.Show("Current");
        }

        public void SwitchToNextWeek()
        {
            MessageBox.Show("Next");
        }

        public bool CanSwitchToPreviousWeek()
        {
            return true;
        }

        public bool CanSwitchToCurrentWeek()
        {
            return true;
        }

        public bool CanSwitchToNextWeek()
        {
            return true;
        }

        #endregion

        #region Properties

        public string CalendarWeek
        {
            get { return calendarWeek; }
            set { SetProperty(ref calendarWeek, value); }
        }

        public DelegateCommand SwitchToPreviousWeekCommand { get; private set; }

        public DelegateCommand SwitchToCurrentWeekCommand { get; private set; }

        public DelegateCommand SwitchToNextWeekCommand { get; private set; }

        #endregion
    }
}
