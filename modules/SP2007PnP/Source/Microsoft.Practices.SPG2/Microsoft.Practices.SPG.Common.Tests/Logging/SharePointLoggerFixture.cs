//===============================================================================
// Microsoft patterns & practices
// SharePoint Guidance version 2.0
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
using System;
using System.Diagnostics;
using System.Linq;
using System.Web;

using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint.Administration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TypeMock.ArrangeActAssert;
using System.Globalization;

namespace Microsoft.Practices.SPG.Common.Tests.Logging
{
    [TestClass]
    public class SharePointLoggerFixture
    {
        private MockTraceLogger traceLogger;
        private MockEventLogger eventLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                                                 .RegisterTypeMapping<ITraceLogger, MockTraceLogger>(InstantiationType.AsSingleton)
                                                 .RegisterTypeMapping<IEventLogLogger, MockEventLogger>(InstantiationType.AsSingleton));

            traceLogger = SharePointServiceLocator.Current.GetInstance<ITraceLogger>() as MockTraceLogger;
            eventLogger = SharePointServiceLocator.Current.GetInstance<IEventLogLogger>() as MockEventLogger;
        }

        [TestCleanup]
        public void Cleanup()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(null);
        }

        [TestMethod]
        public void TraceLogsOnlyToTraceLog()
        {
            SharePointLogger target = new SharePointLogger();

            target.TraceToDeveloper("Message", 99, TraceSeverity.High, "Category1");

            Assert.IsNull((target.EventLogLogger as MockEventLogger).Message);

            AssertLogData(target.TraceLogger as MockTraceLogger, TraceSeverity.High);
        }

        [TestMethod]
        public void LogToOperationsGoesToBothEventLogAndTraceLog()
        {
            SharePointLogger target = new TestableSharePointLogger();

            target.LogToOperations("Message", 99, EventLogEntryType.Error, "Category1");

            AssertLogData(target.TraceLogger as MockTraceLogger, TraceSeverity.High);
            AssertLogData(target.EventLogLogger as MockEventLogger, EventLogEntryType.Error);

            target.LogToOperations("Message", 99, EventLogEntryType.Warning, "Category1");

            AssertLogData(target.TraceLogger as MockTraceLogger, TraceSeverity.Medium);
            AssertLogData(target.EventLogLogger as MockEventLogger, EventLogEntryType.Warning);
        }

        [TestMethod]
        public void IfTracingErrorOccursEventLogMessageShouldStillBeWritten()
        {
            SharePointLogger target = new SharePointLogger();
            ((ActivatingServiceLocator) SharePointServiceLocator.Current).RegisterTypeMapping<ITraceLogger, FailingLogger>();
            target.LogToOperations("Message", 99, EventLogEntryType.Error, "Category1");

            Assert.AreEqual("Message", (target.EventLogLogger as MockEventLogger).Messages[0]);
            Assert.IsTrue((target.EventLogLogger as MockEventLogger).Messages[1].Contains("NotImplementedException"));
        }

        [TestMethod]
        public void WhenLoggingFailsAClearExceptionIsThrown()
        {
            SharePointLogger target = new SharePointLogger();

            ((ActivatingServiceLocator) SharePointServiceLocator.Current)
                .RegisterTypeMapping<ITraceLogger, FailingLogger>()
                .RegisterTypeMapping<IEventLogLogger, FailingLogger>();

            try
            {
                target.LogToOperations("Message", 99, EventLogEntryType.Error, "Category1");
                Assert.Fail();
            }
            catch (LoggingException ex)
            {
                Assert.IsTrue(ex.Message.Contains("trace log"));
                Assert.IsTrue(ex.Message.Contains("EventLog"));
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void LogMessageShouldContainContextualInformation()
        {
            SharePointLogger target = new SharePointLogger();
   
            Isolate.WhenCalled(() => HttpContext.Current.User.Identity.Name).WillReturn("TestUser");
            Isolate.WhenCalled(() => HttpContext.Current.Request.Url).WillReturn(new Uri("http://localhost/mypage.aspx"));
            Isolate.WhenCalled(() => HttpContext.Current.Request.UserHostAddress).WillReturn("1.1.1.1.1");
            Isolate.WhenCalled(() => HttpContext.Current.Request.UserAgent).WillReturn("MyAgent");
            Isolate.WhenCalled(() => HttpContext.Current.Timestamp).WillReturn(new DateTime(2000, 1, 1));

            target.LogToOperations("Message");

            Assert.IsTrue(eventLogger.Message.Contains("Request URL: 'http://localhost/mypage.aspx"));
            Assert.IsTrue(eventLogger.Message.Contains("Request TimeStamp: '" + new DateTime(2000, 1, 1).ToString("o", CultureInfo.CurrentCulture) + "'"));
            Assert.IsTrue(eventLogger.Message.Contains("UserName: 'TestUser'"));
            Assert.IsTrue(eventLogger.Message.Contains("Originating IP address: '1.1.1.1.1'"));
            Assert.IsTrue(eventLogger.Message.Contains("User Agent: 'MyAgent'"));

            
        }

        private void AssertLogData(MockTraceLogger logger, TraceSeverity severity)
        {
            Assert.AreEqual("Message", logger.Message);
            Assert.AreEqual("Category1", logger.Category);
            Assert.AreEqual(99, logger.EventID);
            Assert.AreEqual(severity, logger.Severity);
        }

        private void AssertLogData(MockEventLogger logger, EventLogEntryType severity)
        {
            Assert.AreEqual("Message", logger.Message);
            Assert.AreEqual("Category1", logger.Category);
            Assert.AreEqual(99, logger.EventID);
            Assert.AreEqual(severity, logger.Severity);
        }

        class FailingLogger : ITraceLogger, IEventLogLogger
        {
            public void Trace(string message, int eventId, TraceSeverity severity, string category)
            {
                throw new NotImplementedException();
            }

            public void Log(string message, int eventId, EventLogEntryType severity, string category)
            {
                throw new NotImplementedException();
            }
        }
    }

    public class TestableSharePointLogger : SharePointLogger
    {
        protected override string BuildEventLogMessage(string message, int eventId, EventLogEntryType severity, string category)
        {
            return message;
        }
    }
}