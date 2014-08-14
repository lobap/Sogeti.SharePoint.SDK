//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Web.Behaviors;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.Logging.Moles;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint.Administration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.SharePoint.Moles;
using Microsoft.Practices.SharePoint.Common.Moles;
using Microsoft.Practices.SharePoint.Common.ProxyArgs;
using Microsoft.SharePoint.Behaviors;
using Microsoft.SharePoint.Utilities.Moles;

namespace Microsoft.Practices.SharePoint.Common.Tests.Logging
{

    [TestClass]
    public class SharePointLoggerTests
    {
        const string testMessageString = "{07FC1166-6A0F-499C-854F-C5B15B887D96}";

        [TestInitialize]
        public void Setup()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator());
            ((ActivatingServiceLocator)SharePointServiceLocator.GetCurrent())
                .RegisterTypeMapping<ITraceLogger, MockEventAndTraceLogger>(InstantiationType.AsSingleton)
                .RegisterTypeMapping<IEventLogLogger, MockEventAndTraceLogger>(InstantiationType.AsSingleton);
        }

        [TestCleanup]
        public void Teardown()
        {
            SharePointServiceLocator.Reset();
            SharePointEnvironment.Reset();
        }

        [TestMethod]
        public void TraceLogsOnlyToTraceLogSucceeds()
        {
            //Arrange
            string testMessage = testMessageString;
            int testEventId = 99;
            var traceLogger = SharePointServiceLocator.GetCurrent().GetInstance<ITraceLogger>() as MockEventAndTraceLogger;
            var eventLogger = SharePointServiceLocator.GetCurrent().GetInstance<IEventLogLogger>() as MockEventAndTraceLogger;

            //Act
            var target = new SharePointLogger();
            target.TraceToDeveloper(testMessage, testEventId, TraceSeverity.High, TestsConstants.AreasCategories);

            // Assert
            Assert.IsTrue(traceLogger.Messages.Count == 1);
            Assert.IsTrue(eventLogger.Messages.Count == 0);
            Assert.AreEqual(traceLogger.Messages[0].Message, testMessage);
            Assert.AreEqual(traceLogger.Messages[0].Category, TestsConstants.AreasCategories);
            Assert.AreEqual(traceLogger.Messages[0].EventId, testEventId);
            Assert.AreEqual(traceLogger.Messages[0].TraceSeverity, TraceSeverity.High);
        }


        [TestMethod]
        [HostType("Moles")]
        public void WriteSandboxTrace_TracesMessage()
        {
            //Arrange
            string testMessage = testMessageString;
            int testEventId = 99;
            TracingOperationArgs tracingArgs = null;
            BSPSite site = new BSPSite();
            MSPSite s = new MSPSite(site){ IDGet = () => TestsConstants.TestGuid};
            BSPContext.SetCurrent();
            MSPContext c = new MSPContext(BSPContext.Current){SiteGet = () => site};
            MSPUtility.ExecuteRegisteredProxyOperationStringStringSPProxyOperationArgs = (a, t, args) =>
                                                                                             {
                                                                                                 tracingArgs =
                                                                                                     args as
                                                                                                     TracingOperationArgs;
                                                                                                 return null;
                                                                                             };

            var context = new SIApplicationContextProvider()
                              {
                                  GetCurrentAppDomainFriendlyName = () => "SandboxDomain",
                                  IsProxyCheckerInstalled = () => true,
                                  IsProxyInstalledStringString = (a, t) => true,
                              };

            SharePointEnvironment.ApplicationContextProvider = context;
             
            //Act
            var target = new SharePointLogger();
            target.TraceToDeveloper(testMessage, testEventId, SandboxTraceSeverity.High, TestsConstants.AreasCategories);

            // Assert
            Assert.IsNotNull(tracingArgs);
            Assert.AreEqual(tracingArgs.Message, testMessage);
            Assert.AreEqual(tracingArgs.Category, TestsConstants.AreasCategories);
            Assert.AreEqual(tracingArgs.EventId, testEventId);
            Assert.AreEqual((SandboxTraceSeverity)tracingArgs.Severity, SandboxTraceSeverity.High);
        }

        [TestMethod]
        [HostType("Moles")]
        public void WriteSandboxTrace_NoContext_TracesMessage()
        {
            //Arrange
            SharePointEnvironment.Reset();
            string testMessage = testMessageString;
            int testEventId = 99;
            TracingOperationArgs tracingArgs = null;
            BSPSite site = new BSPSite();
            MSPSite s = new MSPSite(site) { IDGet = () => TestsConstants.TestGuid };
            MSPContext.CurrentGet = () => null;
            MSPUtility.ExecuteRegisteredProxyOperationStringStringSPProxyOperationArgs = (a, t, args) =>
            {
                tracingArgs =
                    args as
                    TracingOperationArgs;
                return null;
            };

            var context = new SIApplicationContextProvider()
            {
                GetCurrentAppDomainFriendlyName = () => "SandboxDomain",
                IsProxyCheckerInstalled = () => true,
                IsProxyInstalledStringString = (a, t) => true,
            };

            SharePointEnvironment.ApplicationContextProvider = context;

            //Act
            var target = new SharePointLogger();
            target.TraceToDeveloper(testMessage, testEventId, SandboxTraceSeverity.High, TestsConstants.AreasCategories);

            // Assert
            Assert.IsNotNull(tracingArgs);
            Assert.AreEqual(tracingArgs.Message, testMessage);
            Assert.AreEqual(tracingArgs.Category, TestsConstants.AreasCategories);
            Assert.AreEqual(tracingArgs.EventId, testEventId);
            Assert.AreEqual((SandboxTraceSeverity)tracingArgs.Severity, SandboxTraceSeverity.High);
        }
   
        [TestMethod]
        public void LogToOperationsOnlyLogsToEventLogSucceeds()
        {
            //Arrange
            string testMessage = testMessageString;
            int testEventId = 99;
            var traceLogger = SharePointServiceLocator.GetCurrent().GetInstance<ITraceLogger>() as MockEventAndTraceLogger;
            var eventLogger = SharePointServiceLocator.GetCurrent().GetInstance<IEventLogLogger>() as MockEventAndTraceLogger;

            //Act
            var target = new SharePointLogger();
            target.LogToOperations(testMessage, testEventId, EventSeverity.Error);

            // Assert
            Assert.IsTrue(traceLogger.Messages.Count == 0);
            Assert.IsTrue(eventLogger.Messages.Count == 1);
            Assert.AreEqual(eventLogger.Messages[0].Message, testMessage);
            Assert.AreEqual(eventLogger.Messages[0].Category, null); // Diagnostics service will use default category when called at a lower level.
            Assert.AreEqual(eventLogger.Messages[0].EventId, testEventId);
            Assert.AreEqual(eventLogger.Messages[0].EventSeverity, EventSeverity.Error);
        }

        [TestMethod]
        [HostType("Moles")]
        public void WriteSandboxLog_LogsMessage()
        {
            //Arrange
            SharePointEnvironment.Reset();
            var context = new SIApplicationContextProvider()
            {
                GetCurrentAppDomainFriendlyName = () => "SandboxDomain",
                IsProxyCheckerInstalled = () => true,
                IsProxyInstalledStringString = (a, t) => true,
            };

            SharePointEnvironment.ApplicationContextProvider = context;
            string testMessage = testMessageString;
            int testEventId = 99;
            LoggingOperationArgs loggingArgs = null;
            BSPSite site = new BSPSite();
            MSPSite s = new MSPSite(site) { IDGet = () => TestsConstants.TestGuid };
            BSPContext.SetCurrent();
            MSPContext c = new MSPContext(BSPContext.Current) { SiteGet = () => site };
            MSPUtility.ExecuteRegisteredProxyOperationStringStringSPProxyOperationArgs = (a, t, args) =>
            {
                loggingArgs =
                    args as
                    LoggingOperationArgs;
                return null;
            };


            //Act
            var target = new SharePointLogger();
            target.LogToOperations(testMessage, testEventId, SandboxEventSeverity.Error, TestsConstants.AreasCategories);

            // Assert
            Assert.IsNotNull(loggingArgs);
            Assert.AreEqual(loggingArgs.Message, testMessage);
            Assert.AreEqual(loggingArgs.Category, TestsConstants.AreasCategories);
            Assert.AreEqual(loggingArgs.EventId, testEventId);
            Assert.AreEqual((SandboxEventSeverity)loggingArgs.Severity, SandboxEventSeverity.Error);
        }


        [TestMethod]
        [HostType("Moles")]
        public void WriteSandboxLog_WithoutContext_LogsMessage()
        {
            //Arrange
            string testMessage = testMessageString;
            int testEventId = 99;
            LoggingOperationArgs loggingArgs = null;
            BSPSite site = new BSPSite();
            MSPSite s = new MSPSite(site) { IDGet = () => TestsConstants.TestGuid };
            BSPContext.SetCurrent();
            MSPContext.CurrentGet = () => null;
            MSPUtility.ExecuteRegisteredProxyOperationStringStringSPProxyOperationArgs = (a, t, args) =>
            {
                loggingArgs =
                    args as
                    LoggingOperationArgs;
                return null;
            };

            var context = new SIApplicationContextProvider()
            {
                GetCurrentAppDomainFriendlyName = () => "SandboxDomain",
                IsProxyCheckerInstalled = () => true,
                IsProxyInstalledStringString = (a, t) => true,
            };

            SharePointEnvironment.ApplicationContextProvider = context;

            //Act
            var target = new SharePointLogger();
            target.LogToOperations(testMessage, testEventId, SandboxEventSeverity.Error, TestsConstants.AreasCategories);

            // Assert
            Assert.IsNotNull(loggingArgs);
            Assert.AreEqual(loggingArgs.Message, testMessage);
            Assert.AreEqual(loggingArgs.Category, TestsConstants.AreasCategories);
            Assert.AreEqual(loggingArgs.EventId, testEventId);
            Assert.AreEqual((SandboxEventSeverity)loggingArgs.Severity, SandboxEventSeverity.Error);
        }

        [TestMethod]
        [ExpectedException(typeof(LoggingException))]
        public void WhenTraceToDeveloperFailsThrowsLoggingException()
        {
            //Arrange
            ((ActivatingServiceLocator)SharePointServiceLocator.GetCurrent()).RegisterTypeMapping<ITraceLogger, FailingTraceLogger>();
            var target = new SharePointLogger();
            string testMessage = testMessageString;

            //Act
            target.TraceToDeveloper(testMessage);

            //Assert is caught by thrown exception.
        }

        [TestMethod]
        public void WhenTraceToDeveloperFailsAClearExceptionIsThrown()
        {
            //Arrange
            ((ActivatingServiceLocator)SharePointServiceLocator.GetCurrent()).RegisterTypeMapping<ITraceLogger, FailingTraceLogger>();
            var target = new SharePointLogger();
            string testMessage = testMessageString;

            //Act
            try
            {
                target.TraceToDeveloper(testMessage);
                Assert.Fail();
            }
            //Assert
            catch (LoggingException ex)
            {
                Assert.IsTrue(ex.Message.Contains(testMessage));
                Assert.IsTrue(ex.Message.Contains("Trace Log"));
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(LoggingException))]
        public void WhenLogToOperationsFailsThrowsLoggingException()
        {
            //Arrange
            ((ActivatingServiceLocator)SharePointServiceLocator.GetCurrent()).RegisterTypeMapping<IEventLogLogger, FailingEventLogger>();
            var target = new SharePointLogger();
            string testMessage = testMessageString;

            //Act
            target.LogToOperations(testMessage);

            //Assert is caught by thrown exception.

        }

        [TestMethod]
        public void WhenLoggingFailsAClearExceptionIsThrown()
        {
            //Arrange
            var target = new SharePointLogger();
            string message = testMessageString;

            ((ActivatingServiceLocator)SharePointServiceLocator.GetCurrent())
                .RegisterTypeMapping<IEventLogLogger, FailingEventLogger>();

            //Act
            try
            {
                target.LogToOperations(message, 99, EventSeverity.Error, TestsConstants.AreasCategories);
                Assert.Fail();
            }
            //Assert
            catch (LoggingException ex)
            {
                Assert.IsTrue(ex.Message.Contains(message));
                Assert.IsTrue(ex.Message.Contains("EventLog"));
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        [HostType("Moles")]
        public void LogMessageShouldContainContextualInformation()
        {
            // arrange
            var context = BHttpContext.SetCurrent();
            MSPContext.CurrentGet = () => null;

            context.Timestamp = new DateTime(2000, 1, 1);
            var user = context.SetUser();
            var identity = user.SetIdentity();
            identity.Name = "TestUser";
            var request = context.SetRequest();
            request.Url = new Uri("http://localhost/mypage.aspx");
            request.UserHostAddress = "1.1.1.1.1";
            request.UserAgent = "MyAgent";
            var eventLogger = SharePointServiceLocator.GetCurrent().GetInstance<IEventLogLogger>() as MockEventAndTraceLogger;
            Exception ex = new Exception("message");

            // act
            var target = new SharePointLogger();
            target.LogToOperations(ex);

            var message = eventLogger.Messages[0].Message;
            Assert.IsTrue(message.Contains(@"Request URL: 'http://localhost/mypage.aspx'"));
            Assert.IsTrue(message.Contains("Request TimeStamp: '" + context.Timestamp.ToString("o", CultureInfo.CurrentCulture) + "'"));
            Assert.IsTrue(message.Contains("UserName: 'TestUser'"));
            Assert.IsTrue(message.Contains("Originating IP address: '1.1.1.1.1'"));
            Assert.IsTrue(message.Contains("User Agent: 'MyAgent'"));
        }
 
        class FailingTraceLogger : SITraceLogger
        {
            public FailingTraceLogger()
            {
                this.TraceStringInt32TraceSeverityString = Trace;
            }

            public void Trace(string message, int eventId, TraceSeverity severity, string category)
            {
                throw new NotImplementedException();
            }

        }

        class FailingEventLogger: SIEventLogLogger
        {
            public FailingEventLogger()
            {
                this.LogStringInt32EventSeverityString = Log;
            }

            public void Log(string message, int eventId, EventSeverity severity, string category)
            {
                throw new NotImplementedException();
            }
        }
    }
}
