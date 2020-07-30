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
        private static string LogType = "DecisionsLog";
        private static string MetricsType = "DecisionsMetrics";

        public static bool SendLog(LogzCredential connection, params LogData[] log)
        {
            try
            {
                var logData = log.Select((it) => new JsonLogData(it)).ToArray();
                var resp = PostRequest<LogzErrorResponse, JsonLogData>(connection, $"?token={connection.LogToken}&type={LogType}", logData);
                return true;
            } catch 
            {
                return false;
            }
        }

        public static bool SendMetrics(LogzCredential connection, params LogzMetricsData[] metris)
        {
            try
            {
                var resp = PostRequest<LogzErrorResponse, LogzMetricsData>(connection, $"?token={connection.MetricsToken}&type={MetricsType}", metris);
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
            public string LevelName { get; set; }
            public string Category { get; set; }
            public string Message { get; set; }
            public int ThreadId { get; set; }
            public string Exception { get; set; }
            public string SessionID { get; set; }
            public string Activity { get; set; }

            public JsonLogData(LogData log)
            {
                TimeStamp = log.TimeStamp;
                LevelName = log.LevelName;
                Category = log.Category ;
                Message = log.Message; 
                ThreadId = log.ThreadId;
                Exception = log.Exception?.ToString();
                SessionID = log.SessionID;
                Activity = log.Activity; 

                LogSeverity[] values = (LogSeverity[])Enum.GetValues(typeof(LogSeverity));
                var levels = new StringBuilder();
                foreach (var lvl in values)
                {
                    if (log.Level.HasFlag(lvl))
                    {
                        if (levels.Length > 0)
                            levels.Append(" ");
                        levels.Append(lvl.ToString());
                    }
                }
            }
        }


    }
}
