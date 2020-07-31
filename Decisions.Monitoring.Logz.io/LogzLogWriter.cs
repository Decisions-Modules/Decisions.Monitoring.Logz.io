using Decisions.Monitoring.Logz.io.Utility;
using DecisionsFramework;
using DecisionsFramework.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decisions.Monitoring.Logz.io
{
    public class LogzLogWriter : LogzBaseWriter, ILogWriter, IInitializable
    {
        public void Initialize()
        {
            Log.AddLogWriter(this);
        }

        public void Write(LogData log)
        {
            LogzApi.SendLog(Credentials, log);
        }
    }
}
