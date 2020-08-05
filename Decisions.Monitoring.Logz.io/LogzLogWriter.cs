using Decisions.Monitoring.Logz.io.Utility;
using DecisionsFramework;
using DecisionsFramework.ServiceLayer;
using DecisionsFramework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decisions.Monitoring.Logz.io
{
    public class LogzLogWriter : ILogWriter, IInitializable
    {
        private LogSendingThreadJob logSendingJob = new LogSendingThreadJob();
        public void Initialize()
        {
            Log.AddLogWriter(this);
        }

        public void Write(LogData log)
        {
            logSendingJob.AddItem(log);
        }
    }

    internal class LogSendingThreadJob : DataSendingThreadJob<LogData>
    {
        public LogSendingThreadJob() : base("Decisions.Logz log queue", TimeSpan.FromSeconds(10))
        { }

        protected override void SendData(LogData[] logs)
        {
            LogzApi.SendLog(CredentialHelper.Credentials, logs);
        }
    }

}
