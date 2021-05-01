using KronosData.Logic;
using KronosData.Model;
using KronosUI.ViewModels;
using KronosUI.Views;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.IO;
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
            Setup();
        }

        private void Save()
        {
            var dataManager = new DataManager(true);

            dataManager.SwitchUser(new User("simpsonho") { FirstName = "Homer", LastName = "Simpson" });

            dataManager.CurrentUser.UserSettings.DefaultDailyWorkTime = new TimeSpan(7, 0, 0);
            dataManager.CurrentUser.UserSettings.DefaultBeginOfWork = new TimeSpan(8, 0, 0);
            dataManager.CurrentUser.UserSettings.DefaultEndOfWork = new TimeSpan(16, 0, 0);

            dataManager.Accounts.Add(new Account("AC-123-456-01") { Title = "Research more C#" });
            dataManager.Accounts[0].AssignedTasks.Add(new WorkTask("Research LINQ", dataManager.Accounts[0]));
            dataManager.Accounts[0].AssignedTasks.Add(new WorkTask("Research multi-threading", dataManager.Accounts[0]));
            dataManager.Accounts.Add(new Account("AC-789-012-34") { Title = "Do chores" });
            dataManager.Accounts[1].AssignedTasks.Add(new WorkTask("Do the dishes", dataManager.Accounts[1]));
            dataManager.Accounts[1].AssignedTasks.Add(new WorkTask("Take out the trash", dataManager.Accounts[1]));

            var task1 = dataManager.Accounts[0].AssignedTasks[0];
            var task2 = dataManager.Accounts[0].AssignedTasks[1];

            var day1 = new WorkDay(WorkDay.ShiftTypeEnum.None, WorkDay.DayTypeEnum.Default)
            {
                DailyWorkTime = new TimeSpan(8, 0, 0),
            };
            day1.WorkTime.Date = new DateTime(2021, 4, 13);
            day1.WorkTime.Begin = new TimeSpan(8, 0, 0);
            day1.WorkTime.End = new TimeSpan(17, 0, 0);
            day1.BreakTime = new TimeSpan(0, 45, 0);

            var day2 = new WorkDay(WorkDay.ShiftTypeEnum.X_Shift, WorkDay.DayTypeEnum.HomeOffice);
            day2.WorkTime.Date = new DateTime(2021, 4, 14);
            day2.WorkTime.Begin = new TimeSpan(8, 0, 0);
            day2.WorkTime.End = new TimeSpan(17, 0, 0);
            day2.BreakTime = new TimeSpan(0, 45, 0);

            day1.AssignedWorkItems.Add(new WorkItem(new TimeSpan(4, 30, 0), task1));
            day1.AssignedWorkItems.Add(new WorkItem(new TimeSpan(4, 15, 0), task2));
            day2.AssignedWorkItems.Add(new WorkItem(new TimeSpan(4, 30, 0), task2));
            day2.AssignedWorkItems.Add(new WorkItem(new TimeSpan(4, 15, 0), task1));

            dataManager.CurrentUser.AssignedWorkDays.Add(day1);
            dataManager.CurrentUser.AssignedWorkDays.Add(day2);

            dataManager.SaveChanges();
        }

        private void Load()
        {
            var dataManager = new DataManager();
        }

        private void Setup()
        {
            if (!Directory.Exists(DataManager.AppDataPath))
            {
                Directory.CreateDirectory(DataManager.AppDataPath);
            }

            //TODO: Remove
            try
            {
                var datMan = new DataManager();
            }
            catch (FileNotFoundException)
            {
                Save();
            }
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(new Shell());
            containerRegistry.RegisterInstance(new DataManager());
            containerRegistry.RegisterInstance(new WeekListingViewModel());
            containerRegistry.RegisterInstance(new ConfigurationViewModel());
        }
    }
}
