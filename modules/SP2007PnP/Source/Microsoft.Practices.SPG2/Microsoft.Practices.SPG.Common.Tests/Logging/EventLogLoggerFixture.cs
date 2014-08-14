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
using System.Threading;
using System.Web;
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock;
using TypeMock.ArrangeActAssert;
using Microsoft.SharePoint.Administration;

namespace Microsoft.Practices.SPG.Common.Tests.Logging
{
    [TestClass]
    public class EventLogLoggerFixture
    {
        private MockHierarchicalConfig config;

        [TestInitialize]
        public void Init()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                .RegisterTypeMapping<IHierarchicalConfig, MockHierarchicalConfig>(InstantiationType.AsSingleton));

            config = SharePointServiceLocator.Current.GetInstance<IHierarchicalConfig>() as MockHierarchicalConfig;
        }

        [TestCleanup]
        public void Cleanup()
        {
            Isolate.CleanUp();
        }

        [TestMethod]
        public void CanLogDiagnosticToEventLog()
        {

            config.ContainsReturnValue = true;
            config.GetBykeyReturnValue = "MyLog";

            bool eventLogWritten = false;
            Isolate.Fake.StaticMethods<EventLog>(Members.ReturnRecursiveFakes);
            Isolate.WhenCalled(() => EventLog.SourceExists("MyLog")).WithExactArguments().WillReturn(true);
            Isolate.WhenCalled(() => EventLog.WriteEntry("MyLog", "Category: Category1\nMessage", EventLogEntryType.Information, 99))
                .WithExactArguments().DoInstead((context) => eventLogWritten = true);


            EventLogLogger target = new TestableEventLogger();
            target.Log("Message", 99, EventLogEntryType.Information, "Category1");

            Assert.IsTrue(eventLogWritten);
        }

        [TestMethod]
        public void CanLogWarningToEventLog()
        {
            config.ContainsReturnValue = true;
            config.GetBykeyReturnValue = "MyLog";

            bool eventLogWritten = false;
            Isolate.Fake.StaticMethods<EventLog>(Members.CallOriginal);
            Isolate.WhenCalled(() => EventLog.SourceExists("MyLog")).WithExactArguments().WillReturn(true);
            Isolate.WhenCalled(() => EventLog.WriteEntry("MyLog", "Category: Category1\nMessage", EventLogEntryType.Warning, 99))
                .WithExactArguments().DoInstead((context) => eventLogWritten = true);


            EventLogLogger target = new TestableEventLogger();
            target.Log("Message", 99, EventLogEntryType.Warning, "Category1");

            Assert.IsTrue(eventLogWritten);
        }

        [TestMethod]
        public void CanLogErrorToEventLog()
        {
            bool eventLogWritten = false;
            config.ContainsReturnValue = true;
            config.GetBykeyReturnValue = "MyLog";
            Isolate.Fake.StaticMethods<EventLog>(Members.CallOriginal);
            Isolate.WhenCalled(() => EventLog.SourceExists("MyLog")).WithExactArguments().WillReturn(true);
            Isolate.WhenCalled(() => EventLog.WriteEntry("MyLog", "Category: Category1\nMessage", EventLogEntryType.Error, 99))
                .WithExactArguments().DoInstead((context) => eventLogWritten = true);


            EventLogLogger target = new TestableEventLogger();
            target.Log("Message", 99, EventLogEntryType.Error, "Category1");

            Assert.IsTrue(eventLogWritten);
        }

        [TestMethod]
        public void CanCreateEventSource()
        {
            bool eventSourceCreated = false;
            config.ContainsReturnValue = true;
            config.GetBykeyReturnValue = "MyLog";
            Isolate.Fake.StaticMethods<EventLog>(Members.CallOriginal);
            Isolate.WhenCalled(() => EventLog.SourceExists("MyLog")).WithExactArguments().WillReturn(false);
            Isolate.WhenCalled(() => EventLog.CreateEventSource("MyLog", "Application")).WithExactArguments().DoInstead((context) => eventSourceCreated = true);

            Isolate.WhenCalled(() => EventLog.WriteEntry("MyLog", "Category: Category1\nMessage", EventLogEntryType.SuccessAudit, 99))
                .WithExactArguments().IgnoreCall();

            EventLogLogger target = new TestableEventLogger();
            target.Log("Message", 99, EventLogEntryType.SuccessAudit, "Category1");

            Assert.IsTrue(eventSourceCreated);
        }

        [TestMethod]
        public void UsesDefaultIfNoEventSourceNameConfigured()
        {
            config.ContainsReturnValue = false;

            bool eventLogWritten = false;
            Isolate.Fake.StaticMethods<EventLog>(Members.CallOriginal);
            Isolate.WhenCalled(() => EventLog.SourceExists("Office SharePoint Server")).WithExactArguments().WillReturn(true);
            Isolate.WhenCalled(() => EventLog.WriteEntry("Office SharePoint Server", "Category: Category1\nMessage", EventLogEntryType.Warning, 0))
                .WithExactArguments().DoInstead((context) => eventLogWritten = true);


            EventLogLogger target = new TestableEventLogger();
            target.Log("Message", 99, EventLogEntryType.Warning, "Category1");

            Assert.IsTrue(eventLogWritten);
        }

        [TestMethod]
        public void IfCreateEventSourceFailsUseDefaultPropertyBag()
        {
            config.ContainsReturnValue = true;
            config.GetBykeyReturnValue = "MyLog";
            Isolate.Fake.StaticMethods<EventLog>(Members.CallOriginal);
            Isolate.WhenCalled(() => EventLog.SourceExists("MyLog")).WithExactArguments().WillReturn(false);
            Isolate.WhenCalled(() => EventLog.CreateEventSource("MyLog", "Application")).WithExactArguments().DoInstead(
                delegate
                    {
                        throw new InvalidOperationException("TestException");
                    });

            bool originalMessageCalled = false;
            Isolate.WhenCalled(() => EventLog.WriteEntry("Office SharePoint Server", "Category: Category1\nMessage", EventLogEntryType.SuccessAudit, 0))
                .WithExactArguments().DoInstead((context) => originalMessageCalled = true);

            string logError = string.Empty;
            Isolate.WhenCalled(() => EventLog.WriteEntry("Office SharePoint Server", "Could not create eventsource 'MyLog'.", EventLogEntryType.Error, 0))
                .DoInstead((context) => logError = context.Parameters[1] as string);

            EventLogLogger target = new TestableEventLogger();
            target.Log("Message", 99, EventLogEntryType.SuccessAudit, "Category1");

            Assert.IsTrue(originalMessageCalled);
            Assert.IsTrue(logError.Contains("'MyLog'"));
            Assert.IsTrue(logError.Contains("InvalidOperationException"));
            Assert.IsTrue(logError.Contains("TestException"));
        }

        [TestMethod]
        public void IfEventSourceIsDefaultEventSourceLogWarning()
        {
            config.ContainsReturnValue = true;
            config.GetBykeyReturnValue = "Office SharePoint Server";
            Isolate.Fake.StaticMethods<EventLog>(Members.CallOriginal);

            bool originalMessageCalled = false;
            Isolate.WhenCalled(() => EventLog.WriteEntry("Office SharePoint Server", "Category: Category1\nMessage", EventLogEntryType.SuccessAudit, 0))
                .WithExactArguments().DoInstead((context) => originalMessageCalled = true);

            bool loggedWarning = false;
            Isolate.WhenCalled(() => EventLog.WriteEntry("Office SharePoint Server","The current application should not be logging using the 'Office SharePoint Server' event source. Please configure a new event source.", EventLogEntryType.Warning, 0))
                .WithExactArguments().DoInstead((context) => loggedWarning = true);

            EventLogLogger target = new TestableEventLogger();
            target.Log("Message", 99, EventLogEntryType.SuccessAudit, "Category1");

            Assert.IsTrue(originalMessageCalled);
            Assert.IsTrue(loggedWarning);
        }

        [TestMethod]
        public void CanLogWithoutContext()
        {
            Isolate.WhenCalled(() => SPContext.Current).WillReturn(null);

            config.ThrowContextError = true;
            config.ContainsReturnValue = true;
            config.GetBykeyReturnValue = "MyLog";

            bool eventLogWritten = false;
            Isolate.Fake.StaticMethods<EventLog>(Members.CallOriginal);
            Isolate.WhenCalled(() => EventLog.SourceExists("MyLog")).WithExactArguments().WillReturn(true);
            Isolate.WhenCalled(() => EventLog.WriteEntry("MyLog", "Category: Category1\nMessage", EventLogEntryType.SuccessAudit, 99))
                .WithExactArguments().DoInstead((context) => eventLogWritten = true);

            EventLogLogger target = new TestableEventLogger();
            target.Log("Message", 99, EventLogEntryType.SuccessAudit, "Category1");

            Assert.IsTrue(eventLogWritten);
            
        }

        class TestableEventLogger : EventLogLogger
        {
            protected override void RunElevated(SPSecurity.CodeToRunElevated method)
            {
                method.Invoke();
            }
        }
    }

    public class MockHierarchicalConfig : IHierarchicalConfig
    {
        public bool ThrowContextError;

        public bool ContainsReturnValue;
        public string GetBykeyReturnValue;

        public TValue GetByKey<TValue>(string key)
        {
            if (ThrowContextError)
                throw new NoSharePointContextException();

            return (TValue) (Object) GetBykeyReturnValue;
        }

        public TValue GetByKey<TValue>(string key, ConfigLevel level)
        {
            if (level == ConfigLevel.CurrentSPFarm)
            {
                return (TValue) (Object) GetBykeyReturnValue;
            }
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            if (ThrowContextError)
                throw new NoSharePointContextException();

            return ContainsReturnValue;
        }

        public bool ContainsKey(string key, ConfigLevel level)
        {
            if (level == ConfigLevel.CurrentSPFarm)
            {
                return ContainsReturnValue;
            }
            throw new NotImplementedException();
        }
    }
}