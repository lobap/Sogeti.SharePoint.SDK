//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


namespace ExecutionModel.ExceptionHandling.Tests
{
    using System;
    using ExecutionModels.Common.ExceptionHandling;
    using ExecutionModels.ExceptionHandling;
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.SharePoint.Common.ServiceLocation;
    using Microsoft.SharePoint.Administration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Practices.SharePoint.Common.Logging;

    [TestClass]
    public class BaseRobustExceptionHandlerFixture
    {
        [TestMethod]
        public void WillNotHideExceptionDetailsIfHandlingFailes()
        {
            // Arrange
            var originalException = new ArgumentException("MyMessage");
            var target = new TestableBaseRobustExceptionHandler();
            try
            {
                // Act
                target.HandleException(originalException);
                
                // Assert
                Assert.Fail();
            }
            catch (ExceptionHandlingException ex)
            {
                // Assert
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
            // Arrange
            SharePointServiceLocator.ReplaceCurrentServiceLocator(new ActivatingServiceLocator().RegisterTypeMapping<ILogger, BadLogger>());
            var originalException = new InvalidOperationException("Bad Error");
            var target = new TestableBaseRobustExceptionHandler();
            
            try
            {
                // Act
                target.CallGetLogger(originalException);
                
                // Assert
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

        protected override void WriteToOperationsLog(string message, int eventId, EventSeverity severity, string category)
        {
            throw new NotImplementedException();
        }

        protected override void WriteToDeveloperTrace(string message, int eventId, TraceSeverity severity, string category)
        {
            throw new NotImplementedException();
        }

        protected override void WriteToOperationsLog(string message, int eventId, string category)
        {
            throw new NotImplementedException();
        }

        protected override void WriteToOperationsLog(string message, int eventId, SandboxEventSeverity severity, string category)
        {
            throw new NotImplementedException();
        }

        protected override void WriteToDeveloperTrace(string message, int eventId, string category)
        {
            throw new NotImplementedException();
        }

        protected override void WriteToDeveloperTrace(string message, int eventId, SandboxTraceSeverity severity, string category)
        {
            throw new NotImplementedException();
        }
    }
}