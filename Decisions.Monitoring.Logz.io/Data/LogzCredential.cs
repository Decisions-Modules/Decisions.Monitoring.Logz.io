using DecisionsFramework.Data.ORMapper;
using DecisionsFramework.ServiceLayer;
using DecisionsFramework.ServiceLayer.Actions;
using DecisionsFramework.ServiceLayer.Actions.Common;
using DecisionsFramework.ServiceLayer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Decisions.Monitoring.Logz.io.Data
{
    public class LogzCredential
    {
        public string BaseUrl { get; set; }
        public string LogToken { get; set; }
        public string MetricsToken { get; set; }
    }
}
