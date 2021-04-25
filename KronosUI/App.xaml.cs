﻿using KronosData.Logic;
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
            var dataManager = new DataManager(true);

            dataManager.SwitchUser(new User("simpsonho") { FirstName = "Homer", LastName = "Simpson" });

            dataManager.Accounts.Add(new Account("AC-123-456-01") { Title = "Research more C#" });
            dataManager.Accounts[0].AssignedTasks.Add(new WorkTask("Research LINQ"));
            dataManager.Accounts[0].AssignedTasks.Add(new WorkTask("Research multi-threading"));
            dataManager.Accounts.Add(new Account("AC-789-012-34") { Title = "Do chores" });
            dataManager.Accounts[1].AssignedTasks.Add(new WorkTask("Do the dishes"));
            dataManager.Accounts[1].AssignedTasks.Add(new WorkTask("Take out the trash"));

            var task1 = dataManager.Accounts[0].AssignedTasks[0];
            var task2 = dataManager.Accounts[0].AssignedTasks[1];

            var day1 = new WorkDay(new DateTime(2021, 4, 13), WorkDay.ShiftTypeEnum.None, WorkDay.DayTypeEnum.Default)
            {
                DailyWorkTime = new TimeSpan(8, 0, 0)
            };
            var day2 = new WorkDay(new DateTime(2021, 4, 14), WorkDay.ShiftTypeEnum.X_Shift, WorkDay.DayTypeEnum.HomeOffice);

            day1.AssignedWorkItems.Add(new WorkItem(new DateTime(2021, 4, 13, 7, 30, 0), new DateTime(2021, 4, 13, 12, 0, 0), task1));
            day1.AssignedWorkItems.Add(new WorkItem(new DateTime(2021, 4, 13, 12, 45, 0), new DateTime(2021, 4, 13, 17, 0, 0), task2));
            day2.AssignedWorkItems.Add(new WorkItem(new DateTime(2021, 4, 14, 8, 30, 0), new DateTime(2021, 4, 14, 12, 0, 0), task2));
            day2.AssignedWorkItems.Add(new WorkItem(new DateTime(2021, 4, 14, 12, 45, 0), new DateTime(2021, 4, 14, 17, 0, 0), task1));

            dataManager.CurrentUser.AssignedWorkDays.Add(day1);
            dataManager.CurrentUser.AssignedWorkDays.Add(day2);

            dataManager.SaveChanges();
        }

        private void Load()
        {
            var dataManager = new DataManager();
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
