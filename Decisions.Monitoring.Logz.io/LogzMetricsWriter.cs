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
    public class LogzMetricsWriter : IProfilerDetailWriter, IInitializable
    {
        private readonly MetricsSendingThreadJob metricSendingJob = new MetricsSendingThreadJob();

        public void Initialize()
        {
            ProfilerService.DetailWriter = this;
        }

        public void WriteDetail(ProfileWriterData header, ProfilerDetail[] details, TimeSpan time)
        {
            if (header.type == ProfilerType.Usage && details != null && details.Length > 0)
            {
                var metrics = new List<LogzMetricsData>(details.Length);
                foreach (ProfilerDetail eachEntry in details)
                {
                    metrics.Add(CreateMetrics(header, eachEntry));
                }
                metricSendingJob.AddItem(metrics.ToArray());
            }
        }

        private LogzMetricsData CreateMetrics(ProfileWriterData header, ProfilerDetail detail)
        {
            var settings = Settings.GetSettings();

            var metrics = new LogzMetricsData()
            {
                Metrics = new LogzMetrics() { DetailCount = detail.Count },
                Dimensions = new LogzDimensions()
                {
                    Name = header.Name,
                    Details = detail.Message,
                    HostName = settings.HostName,
                    BasePortalUrlName = settings.PortalBaseUrl,
                    DecisionsVersion = DecisionsVersion.VERSION
                }
            };
            return metrics;
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
