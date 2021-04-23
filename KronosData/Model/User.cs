using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

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
            AssignedWorkDays = new List<WorkDay>();
        }
        
        /// <summary>
        /// Serialize object to json file
        /// </summary>
        /// <param name="path">The path where to store the desired json file</param>
        public void SerializeToFile(string path)
        {
            var serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented
            };

            using var sw = new StreamWriter(path);
            using var writer = new JsonTextWriter(sw);
            serializer.Serialize(writer, this);
        }

        /// <summary>
        /// Deserialize an json file to an user object
        /// </summary>
        /// <param name="path">The path to the json file to deserialize</param>
        /// <returns>A User object or null in case of an error</returns>
        public static User DeserializeFromFile(string path)
        {
            var json = File.ReadAllText(path);

            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<User>(json);
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
        public List<WorkDay> AssignedWorkDays { get; }

        #endregion

    }
}
