using System;

namespace KronosData.Model
{
    public class DateUnit
    {
        public DateUnit()
        {
            Begin = DateTime.Now;
            End = DateTime.Now;
        }

        #region Properties

        public DateTime Begin { get; set; }

        public DateTime End { get; set; }

        public TimeSpan Duration { get { return End - Begin; } }

        #endregion
    }
}
