using KronosData.Logic;
using KronosData.Model;
using KronosUI.Views;
using Prism.Ioc;
using Prism.Unity;
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
    public partial class App : PrismApplication
    {
        /*public App()
        {
            Test();
        }

        private void Test()
        {
            var user = new User("simpsonho") { FirstName = "Homer", LastName = "Simpson" };
            var account = new Account("AC-123-456-01") { Title = "Research more C#" };
            var task1 = new WorkTask(account) { Title = "Research LINQ" };
            var task2 = new WorkTask(account) { Title = "Research multi-threading" };
            var item1 = new WorkItem(new DateTime(2021, 4, 13, 8, 30, 0), new DateTime(2021, 4, 13, 12, 0, 0), task1);
            var item2 = new WorkItem(new DateTime(2021, 4, 13, 12, 45, 0), new DateTime(2021, 4, 13, 17, 0, 0), task2);
            var item3 = new WorkItem(new DateTime(2021, 4, 14, 8, 30, 0), new DateTime(2021, 4, 14, 12, 0, 0), task2);
            var item4 = new WorkItem(new DateTime(2021, 4, 14, 12, 45, 0), new DateTime(2021, 4, 14, 17, 0, 0), task1);

            user.AssignedWorkItems.Add(item1);
            user.AssignedWorkItems.Add(item2);
            user.AssignedWorkItems.Add(item3);
            user.AssignedWorkItems.Add(item4);

            var results1 = user.GetItemsOfDay(new DateTime(2021, 4, 13));
            var results2 = user.GetItemsOfDay(new DateTime(2021, 4, 14));

            //user.SerializeToFile(@"C:\temp\test.json");
            var user2 = User.DeserializeFromFile(@"C:\temp\test.json");
        }*/

        protected override Window CreateShell()
        {
            var w = Container.Resolve<Shell>();

            return w;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //throw new NotImplementedException();
        }
    }
}
