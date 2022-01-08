using System;

namespace KronosData.Model
{
    public class SummaryInfo
    {
        public SummaryInfo(TimeSpan twh, TimeSpan rwh, TimeSpan toh, TimeSpan tah)
        {
            TotalWorkHours = twh;
            RequiredWorkHours = rwh;
            TotalOvertime = toh;
            TotalAccountedHours = tah;
        }

        public static SummaryInfo Zero
        {
            get
            {
                return new SummaryInfo(new TimeSpan(0), new TimeSpan(0), new TimeSpan(0), new TimeSpan(0));
            }
        }

        public TimeSpan TotalWorkHours { get; }

        public TimeSpan RequiredWorkHours { get; }

        public TimeSpan TotalOvertime { get; }

        public TimeSpan TotalAccountedHours { get; }
    }
}
