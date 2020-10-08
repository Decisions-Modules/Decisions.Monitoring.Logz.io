﻿using System;
using System.Collections.Generic;
using Decisions.Monitoring.Logz.io.Data;
using Decisions.Monitoring.Logz.io.Utility;
using DecisionsFramework;
using DecisionsFramework.ServiceLayer;
using DecisionsFramework.Utilities.Profiler;
using DecisionsFramework.Utilities.Profiler.Heartbeat;

namespace Decisions.Monitoring.Logz.io
{
    public class LogzMetricsSummarizer : IProfilerSummaryWriter, IInitializable
    {
        public void Initialize()
        {
            ProfilerService.SummaryWriter = this;
        }

        public void WriteSummary(ProfileWriterData header, ProfileTimeSummary timeSummary)
        {
            // Do Nothing.  
        }

        public void WriteHeartbeatData(HeartbeatData heartbeat)
        {
            if (heartbeat != null)
            {
                ProfilerService.DetailWriter?.WriteDetail(
                    new ProfileWriterData("Flow Runs", null, ProfilerType.Usage),
                    new[]
                    {
                        new ProfilerDetail(ProfilerDetailType.Info, "Flow Runs")
                        {
                            Count = heartbeat.NumberOfFlowStarts
                        }
                    }, TimeSpan.Zero);
                // 
                ProfilerService.DetailWriter?.WriteDetail(
                    new ProfileWriterData("Rule Runs", null, ProfilerType.Usage),
                    new[]
                    {
                        new ProfilerDetail(ProfilerDetailType.Info, "Rule Runs")
                        {
                            Count = heartbeat.NumberOfRuleExecutions
                        }
                    }, TimeSpan.Zero);
                ProfilerService.DetailWriter?.WriteDetail(
                    new ProfileWriterData("API Calls", null, ProfilerType.Usage),
                    new[]
                    {
                        new ProfilerDetail(ProfilerDetailType.Info, "API Calls")
                        {
                            Count = heartbeat.NumberOfAPICalls
                        }
                    }, TimeSpan.Zero);
                ProfilerService.DetailWriter?.WriteDetail(
                    new ProfileWriterData("Job Runs", null, ProfilerType.Usage),
                    new[]
                    {
                        new ProfilerDetail(ProfilerDetailType.Info, "Job Runs")
                        {
                            Count = heartbeat.NumberOfJobStarts
                        }
                    }, TimeSpan.Zero);
            }
        }
    }

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
                foreach (var eachEntry in details) metrics.Add(CreateMetrics(header, eachEntry));
                metricSendingJob.AddItem(metrics.ToArray());
            }
        }

        private LogzMetricsData CreateMetrics(ProfileWriterData header, ProfilerDetail detail)
        {
            var settings = Settings.GetSettings();

            var metrics = new LogzMetricsData
            {
                Metrics = new LogzMetrics {DetailCount = detail.Count},
                Dimensions = new LogzDimensions
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

    internal class MetricsSendingThreadJob : DataSendingThreadJob<LogzMetricsData>
    {
        public MetricsSendingThreadJob() : base("Decisions.Logz metrics queue", TimeSpan.FromSeconds(10))
        {
        }

        protected override void SendData(LogzMetricsData[] metrics)
        {
            if (!LogzApi.SendMetrics(CredentialHelper.Credentials, metrics))
                LogConstants.SYSTEM.Error("Decisions.Monitoring.Logz.io failed to send Metrics");
        }
    }
}