using DecisionsFramework.Data.ORMapper;
using DecisionsFramework.Design.Properties;
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
    [ORMEntity]
    [DataContract]
    public class LogzSettings : AbstractModuleSettings, IInitializable
    {
        [ORMField]
        [DataMember]
        [PropertyClassificationAttribute("Base Url", 1)]
        public string BaseUrl { get; set; }

        [ORMField]
        [DataMember]
        [PropertyClassificationAttribute("Log Token", 2)]
        public string LogToken { get; set; }

        [ORMField]
        [DataMember]
        [PropertyClassificationAttribute("Metrics Token", 3)]
        public string MetricsToken { get; set; }

        public override BaseActionType[] GetActions(AbstractUserContext userContext, EntityActionType[] types)
        {
            List<BaseActionType> all = new List<BaseActionType>();
            all.Add(new EditEntityAction(typeof(LogzSettings), "Edit", "Edit"));
            return all.ToArray();
        }

        public const string DefaultBaseUrl = "https://listener.logz.io:8071";
        public void Initialize()
        {
            LogzSettings me = ModuleSettingsAccessor<LogzSettings>.GetSettings();
            if (string.IsNullOrEmpty(Id))
            {
                me.BaseUrl = DefaultBaseUrl;
                ModuleSettingsAccessor<LogzSettings>.SaveSettings();
            }
        }
    }
}
