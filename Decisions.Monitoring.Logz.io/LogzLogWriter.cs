using System;
using Decisions.Monitoring.Logz.io.Utility;
using DecisionsFramework;
using DecisionsFramework.ServiceLayer;

namespace Decisions.Monitoring.Logz.io
{
    public class LogzLogWriter : ILogWriter, IInitializable
    {
        private readonly LogSendingThreadJob logSendingJob = new LogSendingThreadJob();

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
        {
        }

        protected override void SendData(LogData[] logs)
        {
            LogzApi.SendLog(CredentialHelper.Credentials, logs);
        }
    }
}