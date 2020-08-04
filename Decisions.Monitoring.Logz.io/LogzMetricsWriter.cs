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
    public class LogzMetricsWriter : LogzBaseWriter, IProfilerDetailWriter, IInitializable
    {
        private Buffer<LogzMetricsData> buffer;

        public void Initialize()
        {
            buffer = new Buffer<LogzMetricsData>("Decisions.Logz sending metrics", TimeSpan.FromSeconds(10), SendMetrics);
            ProfilerService.DetailWriter = this;
        }

        public void WriteDetail(ProfileWriterData header, ProfilerDetail[] details, TimeSpan time)
        {
            var metrics = new LogzMetricsData()
            {
                Metrics = new LogzMetrics() { TimeMilliseconds = time.TotalMilliseconds },
                Dimensions = new LogzDimensions()
                {
                    Name = header.Name,
                    Parents = header.Parents,
                    ProfilerType = header.type.ToString(),
                }
            };

            if (details != null)
            {
                int DetailCount = 0;
                var message = new StringBuilder();
                foreach (var item in details)
                {
                    message.AppendLine($"{item.Count} times: {item.DetailType} {item.Message}");
                    DetailCount += item.Count;
                }
                metrics.Dimensions.Details = message.ToString();
                metrics.Metrics.DetailCount = DetailCount;
            }

            buffer.AddData(metrics);
        }

        private void SendMetrics(LogzMetricsData[] metrics)
        {
            if (!LogzApi.SendMetrics(Credentials, metrics))
                LogConstants.SYSTEM.Info("Decisions.Monitoring.Logz.io failed to send Metrics");
        }
    }
}
