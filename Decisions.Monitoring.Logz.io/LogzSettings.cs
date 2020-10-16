using System.Collections.Generic;
using System.Runtime.Serialization;
using DecisionsFramework;
using DecisionsFramework.Data.ORMapper;
using DecisionsFramework.Design.ConfigurationStorage.Attributes;
using DecisionsFramework.Design.Properties;
using DecisionsFramework.ServiceLayer;
using DecisionsFramework.ServiceLayer.Actions;
using DecisionsFramework.ServiceLayer.Actions.Common;
using DecisionsFramework.ServiceLayer.Utilities;
using DecisionsFramework.Utilities.validation.Rules;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Decisions.Monitoring.Logz.io.Data
{

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LogLevel
    {
        None = 0,
        Debug = 1,
        Info = 2,
        Warn = 4,
        Error = 8,
        Fatal = 16
    }

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
        [PropertyClassificationAttribute("Minimum Log Level to Send", 2)]
        [EnumEditor]
        public LogLevel MinSendLogLevel { get; set; }

        [DataMember]
        [PropertyHiddenByValue(nameof(MinSendLogLevel), LogLevel.None, true)]
        [PropertyClassificationAttribute("Log Token", 3)]
        public string LogToken { get; set; }

        [ORMField]
        [DataMember]
        [PropertyClassificationAttribute("Send Metrics", 4)]
        public bool SendMetrics { get; set; }

        [ORMField]
        [DataMember]
        [PropertyHiddenByValue(nameof(SendMetrics), false, true)]
        [PropertyClassificationAttribute("Metrics Token", 5)]
        public string MetricsToken { get; set; }

        public void Initialize()
        {
            var me = LogzSettings.Instance();
            if (string.IsNullOrEmpty(Id))
            {
                me.BaseUrl = DefaultBaseUrl;
                me.MinSendLogLevel = LogLevel.None;
                ModuleSettingsAccessor<LogzSettings>.SaveSettings();
            }
        }

        public ValidationIssue[] GetValidationIssues()
        {
            var issues = new List<ValidationIssue>();

            if ((MinSendLogLevel != LogLevel.None) && string.IsNullOrEmpty(LogToken))
                issues.Add(new ValidationIssue(this, "Log Token must be supplied", "", BreakLevel.Fatal,
                    nameof(LogToken)));

            if (SendMetrics && string.IsNullOrEmpty(MetricsToken))
                issues.Add(new ValidationIssue(this, "Metrics Token must be supplied", "", BreakLevel.Fatal,
                    nameof(MetricsToken)));

            return issues.ToArray();
        }

        public override BaseActionType[] GetActions(AbstractUserContext userContext, EntityActionType[] types)
        {
            var all = new List<BaseActionType>();
            all.Add(new EditEntityAction(typeof(LogzSettings), "Edit", "Edit"));
            return all.ToArray();
        }

        static public LogzSettings Instance() { 
            return ModuleSettingsAccessor<LogzSettings>.GetSettings();
        }
    }
}