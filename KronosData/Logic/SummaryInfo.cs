using System;

namespace KronosData.Logic
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

        public TimeSpan TotalWorkHours { get; }

        public TimeSpan RequiredWorkHours { get; }

        public TimeSpan TotalOvertime { get; }

        public TimeSpan TotalAccountedHours { get; }
    }
}
