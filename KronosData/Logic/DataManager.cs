using KronosData.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace KronosData.Logic
{
    public class DataManager
    {
        private static readonly string savePath = @"C:\temp\test.json";
        private static readonly string accountPath = @"C:\temp\accounts.json";

        /// <summary>
        /// Creates a DataManager instance and loads the default user file
        /// </summary>
        public DataManager(bool useDefault = false)
        {
            if (useDefault)
            {
                CurrentUser = new User("default");
                Accounts = new ObservableCollection<Account>();
            }
            else
            {
                CurrentUser = DeserializeFromFile<User>(savePath);
                Accounts = DeserializeFromFile<ObservableCollection<Account>>(accountPath);
            }
        }

        public void SaveChanges()
        {
            SerializeToFile(savePath, CurrentUser);
            SerializeToFile(accountPath, Accounts);
        }

        public void SwitchUser(User newUser)
        {
            CurrentUser = newUser;
        }

        /// <summary>
        /// Serialize object to json file
        /// </summary>
        /// <param name="path">The path where to store the desired json file</param>
        public static void SerializeToFile(string path, object instance)
        {
            var serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented
            };

            using var sw = new StreamWriter(path);
            using var writer = new JsonTextWriter(sw);

            serializer.Serialize(writer, instance);
        }

        /// <summary>
        /// Deserialize an json file to an user object
        /// </summary>
        /// <param name="path">The path to the json file to deserialize</param>
        /// <returns>A User object or null in case of an error</returns>
        public static T DeserializeFromFile<T>(string path)
        {
            var json = File.ReadAllText(path);

            if (string.IsNullOrEmpty(json))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Gets all items of the desired day
        /// </summary>
        /// <param name="desiredDay">The day to lookup</param>
        /// <returns>All work items created at the given day</returns>
        public List<WorkItem> GetItemsOfDay(DateTime desiredDay)
        {
            if (CurrentUser == null)
            {
                return null;
            }

            var day = CurrentUser.AssignedWorkDays.Find(d => d.Date.Date.Equals(desiredDay.Date));

            if (day == null)
            {
                return null;
            }

            return day.AssignedWorkItems;
        }

        /// <summary>
        /// Gets all items of the desired month
        /// </summary>
        /// <param name="desiredMonth">The month to lookup</param>
        /// <returns>All work items created at the given month</returns>
        public List<WorkItem> GetItemsOfMonth(DateTime desiredMonth)
        {
            var retVal = new List<WorkItem>();

            foreach (var item in CurrentUser.AssignedWorkDays.Where(d => d.Date.Year == desiredMonth.Year && d.Date.Month == desiredMonth.Month))
            {
                retVal.AddRange(item.AssignedWorkItems);
            }

            return retVal;
        }

        /// <summary>
        /// Gets all items of the desired year
        /// </summary>
        /// <param name="desiredYear">The year to look up</param>
        /// <returns>All work items created at the given year</returns>
        public List<WorkItem> GetItemsOfYear(DateTime desiredYear)
        {
            var retVal = new List<WorkItem>();

            foreach (var item in CurrentUser.AssignedWorkDays.Where(d => d.Date.Year == desiredYear.Year))
            {
                retVal.AddRange(item.AssignedWorkItems);
            }

            return retVal;
        }

        /// <summary>
        /// Calculates the overtime of the given day
        /// </summary>
        /// <param name="desiredDay">The day to check the overtime of</param>
        /// <returns>The total overtime. Note that the overtime can be negative</returns>
        public TimeSpan GetOvertimeOfDay(DateTime desiredDay)
        {
            var day = CurrentUser.AssignedWorkDays.Find(d => d.Date.Date.Equals(desiredDay.Date));

            if (day == null)
            {
                return new TimeSpan(0);
            }

            return day.GetTotalWorkTime().Subtract(day.DailyWorkTime);
        }

        #region Properties

        /// <summary>
        /// The current user loaded into the data manager
        /// </summary>
        public User CurrentUser { get; private set; }

        public ObservableCollection<Account> Accounts { get; private set; }

        #endregion

    }
}
