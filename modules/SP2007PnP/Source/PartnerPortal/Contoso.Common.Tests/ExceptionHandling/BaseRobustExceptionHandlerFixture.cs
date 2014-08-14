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
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint.Administration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;
using Microsoft.Practices.SPG.Common.Logging;

namespace Contoso.Common.Tests.ExceptionHandling
{
    [TestClass]
    public class BaseRobustExceptionHandlerFixture
    {
        [TestCleanup]
        public void Cleanup()
        {
            Isolate.CleanUp();
        }

        [TestMethod]
        public void WillNotHideExceptionDetailsIfHandlingFailes()
        {
            var originalException = new ArgumentException("MyMessage");
            var target = new TestableBaseRobustExceptionHandler();
            try
            {
                target.HandleException(originalException);
                Assert.Fail();
            }
            catch (ExceptionHandlingException ex)
            {
                Assert.AreSame(originalException, ex.InnerException);
                Assert.IsTrue(originalException.Message.Contains("MyMessage"));
                Assert.IsInstanceOfType(ex.HandlingException, typeof(AccessViolationException));
            }
            catch(Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void IfGetLoggerFailsExceptionIsHandled()
        {
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator().RegisterTypeMapping<ILogger, BadLogger>());

            var originalException = new InvalidOperationException("Bad Error");

            var target = new TestableBaseRobustExceptionHandler();
            try
            {
                target.CallGetLogger(originalException);
                Assert.Fail();
            }
            catch (ExceptionHandlingException ex)
            {
                Assert.AreSame(originalException, ex.InnerException);
                Assert.IsInstanceOfType(ex.HandlingException, typeof(ActivationException));

                Assert.IsTrue(ex.Message.Contains("Bad Error"));
            }
            catch(Exception)
            {
                Assert.Fail();
            }
        }


    }

    class TestableBaseRobustExceptionHandler : BaseRobustExceptionHandler
    {
        public void HandleException(Exception originalException)
        {
            try
            {
                throw new AccessViolationException("suppose something goes wrong while handling this exception");
            }
            catch (Exception ex)
            {
                this.ThrowExceptionHandlingException(ex, originalException);
            }
        }

        public void CallGetLogger(Exception originalException)
        {
            GetLogger(originalException);
        }

    }

    class BadLogger : BaseLogger
    {
        public BadLogger()
        {
            throw new Exception("Problem contructing logger");
        }

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