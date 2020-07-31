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
        public void Initialize()
        {
            ProfilerService.DetailWriter = this;
        }

        public void WriteDetail(ProfileWriterData header, ProfilerDetail[] details, TimeSpan time)
        {
            var metrics = new LogzMetricsData()
            {
                Metrics = new LogzMetrics() { TotalMilliseconds = time.TotalMilliseconds },
                Dimensions = new LogzDimensions()
                {
                    Name = header.Name,
                    Parents = header.Parents,
                    ProfilerType = header.type.ToString(),
                }
            };

            if (details != null)
            {
                var message = new StringBuilder();
                foreach (var item in details)
                {
                    message.AppendLine($"{item.Count} times: {item.DetailType} {item.Message}");
                }
                metrics.Dimensions.Details = message.ToString();
            }

            LogzApi.SendMetrics(Credentials, metrics);

            /* if (details != null && details.Length > 0)
             {
                 var metrics = new List<LogzMetricsData>();
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
             }*/
        }


    }
}
