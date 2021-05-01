using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace KronosData.Model
{
    public class User : DB_Access
    {
        /// <summary>
        /// Creates a new user object
        /// </summary>
        /// <param name="username">The username for the user</param>
        public User(string username)
        {
            UserName = username;
            FirstName = string.Empty;
            LastName = string.Empty;
            AssignedWorkDays = new ObservableCollection<WorkDay>();
            UserSettings = new Settings();
        }

        #region Overrides

        public override string ToString()
        {
            return string.Format("{0}: {1}, {2}", UserName, LastName, FirstName);
        }

        public override bool Equals(object obj)
        {
            return obj is User user && UserName.Equals(user.UserName);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserName);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The assigned username by the system
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// The first name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The work items assigned to the user
        /// </summary>
        public ObservableCollection<WorkDay> AssignedWorkDays { get; }

        /// <summary>
        /// The settings for the user
        /// </summary>
        [JsonProperty]
        public Settings UserSettings { get; private set; }

        #endregion

    }
}
