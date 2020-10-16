using Decisions.Monitoring.Logz.io.Data;
using DecisionsFramework.ServiceLayer;

namespace Decisions.Monitoring.Logz.io.Utility
{
    internal static class CredentialHelper
    {
        public static LogzCredential Credentials
        {
            get
            {
                var settings = LogzSettings.Instance();
                return new LogzCredential
                {
                    BaseUrl = settings.BaseUrl,
                    LogToken = settings.LogToken ?? "",
                    MetricsToken = settings.SendMetrics ? settings.MetricsToken : ""
                };
            }
        }
    }
}