/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionsFramework;
using Logzio.DotNet.NLog;
using NLog;
using NLog.Config;

namespace Decisions.Monitoring.Logz.io.Framework
{
    public partial class LogzLogWriter : ILogWriter
    {
        private  String BaseUrl = "https://listener.logz.io:8071";
        private String Token = "ixibrtOtQpXGedjruzVmNTGekpCSWYsl";

        private Logger logger;

        public LogzLogWriter()
        {
            var config = new LoggingConfiguration();

            var logzioTarget = new LogzioTarget
            {
                Name = "Logzio",
                Token = this.Token,
                LogzioType = "nlog",
                ListenerUrl = BaseUrl,
                BufferSize = 100,
                BufferTimeout = TimeSpan.Parse("00:00:05"),
                RetriesMaxAttempts = 3,
                RetriesInterval = TimeSpan.Parse("00:00:02"),
                Debug = false,
            };

            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logzioTarget);
            NLog.LogManager.Configuration = config;

            logger = LogManager.GetCurrentClassLogger();
        }

        public void Write(LogData logData)
        {
            logger.Log(GetLogLevel(logData), logData.ToString);
            
        }

        private LogLevel GetLogLevel(LogData logData)
        {
            var level = logData.Level;
            if (level.HasFlag(LogSeverity.Fatal)) return LogLevel.Fatal;
            if (level.HasFlag(LogSeverity.Error)) return LogLevel.Error;
            if (level.HasFlag(LogSeverity.Warn)) return LogLevel.Warn;
            if (level.HasFlag(LogSeverity.Info)) return LogLevel.Info;
            if (level.HasFlag(LogSeverity.Debug)) return LogLevel.Debug;

            return LogLevel.Info;
        }
    }
}*/
