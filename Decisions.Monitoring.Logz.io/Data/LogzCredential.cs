using DecisionsFramework.Data.ORMapper;
using DecisionsFramework.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Decisions.Monitoring.Logz.io.Data
{
    [ORMEntity]
    [DataContract]
    public class LogzCredential : AbstractModuleSettings, IInitializable
    {
        [ORMField]
        [DataMember]
        public string BaseUrl { get; set; } = "https://listener.logz.io:8071";

        [ORMField]
        [DataMember]
        public string LogToken { get; set; }

        [ORMField]
        [DataMember]
        public string MetricsToken { get; set; }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
