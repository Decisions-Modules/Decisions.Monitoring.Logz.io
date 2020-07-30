using Decisions.Monitoring.Logz.io.Data;
using Decisions.Monitoring.Logz.io.Utility;
using DecisionsFramework;
using DecisionsFramework.ServiceLayer;
using DecisionsFramework.Utilities.Profiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decisions.Monitoring.Logz.io
{
    public class LogzMetricsWriter : IProfilerDetailWriter, ILogWriter
    {
        LogzCredential Credentials { get { return null; } }

        public void Initialize()
        {

        }

        public void WriteDetail(ProfileWriterData header, ProfilerDetail[] details, TimeSpan time)
        {
            var metrics = new List<LogzMetricsData>(details.Length);
            foreach (ProfilerDetail eachEntry in details)
            {
                var item = new LogzMetricsData()
                {
                    Metrics = new LogzMetrics() { Count = eachEntry.Count },
                    Dimensions = new LogzDimensions()
                    {
                        Name = header.Name,
                        Parents = header.Parents,
                        ProfilerType = header.type.ToString(),

                        Message = eachEntry.Message,
                        DetailType = eachEntry.DetailType.ToString(),
                    }
                };
                metrics.Add(item);
            }

            LogzApi.SendMetrics(Credentials, metrics.ToArray());

        }

        public void Write(LogData log)
        {
            LogzApi.SendLog(Credentials, log);
        }
    }
}
