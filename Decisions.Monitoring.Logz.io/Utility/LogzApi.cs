using Decisions.Monitoring.Logz.io.Data;
using DecisionsFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decisions.Monitoring.Logz.io.Utility
{
    public static partial class LogzApi
    {
        private static readonly string logType = "DecisionsLog";
        private static readonly string metricsType = "DecisionsMetrics";

        public static bool SendLog(LogzCredential connection, params LogData[] logs)
        {
            if (connection == null || string.IsNullOrEmpty(connection.LogToken) || string.IsNullOrEmpty(connection.BaseUrl))
                return false;
            try
            {
                var logData = logs.Select((it) => new JsonLogData(it)).ToArray();
                var resp = PostRequest<Object, JsonLogData>(connection, $"?token={connection.LogToken}&type={LogzApi.logType}", logData);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SendMetrics(LogzCredential connection, params LogzMetricsData[] metris)
        {
            if (connection == null || string.IsNullOrEmpty(connection.MetricsToken) || string.IsNullOrEmpty(connection.BaseUrl))
                return false;

            try
            {
                var resp = PostRequest<Object, LogzMetricsData>(connection, $"?token={connection.MetricsToken}&type={LogzApi.metricsType}", metris);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private class JsonLogData
        {
            public DateTime TimeStamp { get; set; }
            public string Level { get; set; }
            public string Category { get; set; }
            public string Message { get; set; }
            public int ThreadId { get; set; }
            public string Exception { get; set; }
            public string SessionID { get; set; }
            public string Activity { get; set; }

            public JsonLogData(LogData log)
            {
                TimeStamp = log.TimeStamp;
                Level = log.LevelName;
                Category = log.Category;
                Message = log.Message;
                ThreadId = log.ThreadId;
                Exception = log.Exception?.ToString();
                SessionID = log.SessionID;
                Activity = log.Activity;
            }
        }
    }
}
