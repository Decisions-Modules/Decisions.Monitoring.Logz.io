using Decisions.Monitoring.Logz.io.Data;
using DecisionsFramework.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decisions.Monitoring.Logz.io
{
    public abstract class LogzBaseWriter
    {
        protected LogzCredential Credentials
        {
            get
            {
                LogzSettings settings = ModuleSettingsAccessor<LogzSettings>.GetSettings();
                return new LogzCredential()
                {
                    BaseUrl = settings.BaseUrl,
                    LogToken = settings.LogToken,
                    MetricsToken = settings.MetricsToken
                };
            }
        }
    }
}
