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
        private Buffer<LogData> buffer;
        public void Initialize()
        {
            buffer = new Buffer<LogData>("Decisions.Logz sending logs", TimeSpan.FromSeconds(10), SendLogs);
            Log.AddLogWriter(this);
        }

        public void Write(LogData log)
        {
            buffer.AddData(log);
        }

        private void SendLogs(LogData[] logs)
        {
            LogzApi.SendLog(Credentials, logs);
        }
    }
}
