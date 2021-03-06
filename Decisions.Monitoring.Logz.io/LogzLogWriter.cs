﻿using Decisions.Monitoring.Logz.io.Data;
using Decisions.Monitoring.Logz.io.Utility;
using DecisionsFramework;
using DecisionsFramework.ServiceLayer;
using System;

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
            var moduleSettings = LogzSettings.Instance();
            if ((moduleSettings.MinSendLogLevel == LogLevel.None) ||
                ((int)log.Level < (int)moduleSettings.MinSendLogLevel) )
                return;

            var decisionsSettings = Settings.GetSettings();
            LogzLogData logData = new LogzLogData(log, decisionsSettings.PortalBaseUrl, System.Environment.MachineName);
            logSendingJob.AddItem(logData);
        }
    }

    internal class LogSendingThreadJob : DataSendingThreadJob<LogzLogData>
    {
        public LogSendingThreadJob() : base("Decisions.Logz log queue", TimeSpan.FromSeconds(10))
        {
        }

        protected override void SendData(LogzLogData[] logs)
        {
            LogzApi.SendLog(CredentialHelper.Credentials, logs); // We ignore log error because we cannot write log-error to log
        }
    }
}