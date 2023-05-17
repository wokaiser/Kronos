using System;
using System.Globalization;
using System.Net;
using System.Net.Http;

namespace KronosData.Logic
{
    public class MappingUploader
    {
        private static readonly string RequestMask = "set.php?userHash={0}&month={1}&year={2}&subtask={3}&hours={4:0.0#}";
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
            var response = HttpGet(url, CreateRequest(currentTimeFrame.Month, currentTimeFrame.Year, task, hoursWorked));

            return response != TransactionSuccess;
        }

        private string CreateRequest(int month, int year, string taskId, TimeSpan hoursWorked)
        {
            var duration = hoursWorked.TotalHours.ToString("0.0#", new NumberFormatInfo() { NumberDecimalSeparator = "." });

            return string.Format(RequestMask, url, token, month, year, taskId, duration);
        }

        private string HttpGet(string url, string request)
        {
            try
            {
                var httpClient = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate })
                {
                    BaseAddress = new Uri(url)
                };
                var response = httpClient.GetAsync(request).Result;
                response.EnsureSuccessStatusCode();

                return response.Content.ReadAsStringAsync().Result;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
