using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosData.Logic
{
    public class MappingUploader
    {
        private static readonly string RequestMask = "{0}/set.php?userHash={1}&month={2}&year={3}&subtask={4}&hours={5:0.0#}";

        private string token;
        private string url;

        public MappingUploader(string url, string token)
        {
            this.token = token;
            this.url = url;
        }

        public bool UploadTask(string task, TimeSpan hoursWorked, DateTime currentTimeFrame)
        {
            var req = CreateRequest(currentTimeFrame.Month, currentTimeFrame.Year, task, hoursWorked);

            //TODO: SEND HTTP-REQUEST

            return false;
        }

        private string CreateRequest(int month, int year, string taskId, TimeSpan hoursWorked)
        {
            var duration = hoursWorked.TotalHours.ToString("0.0#", new NumberFormatInfo() { NumberDecimalSeparator = "." });

            return string.Format(RequestMask, url, token, month, year, taskId, duration);
        }
    }
}
