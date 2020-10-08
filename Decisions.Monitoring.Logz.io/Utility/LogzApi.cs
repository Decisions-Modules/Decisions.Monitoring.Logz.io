using System;
using System.Linq;
using Decisions.Monitoring.Logz.io.Data;
using DecisionsFramework;

namespace Decisions.Monitoring.Logz.io.Utility
{
    public static partial class LogzApi
    {
        private static readonly string logType = "DecisionsLog";
        private static readonly string metricsType = "DecisionsMetrics";

        public static bool SendLog(LogzCredential connection, params LogData[] logs)
        {
            if (connection == null || string.IsNullOrEmpty(connection.LogToken) ||
                string.IsNullOrEmpty(connection.BaseUrl))
                return false;
            try
            {
                var logData = logs.Select(it => new JsonLogData(it)).ToArray();
                var resp = PostRequest<object, JsonLogData>(connection, $"?token={connection.LogToken}&type={logType}",
                    logData);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SendMetrics(LogzCredential connection, params LogzMetricsData[] metris)
        {
            if (connection == null || string.IsNullOrEmpty(connection.MetricsToken) ||
                string.IsNullOrEmpty(connection.BaseUrl))
                return false;

            try
            {
                var resp = PostRequest<object, LogzMetricsData>(connection,
                    $"?token={connection.MetricsToken}&type={metricsType}", metris);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private class JsonLogData
        {
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

            public DateTime TimeStamp { get; }
            public string Level { get; }
            public string Category { get; }
            public string Message { get; }
            public int ThreadId { get; }
            public string Exception { get; }
            public string SessionID { get; }
            public string Activity { get; }
        }
    }
}