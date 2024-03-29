﻿using KronosData.Logic;
using KronosData.Model;
using KronosUI.Controls;
using KronosUI.ViewModels;
using KronosUI.Views;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace KronosUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private static Mutex appMutex = null;

        public App()
        {
            if (!Directory.Exists(DataManager.AppDataPath))
            {
                Directory.CreateDirectory(DataManager.AppDataPath);
            }

            try
            {
                var datMan = new DataManager();
            }
            catch (FileNotFoundException)
            {
                InitializeDB();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            const string appName = "Kronos.KronosUI";
            bool createdNew;

            appMutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                //app is already running! Exiting the application
                PictoMsgBox.ShowMessage("Anwendungsstart abgebrochen!", "Die Anwendung läuft bereits!");
                Current.Shutdown();
            }

            base.OnStartup(e);
        }

#if DEBUG
        private void InitializeDB()
        {
            var dataManager = new DataManager(true);

            dataManager.SwitchUser(new User("simpsonho") { FirstName = "Homer", LastName = "Simpson" });

            dataManager.CurrentUser.UserSettings.DefaultDailyWorkTime = new TimeSpan(7, 0, 0);
            dataManager.CurrentUser.UserSettings.DefaultBeginOfWork = new TimeSpan(8, 0, 0);
            dataManager.CurrentUser.UserSettings.DefaultEndOfWork = new TimeSpan(16, 0, 0);
            dataManager.CurrentUser.UserSettings.DefaultBreakTime = new TimeSpan(0, 45, 0);

            dataManager.Accounts.Add(new Account("AC-123-456-01") { Title = "Research more C#" });
            dataManager.Accounts[0].AssignedTasks.Add(new WorkTask("Research LINQ", dataManager.Accounts[0], "T0001"));
            dataManager.Accounts[0].AssignedTasks.Add(new WorkTask("Research multi-threading", dataManager.Accounts[0], "T0002"));
            dataManager.Accounts.Add(new Account("AC-789-012-34") { Title = "Do chores" });
            dataManager.Accounts[1].AssignedTasks.Add(new WorkTask("Do the dishes", dataManager.Accounts[1], "T0012"));
            dataManager.Accounts[1].AssignedTasks.Add(new WorkTask("Take out the trash", dataManager.Accounts[1], "T0030"));

            var task1 = dataManager.Accounts[0].AssignedTasks[0];
            var task2 = dataManager.Accounts[0].AssignedTasks[1];
            var day = new DateTime(2021, 1, 1);

            for (int i = 0; i < 365; i++)
            {
                if (day.AddDays(i).DayOfWeek == DayOfWeek.Saturday || day.AddDays(i).DayOfWeek == DayOfWeek.Sunday)
                {
                    continue;
                }

                dataManager.CurrentUser.AssignedWorkDays.Add(CreateWorkDaySample(day.AddDays(i), task1, task2));
            }

            dataManager.SaveChanges();
        }

        private WorkDay CreateWorkDaySample(DateTime day, WorkTask task1, WorkTask task2)
        {
            var retVal = new WorkDay(day);
            retVal.WorkTime.Begin = new TimeSpan(8, 0, 0);
            retVal.WorkTime.End = new TimeSpan(17, 0, 0);
            retVal.BreakTime = new TimeSpan(0, 45, 0);

            retVal.AssignedWorkItems.Add(new WorkItem(new TimeSpan(4, 0, 0), task1));
            retVal.AssignedWorkItems.Add(new WorkItem(new TimeSpan(4, 15, 0), task2));

            return retVal;
        }
#else
        private void InitializeDB()
        {
            var dataManager = new DataManager(true);

            dataManager.SwitchUser(new User("default") { FirstName = "Default", LastName = "Default" });

            dataManager.CurrentUser.UserSettings.DefaultDailyWorkTime = new TimeSpan(7, 0, 0);
            dataManager.CurrentUser.UserSettings.DefaultBeginOfWork = new TimeSpan(8, 0, 0);
            dataManager.CurrentUser.UserSettings.DefaultEndOfWork = new TimeSpan(16, 0, 0);
            dataManager.CurrentUser.UserSettings.DefaultBreakTime = new TimeSpan(0, 45, 0);

            dataManager.Accounts.Add(new Account("0815") { Title = "Do some work!" });
            dataManager.Accounts[0].AssignedTasks.Add(new WorkTask("Did some work!", dataManager.Accounts[0], "T0001"));

            dataManager.SaveChanges();
        }
#endif

        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(new Shell());
            containerRegistry.RegisterInstance(new DataManager());
            containerRegistry.RegisterInstance(new WeekListingViewModel());
            containerRegistry.RegisterInstance(new MonthListingViewModel());
            containerRegistry.RegisterInstance(new ConfigurationViewModel());
        }
    }
}
