using Newtonsoft.Json;
using System;

namespace KronosData.Model
{
    public class DateUnit
    {
        public DateUnit()
        {
            Date = DateTime.Now;
            Begin = new TimeSpan(0);
            End = new TimeSpan(0);
        }

        #region Properties

        /// <summary>
        /// The date of the DateUnit
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The time where the DateUnit started
        /// </summary>
        public TimeSpan Begin { get; set; }

        /// <summary>
        /// The time where the DateUnit ended
        /// </summary>
        public TimeSpan End { get; set; }

        /// <summary>
        /// The duration of the DateUnit
        /// </summary>
        [JsonIgnore]
        public TimeSpan Duration { get { return End - Begin; } }

        #endregion
    }
}
