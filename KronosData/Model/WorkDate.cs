using Newtonsoft.Json;
using System;

namespace KronosData.Model
{
    public class WorkDate
    {
        public WorkDate()
        {
            DateOfWork = DateTime.Now;
            Begin = new TimeSpan(0);
            End = new TimeSpan(0);
        }

        public WorkDate(DateTime date)
        {
            DateOfWork = date;
            Begin = new TimeSpan(0);
            End = new TimeSpan(0);
        }

        #region Properties

        /// <summary>
        /// The date of the WorkDate
        /// </summary>
        public DateTime DateOfWork { get; set; }

        /// <summary>
        /// The time where the WorkDate started
        /// </summary>
        public TimeSpan Begin { get; set; }

        /// <summary>
        /// The time where the WorkDate ended
        /// </summary>
        public TimeSpan End { get; set; }

        /// <summary>
        /// The duration of the WorkDate
        /// </summary>
        [JsonIgnore]
        public TimeSpan Duration { get { return End - Begin; } }

        #endregion
    }
}
