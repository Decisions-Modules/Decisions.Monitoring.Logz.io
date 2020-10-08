using System;
using Decisions.Monitoring.Logz.io.Data;
using Decisions.Monitoring.Logz.io.Utility;
using DecisionsFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Decisions.Monitoring.Logz.io.TestSuit
{
    [TestClass]
    public class LogzIoTest
    {
        private readonly LogzCredential credential = new LogzCredential
        {
            BaseUrl = LogzSettings.DefaultBaseUrl,
            LogToken = Tokens.LogToken,
            MetricsToken = Tokens.MetricsToken
        };

        [TestMethod]
        public void LogTest()
        {
            Log.LogToFile = false;

            var data = new[]
            {
                new LogData(DateTime.Now, LogSeverity.Debug, "category", "message")
                {
                    SessionID = "sessionid", Activity = "activity",
                    Exception = new InvalidOperationException("TestException")
                },
                new LogData(DateTime.Now, LogSeverity.Fatal | LogSeverity.Info, "category", "message")
                    {SessionID = "sessionid", Activity = "activity"},
                new LogData(DateTime.Now, LogSeverity.All, "category", "message")
                    {SessionID = "sessionid", Activity = "activity"},
                new LogData(DateTime.Now, LogSeverity.None, "category", "message")
                    {SessionID = "sessionid", Activity = "activity"},
                new LogData()
            };
            var res = LogzApi.SendLog(credential, data);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void MetricsTest()
        {
            var data = new[]
            {
                new LogzMetricsData
                {
                    Metrics = new LogzMetrics {DetailCount = 1},
                    Dimensions = new LogzDimensions
                    {
                        Name = "Name", Details = "Test Detail", HostName = "HostName",
                        BasePortalUrlName = "BasePortalUrlName", DecisionsVersion = "DecisionsVersion"
                    }
                },
                new LogzMetricsData
                {
                    Metrics = new LogzMetrics {DetailCount = 2},
                    Dimensions = new LogzDimensions {Name = "Name", Details = "metrics message"}
                }
            };
            var res = LogzApi.SendMetrics(credential, data);
            Assert.IsTrue(res);
        }
    }
}