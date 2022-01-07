using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosData.Model
{
    public class SummaryInfo
    {
        public TimeSpan TotalWorkHours { get; set; }

        public TimeSpan RequiredWorkHours { get; set; }

        public TimeSpan TotalOvertime { get; set; }

        public TimeSpan TotalAccountedHours { get; set; }
    }
}
