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
        public static readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\KronosData";

        private static readonly string savePath = AppDataPath + @"\userData.json";
        private static readonly string accountPath = AppDataPath + @"\accountData.json";

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
                LoadFromFile();
            }
        }

        /// <summary>
        /// Saves pending changes to the corresponding files
        /// </summary>
        public void SaveChanges()
        {
            SerializeToFile(savePath, CurrentUser);
            SerializeToFile(accountPath, Accounts);
        }

        /// <summary>
        /// Load the data from files
        /// </summary>
        public void LoadFromFile()
        {
            CurrentUser = DeserializeFromFile<User>(savePath);
            Accounts = DeserializeFromFile<ObservableCollection<Account>>(accountPath);
        }

        /// <summary>
        /// Switch to another user
        /// </summary>
        /// <param name="newUser">The new user to user</param>
        public void SwitchUser(User newUser)
        {
            CurrentUser = newUser;
        }

        /// <summary>
        /// Serialize object to json file
        /// </summary>
        /// <param name="path">The path where to store the desired json file</param>
        /// <param name="instance">The object to serialize</param>
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
        /// <typeparam name="T">The type of object to deserialize into</typeparam>
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
        public ObservableCollection<WorkItem> GetItemsOfDay(DateTime desiredDay)
        {
            if (CurrentUser == null)
            {
                return null;
            }

            var day = CurrentUser.AssignedWorkDays.Where(d => d.WorkTime.DateOfWork.Date.Equals(desiredDay.Date)).FirstOrDefault();

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

            foreach (var item in CurrentUser.AssignedWorkDays.Where(d => d.WorkTime.DateOfWork.Year == desiredMonth.Year && d.WorkTime.DateOfWork.Month == desiredMonth.Month))
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

            foreach (var item in CurrentUser.AssignedWorkDays.Where(d => d.WorkTime.DateOfWork.Year == desiredYear.Year))
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
            var day = CurrentUser.AssignedWorkDays.Where(d => d.WorkTime.DateOfWork.Date.Equals(desiredDay.Date)).FirstOrDefault();

            if (day == null)
            {
                return new TimeSpan(0);
            }

            return day.TotalWorkTime.Subtract(day.DailyWorkTime);
        }

        /// <summary>
        /// Checks whether an account is still in use
        /// </summary>
        /// <param name="account">The account to check</param>
        /// <returns>True, if the account is still in use</returns>
        public bool IsAccountInUse(Account account)
        {
            foreach (var item in CurrentUser.AssignedWorkDays)
            {
                if (item.AssignedWorkItems.Where(d => d.AssignedWorkTask.AssignedAccountNumber.Equals(account.Number)).Any())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks whether a task is still in use
        /// </summary>
        /// <param name="task">The task to check</param>
        /// <returns>True, if the task is still in use</returns>
        public bool IsTaskInUse(WorkTask task)
        {
            foreach (var item in CurrentUser.AssignedWorkDays)
            {
                if (item.AssignedWorkItems.Where(d => d.AssignedWorkTask.Equals(task)).Any())
                {
                    return true;
                }
            }

            return false;
        }

        public Account FindCorrespondingAccount(WorkTask task)
        {
            return Accounts.Where(d => d.AssignedTasks.Contains(task)).FirstOrDefault();
        }

        #region Properties

        /// <summary>
        /// The current user loaded into the data manager
        /// </summary>
        public User CurrentUser { get; private set; }

        /// <summary>
        /// The accounts currently stored in the accounts file
        /// </summary>
        public ObservableCollection<Account> Accounts { get; private set; }

        #endregion
    }
}
