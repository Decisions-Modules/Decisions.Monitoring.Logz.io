using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decisions.Monitoring.Logz.io.Data
{
    public class LogzMetricsData
    {
        [JsonProperty(PropertyName = "metrics")]
        public LogzMetrics Metrics { get; set; } 

        [JsonProperty(PropertyName = "dimensions")]
        public LogzDimensions Dimensions { get; set; } = new LogzDimensions();
    }

    public class LogzMetrics
    {
        public long Count { get; set; }
    }

    public class LogzDimensions
    {
        public string Name { get; set; }
        public string Parents { get; set; }
        public string ProfilerType { get; set; }

        public string Message { get; set; }
        public string DetailType { get; set; }
    }
}
