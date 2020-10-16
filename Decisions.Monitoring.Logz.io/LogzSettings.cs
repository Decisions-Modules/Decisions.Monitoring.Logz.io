using System.Collections.Generic;
using System.Runtime.Serialization;
using DecisionsFramework;
using DecisionsFramework.Data.ORMapper;
using DecisionsFramework.Design.Properties;
using DecisionsFramework.ServiceLayer;
using DecisionsFramework.ServiceLayer.Actions;
using DecisionsFramework.ServiceLayer.Actions.Common;
using DecisionsFramework.ServiceLayer.Utilities;
using DecisionsFramework.Utilities.validation.Rules;

namespace Decisions.Monitoring.Logz.io.Data
{
    [ORMEntity]
    [DataContract]
    public class LogzSettings : AbstractModuleSettings, IValidationSource, IInitializable
    {
        public const string DefaultBaseUrl = "https://listener.logz.io:8071";

        [ORMField]
        [DataMember]
        [PropertyClassificationAttribute("Base Url", 1)]
        [EmptyStringRule("Base Url is required")]
        public string BaseUrl { get; set; }

        [ORMField]
        [DataMember]
        [PropertyClassificationAttribute("Send Logs", 2)]
        public bool SendLogs { get; set; }

        [ORMField]
        [DataMember]
        [PropertyHiddenByValue(nameof(SendLogs), false, true)]
        [PropertyClassificationAttribute("Log Token", 3)]
        public string LogToken { get; set; }

        public void Initialize()
        {
            var me = ModuleSettingsAccessor<LogzSettings>.GetSettings();
            if (string.IsNullOrEmpty(Id))
            {
                me.BaseUrl = DefaultBaseUrl;
                ModuleSettingsAccessor<LogzSettings>.SaveSettings();
            }
        }

        public ValidationIssue[] GetValidationIssues()
        {
            var issues = new List<ValidationIssue>();

            if (SendLogs && string.IsNullOrEmpty(LogToken))
                issues.Add(new ValidationIssue(this, "Log Token must be supplied", "", BreakLevel.Fatal,
                    nameof(LogToken)));

            return issues.ToArray();
        }

        public override BaseActionType[] GetActions(AbstractUserContext userContext, EntityActionType[] types)
        {
            var all = new List<BaseActionType>();
            all.Add(new EditEntityAction(typeof(LogzSettings), "Edit", "Edit"));
            return all.ToArray();
        }
    }
}