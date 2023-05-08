using System;
using System.Globalization;
using System.IO;
using System.Net;

namespace KronosData.Logic
{
    public class MappingUploader
    {
        private static readonly string RequestMask = "{0}/set.php?userHash={1}&month={2}&year={3}&subtask={4}&hours={5:0.0#}";
        private static readonly string TransactionSuccess = "";

        private string token;
        private string url;

        public MappingUploader(string url, string token)
        {
            this.token = token;
            this.url = url;
        }

        public bool UploadTask(string task, TimeSpan hoursWorked, DateTime currentTimeFrame)
        {
            var response = HttpGet(CreateRequest(currentTimeFrame.Month, currentTimeFrame.Year, task, hoursWorked));

            return response != TransactionSuccess;
        }

        private string CreateRequest(int month, int year, string taskId, TimeSpan hoursWorked)
        {
            var duration = hoursWorked.TotalHours.ToString("0.0#", new NumberFormatInfo() { NumberDecimalSeparator = "." });

            return string.Format(RequestMask, url, token, month, year, taskId, duration);
        }

        private string HttpGet(string url)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
