using KronosData.Logic;
using KronosData.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KronosUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Test();
        }

        private void Test()
        {
            var user = new User() { FirstName = "Homer", LastName = "Simpson", UserName = "simpsonho" };
            var account = new Account() { Number = "AC-123-456-01", Title = "Research more C#" };
            var task1 = new WorkTask() { Title = "Research LINQ", AssignedAccount = account };
            var task2 = new WorkTask() { Title = "Research multi-threading", AssignedAccount = account };
            var item1 = new WorkItem(new DateTime(2021, 4, 13, 8, 30, 0), new DateTime(2021, 4, 13, 12, 0, 0)) { AssignedWorkTask = task1 };
            var item2 = new WorkItem(new DateTime(2021, 4, 13, 12, 45, 0), new DateTime(2021, 4, 13, 17, 0, 0)) { AssignedWorkTask = task2 };
            var item3 = new WorkItem(new DateTime(2021, 4, 14, 8, 30, 0), new DateTime(2021, 4, 14, 12, 0, 0)) { AssignedWorkTask = task2};
            var item4 = new WorkItem(new DateTime(2021, 4, 14, 12, 45, 0), new DateTime(2021, 4, 14, 17, 0, 0)) { AssignedWorkTask = task1 };

            user.AssignedWorkItems.Add(item1);
            user.AssignedWorkItems.Add(item2);
            user.AssignedWorkItems.Add(item3);
            user.AssignedWorkItems.Add(item4);

            var results1 = user.GetItemsOfDay(new DateTime(2021, 4, 13));
            var results2 = user.GetItemsOfDay(new DateTime(2021, 4, 14));

        }
    }
}
