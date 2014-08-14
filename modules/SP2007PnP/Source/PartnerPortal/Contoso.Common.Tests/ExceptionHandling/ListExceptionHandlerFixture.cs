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
using Contoso.Common.ExceptionHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint;
using TypeMock.ArrangeActAssert;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.SharePoint.Administration;
using Microsoft.Practices.SPG.Common.ServiceLocation;

namespace Contoso.Common.Tests.ExceptionHandling
{
    /// <summary>
    /// Summary description for ListExceptionHandlerFixture
    /// </summary>
    [TestClass]
    public class ListExceptionHandlerFixture
    {
        [TestInitialize]
        public void Initialize()
        {
        }


        [TestCleanup]
        public void TestCleanup()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(null);
        }

        [TestMethod]
        public void HandleListItemEventExceptionTest()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                                                                      .RegisterTypeMapping<ILogger, MockLogger>(InstantiationType.AsSingleton));

            MockLogger logger = SharePointServiceLocator.Current.GetInstance<ILogger>() as MockLogger;

            SPItemEventProperties mockSpItemEventProperties = Isolate.Fake.Instance<SPItemEventProperties>(Members.ReturnRecursiveFakes);
            Exception exception = new Exception("Unhandled Exception Occured.");

            ListExceptionHandler exceptionHandler = new ListExceptionHandler();
            exceptionHandler.HandleListItemEventException(exception, mockSpItemEventProperties);

            Assert.AreEqual("Unhandled Exception Occured.", mockSpItemEventProperties.ErrorMessage);

            Assert.AreEqual("Unhandled Exception Occured.", logger.ErrorMessage);
            Assert.IsTrue(mockSpItemEventProperties.Cancel);
        }

        [TestMethod]
        public void HandleListItemEventExceptionWithoutCancelingTest()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                                                                      .RegisterTypeMapping<ILogger, MockLogger>(InstantiationType.AsSingleton));

            MockLogger logger = SharePointServiceLocator.Current.GetInstance<ILogger>() as MockLogger;
            SPItemEventProperties mockSpItemEventProperties = Isolate.Fake.Instance<SPItemEventProperties>(Members.ReturnRecursiveFakes);
            Exception exception = new Exception("Unhandled Exception Occured.");

            ListExceptionHandler exceptionHandler = new ListExceptionHandler();
            exceptionHandler.HandleListItemEventException(exception, mockSpItemEventProperties, ListItemAction.Continue);

            Assert.AreEqual("Unhandled Exception Occured.", mockSpItemEventProperties.ErrorMessage);
            Assert.AreEqual("Unhandled Exception Occured.", logger.ErrorMessage);
            Assert.IsFalse(mockSpItemEventProperties.Cancel);
        }

        [TestMethod]
        public void HandleListItemEventExceptionCanSwapMessage()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                                                                      .RegisterTypeMapping<ILogger, MockLogger>(InstantiationType.AsSingleton));
            MockLogger logger = SharePointServiceLocator.Current.GetInstance<ILogger>() as MockLogger;

            SPItemEventProperties mockSpItemEventProperties = Isolate.Fake.Instance<SPItemEventProperties>(Members.ReturnRecursiveFakes);
            Exception exception = new Exception("Unhandled Exception Occured.");

            ListExceptionHandler exceptionHandler = new ListExceptionHandler();
            exceptionHandler.HandleListItemEventException(exception, mockSpItemEventProperties, "New Message");

            Assert.AreEqual("New Message", mockSpItemEventProperties.ErrorMessage);
            Assert.IsTrue(mockSpItemEventProperties.Cancel);
            Assert.AreEqual("Unhandled Exception Occured.", logger.ErrorMessage);
        }

        [TestMethod]
        [ExpectedException(typeof(ExceptionHandlingException))]
        public void HandleListEventExceptionIsRobust()
        {

            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator()
                                                                      .RegisterTypeMapping<ILogger, MockLoggerThatThrows>());


            SPItemEventProperties mockSpItemEventProperties = Isolate.Fake.Instance<SPItemEventProperties>(Members.ReturnRecursiveFakes);
            Exception exception = new Exception("Unhandled Exception Occured.");

            ListExceptionHandler exceptionHandler = new ListExceptionHandler();
            exceptionHandler.HandleListItemEventException(exception, mockSpItemEventProperties, "New Message");

        }
    }

    internal class MockLoggerThatThrows : BaseLogger
    {
        protected override void WriteToOperationsLog(string message, int eventId, EventLogEntryType severity, string category)
        {
            throw new NotImplementedException();
        }

        protected override void WriteToDeveloperTrace(string message, int eventId, TraceSeverity severity, string category)
        {
            throw new NotImplementedException();
        }
    }
}