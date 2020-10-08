using Newtonsoft.Json;

namespace Decisions.Monitoring.Logz.io.Data
{
    public class LogzMetricsData
    {
        [JsonProperty(PropertyName = "metrics")]
        public LogzMetrics Metrics { get; set; }

        [JsonProperty(PropertyName = "dimensions")]
        public LogzDimensions Dimensions { get; set; }
    }

    public class LogzMetrics
    {
        public int DetailCount { get; set; }
    }

    public class LogzDimensions
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public string HostName { get; set; }
        public string BasePortalUrlName { get; set; }
        public string DecisionsVersion { get; set; }
    }
}