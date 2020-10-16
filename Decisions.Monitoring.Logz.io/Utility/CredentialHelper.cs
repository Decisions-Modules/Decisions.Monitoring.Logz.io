using Decisions.Monitoring.Logz.io.Data;
using DecisionsFramework.ServiceLayer;

namespace Decisions.Monitoring.Logz.io.Utility
{
    internal static class CredentialHelper
    {
        private const string metricsTokenValue = "put metrics token here";
        public static LogzCredential Credentials
        {
            get
            {
                var settings = ModuleSettingsAccessor<LogzSettings>.GetSettings();
                return new LogzCredential
                {
                    BaseUrl = settings.BaseUrl,
                    LogToken = settings.SendLogs? settings.LogToken : "",
                    MetricsToken = metricsTokenValue
                };
            }
        }
    }
}