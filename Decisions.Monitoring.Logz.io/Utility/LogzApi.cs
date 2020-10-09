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

        public static bool SendLog(LogzCredential connection, params LogzLogData[] logs)
        {
            if (connection == null || string.IsNullOrEmpty(connection.LogToken) ||
                string.IsNullOrEmpty(connection.BaseUrl))
                return false;
            try
            {
                var resp = PostRequest<Object, LogzLogData>(connection, $"?token={connection.LogToken}&type={LogzApi.logType}", logs);
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

    }
}