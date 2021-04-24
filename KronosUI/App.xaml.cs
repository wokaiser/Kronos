using KronosData.Logic;
using KronosData.Model;
using KronosUI.ViewModels;
using KronosUI.Views;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.Windows;

namespace KronosUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public App()
        {
            //Save();
            //Load();
        }

        private void Save()
        {
            var user = new User("simpsonho") { FirstName = "Homer", LastName = "Simpson" };

            var account = new Account("AC-123-456-01") { Title = "Research more C#" };

            var task1 = new WorkTask(account) { Title = "Research LINQ" };
            var task2 = new WorkTask(account) { Title = "Research multi-threading" };

            var day1 = new WorkDay(new DateTime(2021, 4, 13), WorkDay.ShiftTypeEnum.None, WorkDay.DayTypeEnum.Default);
            day1.DailyWorkTime = new TimeSpan(8, 0, 0);
            var day2 = new WorkDay(new DateTime(2021, 4, 14), WorkDay.ShiftTypeEnum.X_Shift, WorkDay.DayTypeEnum.HomeOffice);
            day2.DailyWorkTime = new TimeSpan(8, 0, 0);

            day1.AssignedWorkItems.Add(new WorkItem(new DateTime(2021, 4, 13, 7, 30, 0), new DateTime(2021, 4, 13, 12, 0, 0), task1));
            day1.AssignedWorkItems.Add(new WorkItem(new DateTime(2021, 4, 13, 12, 45, 0), new DateTime(2021, 4, 13, 17, 0, 0), task2));
            day2.AssignedWorkItems.Add(new WorkItem(new DateTime(2021, 4, 14, 8, 30, 0), new DateTime(2021, 4, 14, 12, 0, 0), task2));
            day2.AssignedWorkItems.Add(new WorkItem(new DateTime(2021, 4, 14, 12, 45, 0), new DateTime(2021, 4, 14, 17, 0, 0), task1));

            user.AssignedWorkDays.Add(day1);
            user.AssignedWorkDays.Add(day2);

            user.SerializeToFile(@"C:\temp\test.json");
        }

        private void Load()
        {
            var datMan1 = new DataManager();
            var ret1 = datMan1.GetAllAccounts();
            var ret2 = datMan1.GetAllTasks();
            var t1 = datMan1.CurrentUser.AssignedWorkDays[0].GetTotalWorkTime();
            var t2 = datMan1.CurrentUser.AssignedWorkDays[1].GetTotalWorkTime();
            var t3 = datMan1.GetOvertimeOfDay(new DateTime(2021, 4, 13));
            var t4 = datMan1.GetOvertimeOfDay(new DateTime(2021, 4, 14));
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(new DataManager());
            containerRegistry.RegisterInstance(new WeekListingViewModel());
            containerRegistry.RegisterInstance(new ConfigurationViewModel());
        }
    }
}
