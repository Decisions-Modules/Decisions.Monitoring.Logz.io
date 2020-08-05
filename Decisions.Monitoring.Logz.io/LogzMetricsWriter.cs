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
    public class LogzMetricsWriter : IProfilerDetailWriter, IProfilerSummaryWriter, IInitializable
    {
        private MetricsSendingThreadJob metricSendingJob = new MetricsSendingThreadJob();

        public void Initialize()
        {
            ProfilerService.DetailWriter = this;
            ProfilerService.SummaryWriter = this;
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

            metricSendingJob.AddItem(metrics);
        }

        public void WriteSummary(ProfileWriterData header, ProfileTimeSummary timeSummary)
        {
            
        }

    }

    class MetricsSendingThreadJob : DataSendingThreadJob<LogzMetricsData>
    {
        public MetricsSendingThreadJob() : base("Decisions.Logz metrics queue", TimeSpan.FromSeconds(10))
        { }

        protected override void SendData(LogzMetricsData[] metrics)
        {
            if (!LogzApi.SendMetrics(CredentialHelper.Credentials, metrics))
                LogConstants.SYSTEM.Error("Decisions.Monitoring.Logz.io failed to send Metrics");
        }
    }
}
