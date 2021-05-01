using System;

namespace KronosData.Model
{
    public class Settings
    {
        public Settings()
        {
            DefaultDailyWorkTime = new TimeSpan(0);
            DefaultBeginOfWork = new TimeSpan(0);
            DefaultEndOfWork = new TimeSpan(0);
        }

        #region Properties

        public TimeSpan DefaultDailyWorkTime { get; set; }

        public TimeSpan DefaultBeginOfWork { get; set; }

        public TimeSpan DefaultEndOfWork { get; set; }

        #endregion
    }
}
