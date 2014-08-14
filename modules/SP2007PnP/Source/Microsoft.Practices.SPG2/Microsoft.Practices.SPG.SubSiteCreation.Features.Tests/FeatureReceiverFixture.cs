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
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Workflow;

using TypeMock.ArrangeActAssert;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.Practices.SPG.Common.Configuration;

namespace Microsoft.Practices.SPG.SubSiteCreation.Features.Tests
{
    /// <summary>
    ///This is a test class for FeatureReceiverTest and is intended
    ///to contain all FeatureReceiverTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FeatureReceiverFixture
    {
        [TestCleanup]
        public void CleanUpMocks()
        {
            Isolate.CleanUp();
            SharePointServiceLocator.Reset();
        }

        /// <summary>
        ///A test for FeatureActivated
        ///</summary>
        [TestMethod()]
        public void FeatureActivatedTest()
        {
            SPFeatureReceiverProperties properties = Isolate.Fake.Instance<SPFeatureReceiverProperties>(Members.ReturnRecursiveFakes);
            SPFarm fakeSPFarm = Isolate.Fake.Instance<SPFarm>(Members.ReturnRecursiveFakes);
            Isolate.WhenCalled(() => SPFarm.Local).WillReturn(fakeSPFarm);
            Hashtable farmProperties = new Hashtable();
            Isolate.WhenCalled(() => fakeSPFarm.Properties).WillReturn(farmProperties);
            
            MockConfigManager.ReturnValue = "http://localhost";
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                .RegisterTypeMapping<IConfigManager, MockConfigManager>()
                .RegisterTypeMapping<ILogger, MockLogger>());

            SPWeb fakeSPWeb = Isolate.Fake.Instance<SPWeb>(Members.ReturnRecursiveFakes);
            Isolate.WhenCalled(() => properties.Feature.Parent).WillReturn(fakeSPWeb);

            SPWorkflowAssociation fakeAssociation = Isolate.Fake.Instance<SPWorkflowAssociation>(Members.ReturnRecursiveFakes);
            Isolate.WhenCalled(() => SPWorkflowAssociation.CreateListAssociation(null, "", null, null)).WillReturn(fakeAssociation);

            FeatureReceiver target = new FeatureReceiver();
            target.FeatureActivated(properties);

            Isolate.Verify.WasCalledWithAnyArguments(() => fakeSPWeb.Lists["Sub Site Creation Requests"].Update());
        }
    }

    public class MockLogger : ILogger
    {
        public void LogToOperations(string message)
        {

        }

        public void LogToOperations(string message, EventLogEntryType severity)
        {
        }

        public void LogToOperations(string message, int eventId)
        {
        }

        public void LogToOperations(string message, int eventId, EventLogEntryType severity)
        {
        }

        public void LogToOperations(string message, string category)
        {
        }

        public void LogToOperations(string message, int eventId, string category)
        {
        }

        public void LogToOperations(string message, int eventId, EventLogEntryType severity, string category)
        {
        }

        public void TraceToDeveloper(string message)
        {
        }

        public void TraceToDeveloper(string message, TraceSeverity severity)
        {
        }

        public void TraceToDeveloper(string message, int eventId)
        {
        }

        public void TraceToDeveloper(string message, string category)
        {
        }

        public void TraceToDeveloper(string message, int eventId, TraceSeverity severity, string category)
        {
        }

        public void TraceToDeveloper(string message, int eventId, string category)
        {
        }

        public void LogToOperations(Exception exception, string additionalMessage, int eventId, EventLogEntryType severity, string category)
        {
        }

        public void LogToOperations(Exception exception, string additionalMessage, int eventId)
        {
        }

        public void LogToOperations(Exception exception, int eventId, EventLogEntryType severity, string category)
        {
        }

        public void LogToOperations(Exception exception)
        {
        }

        public void LogToOperations(Exception exception, string additionalMessage)
        {
        }

        public void TraceToDeveloper(Exception exception, int eventId, TraceSeverity severity, string category)
        {
        }

        public void TraceToDeveloper(Exception exception, string additionalMessage, int eventId, TraceSeverity severity, string category)
        {
        }

        public void TraceToDeveloper(Exception exception)
        {
        }

        public void TraceToDeveloper(Exception exception, string additionalMessage)
        {
        }

        public void TraceToDeveloper(Exception exception, string additionalMessage, int eventId)
        {
        }
    }

    public class MockConfigManager : IConfigManager
    {
        public static string ReturnValue;

        public void RemoveKeyFromPropertyBag(string key, SPFarm propertyBag)
        {
            
        }

        public void RemoveKeyFromPropertyBag(string key, SPSite propertyBag)
        {
            
        }

        public void RemoveKeyFromPropertyBag(string key, SPWebApplication propertyBag)
        {
            
        }

        public void RemoveKeyFromPropertyBag(string key, SPWeb propertyBag)
        {
            
        }

        public bool ContainsKeyInPropertyBag(string key, SPFarm propertyBag)
        {
            return true;
        }

        public bool ContainsKeyInPropertyBag(string key, SPSite propertyBag)
        {
            return true;
        }

        public bool ContainsKeyInPropertyBag(string key, SPWebApplication propertyBag)
        {
            return true;
        }

        public bool ContainsKeyInPropertyBag(string key, SPWeb propertyBag)
        {
            return true;
        }

        public void SetInPropertyBag(string key, object value, SPWeb propertyBag)
        {
            
        }

        public void SetInPropertyBag(string key, object value, SPSite propertyBag)
        {
            
        }

        public void SetInPropertyBag(string key, object value, SPWebApplication propertyBag)
        {
            
        }

        public void SetInPropertyBag(string key, object value, SPFarm propertyBag)
        {
            
        }

        public TValue GetFromPropertyBag<TValue>(string key, SPWeb propertyBag)
        {
            return (TValue) (object) ReturnValue;
        }

        public TValue GetFromPropertyBag<TValue>(string key, SPSite propertyBag)
        {
            return (TValue)(object)ReturnValue;
        }

        public TValue GetFromPropertyBag<TValue>(string key, SPWebApplication propertyBag)
        {
            return (TValue)(object)ReturnValue;
        }

        public TValue GetFromPropertyBag<TValue>(string key, SPFarm propertyBag)
        {
            return (TValue)(object)ReturnValue;
        }
    }
}