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
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint.Administration;

namespace Microsoft.Practices.SPG.Common.Tests.Logging
{
    [TestClass]
    public class BaseLoggerFixture
    {

        [TestMethod]
        public void CanWriteErrorMessage()
        {
            var testableLogger = new TestableLogger();
            testableLogger.LogToOperations("Message");

            Assert.AreEqual("Message", testableLogger.Message);
            Assert.AreEqual(null, testableLogger.Category);
            Assert.AreEqual(0, testableLogger.EventID);
            Assert.AreEqual(EventLogEntryType.Error, testableLogger.EventLogSeverity);
        }

        [TestMethod]
        public void CanWriteErrorMessageWithEventID()
        {
            var testableLogger = new TestableLogger();
            testableLogger.LogToOperations("Message", 99);

            Assert.AreEqual("Message", testableLogger.Message);
            Assert.AreEqual(null, testableLogger.Category);
            Assert.AreEqual(99, testableLogger.EventID);
            Assert.AreEqual(EventLogEntryType.Error, testableLogger.EventLogSeverity);
        }

        [TestMethod]
        public void CanWriteErrorMessageWithSingleCategory()
        {
            var testableLogger = new TestableLogger();
            testableLogger.LogToOperations("Message", "Category");

            Assert.AreEqual("Message", testableLogger.Message);
            Assert.AreEqual("Category", testableLogger.Category);
            Assert.AreEqual(0, testableLogger.EventID);
            Assert.AreEqual(EventLogEntryType.Error, testableLogger.EventLogSeverity);
        }
        [TestMethod]
        public void CanWriteErrorMessageWithSingleCategoryAndEventID()
        {
            var testableLogger = new TestableLogger();
            testableLogger.LogToOperations("Message", 99, "Category");

            Assert.AreEqual("Message", testableLogger.Message);
            Assert.AreEqual("Category", testableLogger.Category);
            Assert.AreEqual(99, testableLogger.EventID);
            Assert.AreEqual(EventLogEntryType.Error, testableLogger.EventLogSeverity);
        }



        [TestMethod]
        public void CanWriteDiagnosticMessage()
        {
            var testableLogger = new TestableLogger();
            testableLogger.TraceToDeveloper("Message");

            Assert.AreEqual("Message", testableLogger.Message);
            Assert.AreEqual(null, testableLogger.Category);
            Assert.AreEqual(0, testableLogger.EventID);
            Assert.AreEqual(TraceSeverity.Medium, testableLogger.TraceSeverity);
        }

        [TestMethod]
        public void CanWriteDiagnosticMessageWithEventID()
        {
            var testableLogger = new TestableLogger();
            testableLogger.TraceToDeveloper("Message", 99);

            Assert.AreEqual("Message", testableLogger.Message);
            Assert.AreEqual(null, testableLogger.Category);
            Assert.AreEqual(99, testableLogger.EventID);
            Assert.AreEqual(TraceSeverity.Medium, testableLogger.TraceSeverity);
        }

        [TestMethod]
        public void CanWriteDiagnosticMessageWithSingleCategory()
        {
            var testableLogger = new TestableLogger();
            testableLogger.TraceToDeveloper("Message", "Category");

            Assert.AreEqual("Message", testableLogger.Message);
            Assert.AreEqual("Category", testableLogger.Category);
            Assert.AreEqual(0, testableLogger.EventID);
            Assert.AreEqual(TraceSeverity.Medium, testableLogger.TraceSeverity);
        }
        [TestMethod]
        public void CanWriteDiagnosticMessageWithSingleCategoryAndEventID()
        {
            var testableLogger = new TestableLogger();
            testableLogger.TraceToDeveloper("Message", 99, "Category");

            Assert.AreEqual("Message", testableLogger.Message);
            Assert.AreEqual("Category", testableLogger.Category);
            Assert.AreEqual(99, testableLogger.EventID);
            Assert.AreEqual(TraceSeverity.Medium, testableLogger.TraceSeverity);
        }

        [TestMethod]
        public void ExceptionMessagesAreFormatted()
        {
            var testableLogger = new TestableLogger();

            try
            {
                ThrowException();
            }
            catch (Exception ex)
            {
                testableLogger.LogToOperations(ex, 99, EventLogEntryType.Error, "Blurp");
            }

            // Make sure all the exception messages are displayed
            Assert.IsTrue(testableLogger.Message.Contains("Message1"));
            Assert.IsTrue(testableLogger.Message.Contains("Message2"));
            Assert.IsTrue(testableLogger.Message.Contains("Message3"));

            // make sure the stacktraces are displayed
            Assert.IsTrue(testableLogger.Message.Contains("ThrowInnerException()"));
            Assert.IsTrue(testableLogger.Message.Contains("ThrowException()"));

            // make sure the exception types are displayed
            Assert.IsTrue(testableLogger.Message.Contains("ArgumentException"));
            Assert.IsTrue(testableLogger.Message.Contains("AccessViolationException"));
            Assert.IsTrue(testableLogger.Message.Contains("InvalidOperationException"));

            Assert.IsTrue(testableLogger.Message.Contains("MyDataKey"));
            Assert.IsTrue(testableLogger.Message.Contains("MyDataValue"));
        }

        [TestMethod]
        public void ExceptionsInDataPropertyShouldBeFormatted()
        {
            
            var exception = new Exception("MyMessage");
            Exception innerException = null;
            try
            {
                ThrowInnerException();
            }
            catch (Exception ex)
            {
                innerException = ex;
            }
            exception.Data.Add("OtherException", innerException);

            var testableLogger = new TestableLogger();
            testableLogger.LogToOperations(exception, 0, EventLogEntryType.Error, null);

            Assert.IsTrue(testableLogger.Message.Contains("OtherException"));
            Assert.IsTrue(testableLogger.Message.Contains("InvalidOperationException"));
            Assert.IsTrue(testableLogger.Message.Contains("ThrowInnerException()"));

            Assert.IsTrue(testableLogger.Message.Contains("MyDataKey"));
            Assert.IsTrue(testableLogger.Message.Contains("MyDataValue"));

        }

        private void ThrowException()
        {
            try
            {
                try
                {
                    ThrowInnerException();
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Message2", ex);
                }
            }
            catch (Exception ex)
            {

                throw new AccessViolationException("Message3", ex);
            }
        }

        private void ThrowInnerException()
        {
            var ex = new InvalidOperationException("Message1");
            ex.Data.Add("MyDataKey", "MyDataValue");
            throw ex;
        }
    }

    class TestableLogger : BaseLogger
    {
        public string Message;
        public string Category;
        public int EventID;
        public TraceSeverity TraceSeverity;
        public EventLogEntryType EventLogSeverity;

        protected override void WriteToOperationsLog(string message, int eventId, EventLogEntryType severity, string category)
        {
            this.Message = message;
            this.Category = category;
            this.EventID = eventId;
            this.EventLogSeverity = severity;
        }

        protected override void WriteToDeveloperTrace(string message, int eventId, TraceSeverity severity, string category)
        {
            this.Message = message;
            this.Category = category;
            this.EventID = eventId;
            this.TraceSeverity = severity;
        }
    }
}