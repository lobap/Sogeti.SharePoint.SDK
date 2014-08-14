//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Diagnostics;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.SharePoint.Administration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.SharePoint.Common.Tests.Logging
{
    [TestClass]
    public class BaseLoggerTests
    {

        [TestMethod]
        public void WriteDiagnosticMessageSucceeds()
        {
            //Arrange
            string testMessage = "Test Message";
            var mockLogger = new TestableBaseLogger();

            //Act
            mockLogger.TraceToDeveloper(testMessage);

            //Assert
            Assert.AreEqual(testMessage, mockLogger.Messages[0].Message);
            Assert.IsNull(mockLogger.Messages[0].Category);
            Assert.AreEqual(0, mockLogger.Messages[0].EventId);
            Assert.AreEqual(TraceSeverity.None, mockLogger.Messages[0].TraceSeverity);
        }

        [TestMethod]
        public void WriteDiagnosticMessageWithExceptionSucceeds()
        {
            //Arrange
            string testParam = "test param";
            var mockLogger = new TestableBaseLogger();
            var exception = new ArgumentException(TestsConstants.TestGuidName, testParam);

            //Act
            mockLogger.TraceToDeveloper(exception);

            //Assert
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(TestsConstants.TestGuidName));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("ArgumentException"));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(testParam));
            Assert.IsNull(mockLogger.Messages[0].Category);
            Assert.AreEqual(0, mockLogger.Messages[0].EventId);
            Assert.AreEqual(TraceSeverity.None, mockLogger.Messages[0].TraceSeverity);
        }

        [TestMethod]
        public void WriteDiagnosticMessageWithExceptionAndMessageSucceeds()
        {
            //Arrange
            string testParam = "test param";
            string testMessage = "test message";
            var mockLogger = new TestableBaseLogger();
            var exception = new ArgumentException(TestsConstants.TestGuidName, testParam);

            //Act
            mockLogger.TraceToDeveloper(exception, testMessage);

            //Assert
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(TestsConstants.TestGuidName));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("ArgumentException"));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(testParam));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(testMessage)); 
            Assert.IsNull(mockLogger.Messages[0].Category);
            Assert.AreEqual(0, mockLogger.Messages[0].EventId);
            Assert.AreEqual(TraceSeverity.None, mockLogger.Messages[0].TraceSeverity);
        }

        [TestMethod]
        public void WriteDiagnosticMessageWithTraceSeveritySucceeds()
        {
            //Arrange
            string testMessage = TestsConstants.TestGuidName;
            var mockLogger = new TestableBaseLogger();

            //Act
            mockLogger.TraceToDeveloper(testMessage, TraceSeverity.Monitorable);

            //Assert
            Assert.AreEqual(testMessage, mockLogger.Messages[0].Message);
            Assert.IsNull(mockLogger.Messages[0].Category);
            Assert.AreEqual(0, mockLogger.Messages[0].EventId);
            Assert.AreEqual(TraceSeverity.Monitorable, mockLogger.Messages[0].TraceSeverity);
        }


        [TestMethod]
        public void WriteDiagnosticMessageWithSandboxTraceSeveritySucceeds()
        {
            //Arrange
            string testMessage = TestsConstants.TestGuidName;
            var mockLogger = new TestableBaseLogger();

            //Act
            mockLogger.TraceToDeveloper(testMessage, SandboxTraceSeverity.Verbose);

            //Assert
            Assert.AreEqual(testMessage, mockLogger.Messages[0].Message);
            Assert.IsNull(mockLogger.Messages[0].Category);
            Assert.AreEqual(0, mockLogger.Messages[0].EventId);
            Assert.AreEqual(SandboxTraceSeverity.Verbose, mockLogger.Messages[0].SandboxTraceSeverity);
        }


        [TestMethod]
        public void WriteDiagnosticMessageWithEventIDSucceeds()
        {
            //Arrange
            string testMessage = "Test Message";
            var mockLogger = new TestableBaseLogger();
            int testEventId = 99;

            //Act
            mockLogger.TraceToDeveloper(testMessage, testEventId);

            //Assert
            Assert.AreEqual(testMessage, mockLogger.Messages[0].Message);
            Assert.IsNull(mockLogger.Messages[0].Category);
            Assert.AreEqual(testEventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(TraceSeverity.None, mockLogger.Messages[0].TraceSeverity);
        }

        [TestMethod]
        public void WriteDiagnosticMessageWithSingleCategorySucceeds()
        {
            //Arrange
            string testMessage = "Test Message";
            var mockLogger = new TestableBaseLogger();

            //Act
            mockLogger.TraceToDeveloper(testMessage, TestsConstants.AreasCategories);

            //Assert
            Assert.AreEqual(testMessage, mockLogger.Messages[0].Message);
            Assert.AreEqual(TestsConstants.AreasCategories, mockLogger.Messages[0].Category);
            Assert.AreEqual(0, mockLogger.Messages[0].EventId);
            Assert.AreEqual(TraceSeverity.None, mockLogger.Messages[0].TraceSeverity);
        }

        [TestMethod]
        public void WriteDiagnosticMessageWithCategoryAndEventIDSucceeds()
        {
            //Arrange
            string testMessage = "Test Message";
            var mockLogger = new TestableBaseLogger();
            int testEventId = 99;

            //Act
            mockLogger.TraceToDeveloper(testMessage, testEventId, TestsConstants.AreasCategories);

            //Assert
            Assert.AreEqual(testMessage, mockLogger.Messages[0].Message);
            Assert.AreEqual(TestsConstants.AreasCategories, mockLogger.Messages[0].Category);
            Assert.AreEqual(testEventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(TraceSeverity.None, mockLogger.Messages[0].TraceSeverity);
        }

        [TestMethod]
        public void WriteDiagnosticMessageWithExceptionMessageAndEventIdSucceeds()
        {
            //Arrange
            string testParam = "test param";
            string testMessage = "test message";
            int eventId = 988;
            var mockLogger = new TestableBaseLogger();
            var exception = new ArgumentException(TestsConstants.TestGuidName, testParam);

            //Act
            mockLogger.TraceToDeveloper(exception, testMessage, eventId);

            //Assert
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(TestsConstants.TestGuidName));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("ArgumentException"));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(testParam));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(testMessage));
            Assert.IsNull(mockLogger.Messages[0].Category);
            Assert.AreEqual(eventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(TraceSeverity.None, mockLogger.Messages[0].TraceSeverity);
        }

        [TestMethod]
        public void WriteDiagnosticMessageWithCategoryEventIDAndSeveritySucceeds()
        {
            //Arrange
            string testMessage = "Test Message";
            var mockLogger = new TestableBaseLogger();
            int testEventId = 99;

            //Act
            mockLogger.TraceToDeveloper(testMessage, testEventId, TraceSeverity.Monitorable, TestsConstants.AreasCategories);

            //Assert
            Assert.AreEqual(testMessage, mockLogger.Messages[0].Message);
            Assert.AreEqual(TestsConstants.AreasCategories, mockLogger.Messages[0].Category);
            Assert.AreEqual(testEventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(TraceSeverity.Monitorable, mockLogger.Messages[0].TraceSeverity);
        }

        [TestMethod]
        public void WriteDiagnosticMessageWithCategoryEventIDAndSandboxTraceSeveritySucceeds()
        {
            //Arrange
            string testMessage = "Test Message";
            var mockLogger = new TestableBaseLogger();
            int testEventId = 99;

            //Act
            mockLogger.TraceToDeveloper(testMessage, testEventId, SandboxTraceSeverity.Monitorable, TestsConstants.AreasCategories);

            //Assert
            Assert.AreEqual(testMessage, mockLogger.Messages[0].Message);
            Assert.AreEqual(TestsConstants.AreasCategories, mockLogger.Messages[0].Category);
            Assert.AreEqual(testEventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(SandboxTraceSeverity.Monitorable, mockLogger.Messages[0].SandboxTraceSeverity);
        }


        [TestMethod]
        public void WriteDiagnosticMessageWithExceptionMessageEventIdAndSeveritySucceeds()
        {
            //Arrange
            string testParam = "test param";
            int eventId = 988;
            var mockLogger = new TestableBaseLogger();
            var exception = new ArgumentException(TestsConstants.TestGuidName, testParam);

            //Act
            mockLogger.TraceToDeveloper(exception, eventId, TraceSeverity.Unexpected, TestsConstants.AreasCategories);

            //Assert
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(TestsConstants.TestGuidName));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("ArgumentException"));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(testParam));
            Assert.AreEqual(TestsConstants.AreasCategories, mockLogger.Messages[0].Category);
            Assert.AreEqual(eventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(TraceSeverity.Unexpected, mockLogger.Messages[0].TraceSeverity);
        }


        [TestMethod]
        public void WriteDiagnosticMessageWithExceptionEventIdCategoryAndSandboxTraceSeveritySucceeds()
        {
            //Arrange
            string testParam = "test param";
            int eventId = 988;
            var mockLogger = new TestableBaseLogger();
            var exception = new ArgumentException(TestsConstants.TestGuidName, testParam);

            //Act
            mockLogger.TraceToDeveloper(exception, eventId, SandboxTraceSeverity.Monitorable, TestsConstants.AreasCategories);

            //Assert
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(TestsConstants.TestGuidName));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("ArgumentException"));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(testParam));
            Assert.AreEqual(TestsConstants.AreasCategories, mockLogger.Messages[0].Category);
            Assert.AreEqual(eventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(SandboxTraceSeverity.Monitorable, mockLogger.Messages[0].SandboxTraceSeverity);
        }

        [TestMethod]
        public void WriteDiagnosticMessageWithExceptionMessageEventIdCategoryAndTraceSeveritySucceeds()
        {
            //Arrange
            string testParam = "test param";
            string testMessage = "test message";
            int eventId = 988;
            var mockLogger = new TestableBaseLogger();
            var exception = new ArgumentException(TestsConstants.TestGuidName, testParam);

            //Act
            mockLogger.TraceToDeveloper(exception, testMessage, eventId, TraceSeverity.Unexpected, TestsConstants.AreasCategories);

            //Assert
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(TestsConstants.TestGuidName));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("ArgumentException"));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(testParam));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(testMessage)); 
            Assert.AreEqual(TestsConstants.AreasCategories, mockLogger.Messages[0].Category);
            Assert.AreEqual(eventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(TraceSeverity.Unexpected, mockLogger.Messages[0].TraceSeverity);
        }

        [TestMethod]
        public void WriteDiagnosticMessageWithExceptionMessageEventIdCategoryAndSandboxTraceSeveritySucceeds()
        {
            //Arrange
            string testParam = "test param";
            string testMessage = "test message";
            int eventId = 988;
            var mockLogger = new TestableBaseLogger();
            var exception = new ArgumentException(TestsConstants.TestGuidName, testParam);

            //Act
            mockLogger.TraceToDeveloper(exception, testMessage, eventId, SandboxTraceSeverity.Monitorable, TestsConstants.AreasCategories);

            //Assert
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(TestsConstants.TestGuidName));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("ArgumentException"));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(testParam));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(testMessage));
            Assert.AreEqual(TestsConstants.AreasCategories, mockLogger.Messages[0].Category);
            Assert.AreEqual(eventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(SandboxTraceSeverity.Monitorable, mockLogger.Messages[0].SandboxTraceSeverity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteDiagnosticMessageWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();

            //Act
            mockLogger.TraceToDeveloper((string)null);

            //Assert caught by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteDiagnosticMessageWithNullExceptionThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();

            //Act
            mockLogger.TraceToDeveloper((Exception)null);

            //Assert caught by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteDiagnosticMessageEventIdWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;

            //Act
            mockLogger.TraceToDeveloper((string)null, eventId);

            //Assert caught by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteDiagnosticExceptionMessagedWithNullExceptionThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            string message = "test messag";

            //Act
            mockLogger.TraceToDeveloper((Exception)null, message);

            //Assert caught by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteDiagnosticExceptionMessagedWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            var exception = new Exception("test exception");

            //Act
            mockLogger.TraceToDeveloper(exception, null);

            //Assert caught by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteDiagnosticMessageTraceSeverityWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();

            //Act
            mockLogger.TraceToDeveloper((string)null, TraceSeverity.High);

            //Assert caught by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteDiagnosticMessageSandboxTraceSeverityWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();

            //Act
            mockLogger.TraceToDeveloper((string)null, SandboxTraceSeverity.High);

            //Assert caught by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteDiagnosticMessageSandboxEventIdWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;

            //Act
            mockLogger.TraceToDeveloper((string)null, eventId);

            //Assert caught by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteDiagnosticMessageCategoryWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();

            //Act
            mockLogger.TraceToDeveloper((string)null, TestsConstants.AreasCategories);

            //Assert caught by exception
        }

        [TestMethod]
        public void TraceToDeveloper_WriteNullCategoryDoesntThrowArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();

            //Act
            mockLogger.TraceToDeveloper(TestsConstants.TestGuidName, null);

            //Assert caught by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteDiagnosticMessageEventIdCategoryWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;

            //Act
            mockLogger.TraceToDeveloper((string)null, eventId, TestsConstants.AreasCategories);

            //Assert caught by exception
        }

        [TestMethod]
        public void TraceToDeveloper_DoesNotThrowArgumentNullExceptionWithNullCategory_WithEventId()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;

            //Act
            mockLogger.TraceToDeveloper(TestsConstants.TestGuidName, eventId,null);

            //Assert caught by exception
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteDiagnosticMessageEventIdSeverityCategoryWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;

            //Act
            mockLogger.TraceToDeveloper((string)null, eventId, TraceSeverity.High, TestsConstants.AreasCategories);

            //Assert caught by exception
        }

        [TestMethod]
        public void TraceToDeveloper_DoesNotThrowArgumentNullExceptionWithNullCategory_WithEventIdAndSeverity()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;

            //Act
            mockLogger.TraceToDeveloper(TestsConstants.TestGuidName, eventId, TraceSeverity.High, null);

            //Assert caught by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteDiagnosticMessageEventIdSandboxSeverityCategoryWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;

            //Act
            mockLogger.TraceToDeveloper((string) null, eventId, SandboxTraceSeverity.High, TestsConstants.AreasCategories);

            //Assert caught by exception
        }

        [TestMethod]
        public void TraceToDeveloper_DoesNotThrowArgumentNullExceptionWithNullCategory_WithEventIdSandboxSeverity()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;

            //Act
            mockLogger.TraceToDeveloper(TestsConstants.TestGuidName, eventId, SandboxTraceSeverity.High, null);

            //Assert caught by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteDiagnosticExceptionEventIdSeverityCategoryWithNullExceptionThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;

            //Act
            mockLogger.TraceToDeveloper((Exception) null, eventId, TraceSeverity.High, TestsConstants.AreasCategories);

            //Assert caught by exception
        }

        [TestMethod]
        public void TraceToDeveloper_DoesNotThrowArgumentNullExceptionWithNullCategory_WithEventIdSeverity()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;
            var ex = new Exception("test exception");

            //Act
            mockLogger.TraceToDeveloper(ex, eventId, TraceSeverity.High, null);

            //Assert caught by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteDiagnosticExceptionEventIdSandboxSeverityCategoryWithNullExceptionThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;

            //Act
            mockLogger.TraceToDeveloper((Exception)null, eventId, SandboxTraceSeverity.High, TestsConstants.AreasCategories);

            //Assert caught by exception
        }

        [TestMethod]
        public void TraceToDeveloper_DoesNotThrowExceptionOnNullCategory_WithEvenIdAndSeverity()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;
            var ex = new Exception("test exception");

            //Act
            mockLogger.TraceToDeveloper(ex, eventId, SandboxTraceSeverity.High, null);

            //Assert caught by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteDiagnosticExceptionMessageEventIdSeverityCategoryWithNullExceptionThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;
            string message = "test message";

            //Act
            mockLogger.TraceToDeveloper((Exception)null, message, eventId, TraceSeverity.High, TestsConstants.AreasCategories);

            //Assert caught by exception
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteDiagnosticExceptionMessageEventIdSeverityCategoryWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;
            var ex = new Exception("test exception");

            //Act
            mockLogger.TraceToDeveloper(ex, null, eventId, TraceSeverity.High, TestsConstants.AreasCategories);

            //Assert caught by exception
        }

        [TestMethod]
        public void TraceToDeveloper_DoesNotThrowArgumentNullExceptionWithNullCategory_WithExceptionMessageEventIdAndSeverity()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;
            var ex = new Exception("test exception");
            string message = "test message";

            //Act
            mockLogger.TraceToDeveloper(ex, message, eventId, TraceSeverity.High, null);

            //Assert caught by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteDiagnosticExceptionMessageEventIdSandboxSeverityCategoryWithNullExceptionThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;
            string message = "test message";

            //Act
            mockLogger.TraceToDeveloper((Exception)null, message, eventId, SandboxTraceSeverity.High, TestsConstants.AreasCategories);

            //Assert caught by exception
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteDiagnosticExceptionMessageEventIdSandboxSeverityCategoryWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;
            var ex = new Exception("test exception");

            //Act
            mockLogger.TraceToDeveloper(ex, null, eventId, SandboxTraceSeverity.High, TestsConstants.AreasCategories);

            //Assert caught by exception
        }

        [TestMethod]
        public void TraceToDeveloper_DoesNotThrowArgumentNullExceptionWithNullCategory_WithSandboxSeverityExceptionMessageAndEventId()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;
            var ex = new Exception("test exception");
            string message = "test message";

            //Act
            mockLogger.TraceToDeveloper(ex, message, eventId, SandboxTraceSeverity.High, null);

            //Assert caught by exception
        }


        [TestMethod]
        public void WriteLogMessageSucceeds()
        {
            //Arrange
            string testMessage = "Test Message";
            var mockLogger = new TestableBaseLogger();
            
            //Act
            mockLogger.LogToOperations(testMessage);

            //Assert
            Assert.IsTrue(mockLogger.Messages.Count == 1);
            Assert.AreEqual(testMessage, mockLogger.Messages[0].Message);
            Assert.IsNull(mockLogger.Messages[0].Category);
            Assert.AreEqual(0, mockLogger.Messages[0].EventId);
            Assert.AreEqual(EventSeverity.None, mockLogger.Messages[0].EventSeverity);
        }

        [TestMethod]
        public void WriteLogExceptionSucceeds()
        {
            //Arrange
           var testException = new Exception(TestsConstants.TestGuidName);
            var mockLogger = new TestableBaseLogger();

            //Act
            mockLogger.LogToOperations(testException);

            //Assert
            Assert.IsTrue(mockLogger.Messages.Count == 1);
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(TestsConstants.TestGuidName));
            Assert.IsNull(mockLogger.Messages[0].Category);
            Assert.AreEqual(0, mockLogger.Messages[0].EventId);
            Assert.AreEqual(EventSeverity.None, mockLogger.Messages[0].EventSeverity);
        }

        [TestMethod]
        public void WriteLogMessageWithEventSeveritySucceeds()
        {
            //Arrange
            string testMessage = "Test Message";
            var mockLogger = new TestableBaseLogger();

            //act
            mockLogger.LogToOperations(testMessage, EventSeverity.ErrorCritical);

            //assert
            Assert.IsTrue(mockLogger.Messages.Count == 1);
            Assert.AreEqual(testMessage, mockLogger.Messages[0].Message);
            Assert.IsNull(mockLogger.Messages[0].Category);
            Assert.AreEqual(0, mockLogger.Messages[0].EventId);
            Assert.AreEqual(EventSeverity.ErrorCritical, mockLogger.Messages[0].EventSeverity);
        }

        [TestMethod]
        public void WriteLogMessageWithSandboxEventSeveritySucceeds()
        {
            //Arrange
            string testMessage = "Test Message";
            var mockLogger = new TestableBaseLogger();

            //act
            mockLogger.LogToOperations(testMessage, SandboxEventSeverity.ErrorCritical);

            //assert
            Assert.IsTrue(mockLogger.Messages.Count == 1);
            Assert.AreEqual(testMessage, mockLogger.Messages[0].Message);
            Assert.IsNull(mockLogger.Messages[0].Category);
            Assert.AreEqual(0, mockLogger.Messages[0].EventId);
            Assert.AreEqual(SandboxEventSeverity.ErrorCritical, mockLogger.Messages[0].SandboxEventSeverity);
        } 

        [TestMethod]
        public void WriteLogMessageWithEventIDSucceeds()
        {
            //Arrange
            string testMessage = "Test Message";
            var mockLogger = new TestableBaseLogger();
            int testEventId = 99;

            //act
            mockLogger.LogToOperations(testMessage, testEventId);

            //assert
            Assert.IsTrue(mockLogger.Messages.Count == 1);
            Assert.AreEqual(testMessage, mockLogger.Messages[0].Message);
            Assert.IsNull(mockLogger.Messages[0].Category);
            Assert.AreEqual(testEventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(EventSeverity.None, mockLogger.Messages[0].EventSeverity); 
        }

        [TestMethod]
         public void WriteLogMessageWithCategorySucceeds()
        {
            //Arrange
            string testMessage = "Test Message";
            var mockLogger = new TestableBaseLogger();

            //Act
            mockLogger.LogToOperations(testMessage, TestsConstants.AreasCategories);

            //Assert
            Assert.AreEqual(testMessage, mockLogger.Messages[0].Message);
            Assert.AreEqual(TestsConstants.AreasCategories, mockLogger.Messages[0].Category);
            Assert.AreEqual(0, mockLogger.Messages[0].EventId);
            Assert.AreEqual(EventSeverity.None, mockLogger.Messages[0].EventSeverity);
        }

        [TestMethod]
        public void WriteLogMessageWithEventIdAndSeveritySucceeds()
        {
            //Arrange
            string testMessage = "Test Message";
            var mockLogger = new TestableBaseLogger();
            int testEventId = 99;

            //Act
            mockLogger.LogToOperations(testMessage, testEventId, EventSeverity.Information);

            //Assert
            Assert.AreEqual(testMessage, mockLogger.Messages[0].Message);
            Assert.IsNull(mockLogger.Messages[0].Category);
            Assert.AreEqual(testEventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(EventSeverity.Information, mockLogger.Messages[0].EventSeverity);
        }

        [TestMethod]
        public void WriteLogMessageWithEventIdAndSandboxSeveritySucceeds()
        {
            //Arrange
            string testMessage = "Test Message";
            var mockLogger = new TestableBaseLogger();
            int testEventId = 99;

            //Act
            mockLogger.LogToOperations(testMessage, testEventId, SandboxEventSeverity.Information);

            //Assert
            Assert.AreEqual(testMessage, mockLogger.Messages[0].Message);
            Assert.IsNull(mockLogger.Messages[0].Category);
            Assert.AreEqual(testEventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(SandboxEventSeverity.Information, mockLogger.Messages[0].SandboxEventSeverity);
        }


        [TestMethod]
        public void WriteLogMessageWithCategoryAndEventIDSucceeds()
        {
            //Arrange
            string testMessage = "Test Message";
            var mockLogger = new TestableBaseLogger();
            int testEventId = 99;

            //Act
            mockLogger.LogToOperations(testMessage, testEventId, TestsConstants.AreasCategories);

            //Assert
            Assert.AreEqual(testMessage, mockLogger.Messages[0].Message);
            Assert.AreEqual(TestsConstants.AreasCategories, mockLogger.Messages[0].Category);
            Assert.AreEqual(testEventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(EventSeverity.None, mockLogger.Messages[0].EventSeverity);
        }

        [TestMethod]
        public void WriteLogMessageWithCategorySeverityAndEventIDSucceeds()
        {
            //Arrange
            string testMessage = "Test Message";
            var mockLogger = new TestableBaseLogger();
            int testEventId = 99;

            //Act
            mockLogger.LogToOperations(testMessage, testEventId, EventSeverity.Information, TestsConstants.AreasCategories);

            //Assert
            Assert.AreEqual(testMessage, mockLogger.Messages[0].Message);
            Assert.AreEqual(TestsConstants.AreasCategories, mockLogger.Messages[0].Category);
            Assert.AreEqual(testEventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(EventSeverity.Information, mockLogger.Messages[0].EventSeverity);
        }

        [TestMethod]
        public void WriteLogMessageWithCategorySandboxSeverityAndEventIDSucceeds()
        {
            //Arrange
            string testMessage = "Test Message";
            var mockLogger = new TestableBaseLogger();
            int testEventId = 99;

            //Act
            mockLogger.LogToOperations(testMessage, testEventId, SandboxEventSeverity.Information, TestsConstants.AreasCategories);

            //Assert
            Assert.AreEqual(testMessage, mockLogger.Messages[0].Message);
            Assert.AreEqual(TestsConstants.AreasCategories, mockLogger.Messages[0].Category);
            Assert.AreEqual(testEventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(SandboxEventSeverity.Information, mockLogger.Messages[0].SandboxEventSeverity);
        }

        [TestMethod]
        public void WriteLogExceptionMessageAndEventIdSucceeds()
        {
            //Arrange
            string testMessage = "Test Message";
            var mockLogger = new TestableBaseLogger();
            int testEventId = 99;
            var ex = new Exception(TestsConstants.TestGuidName);

            //Act
            mockLogger.LogToOperations(ex, testMessage, testEventId);

            //Assert
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(testMessage));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(TestsConstants.TestGuidName)); 
            Assert.IsNull(mockLogger.Messages[0].Category);
            Assert.AreEqual(testEventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(EventSeverity.None, mockLogger.Messages[0].EventSeverity);
        }

        [TestMethod]
        public void WriteLogExceptionSeverityCategoryAndEventIdSucceeds()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int testEventId = 99;
            var ex = new Exception(TestsConstants.TestGuidName);

            //Act
            mockLogger.LogToOperations(ex, testEventId, EventSeverity.Information, TestsConstants.AreasCategories);

            //Assert
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(TestsConstants.TestGuidName));
            Assert.AreEqual(TestsConstants.AreasCategories, mockLogger.Messages[0].Category);
            Assert.AreEqual(testEventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(EventSeverity.Information, mockLogger.Messages[0].EventSeverity);
        }

        [TestMethod]
        public void WriteLogExceptionSandboxSeverityCategoryAndEventIdSucceeds()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int testEventId = 99;
            var ex = new Exception(TestsConstants.TestGuidName);

            //Act
            mockLogger.LogToOperations(ex, testEventId, SandboxEventSeverity.Information, TestsConstants.AreasCategories);

            //Assert
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(TestsConstants.TestGuidName));
            Assert.AreEqual(TestsConstants.AreasCategories, mockLogger.Messages[0].Category);
            Assert.AreEqual(testEventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(SandboxEventSeverity.Information, mockLogger.Messages[0].SandboxEventSeverity);
        }

        [TestMethod]
        public void WriteLogExceptionMessageSeverityCategoryAndEventIdSucceeds()
        {
            //Arrange
            string testMessage = "Test Message";
            var mockLogger = new TestableBaseLogger();
            int testEventId = 99;
            var ex = new Exception(TestsConstants.TestGuidName);

            //Act
            mockLogger.LogToOperations(ex, testMessage, testEventId, EventSeverity.Information, TestsConstants.AreasCategories);

            //Assert
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(testMessage));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(TestsConstants.TestGuidName));
            Assert.AreEqual(TestsConstants.AreasCategories, mockLogger.Messages[0].Category);
            Assert.AreEqual(testEventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(EventSeverity.Information, mockLogger.Messages[0].EventSeverity);
        }

        [TestMethod]
        public void WriteLogMessageExceptionSandboxSeverityCategoryAndEventIdSucceeds()
        {
            //Arrange
            string testMessage = "Test Message";
            var mockLogger = new TestableBaseLogger();
            int testEventId = 99;
            var ex = new Exception(TestsConstants.TestGuidName);

            //Act
            mockLogger.LogToOperations(ex, testMessage, testEventId, SandboxEventSeverity.Information, TestsConstants.AreasCategories);

            //Assert
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(testMessage));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(TestsConstants.TestGuidName));
            Assert.AreEqual(TestsConstants.AreasCategories, mockLogger.Messages[0].Category);
            Assert.AreEqual(testEventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(SandboxEventSeverity.Information, mockLogger.Messages[0].SandboxEventSeverity);
        }

        [TestMethod]
        public void WriteLogExceptionMessageSandboxSeverityCategoryAndEventIdSucceeds()
        {
            //Arrange
            string testMessage = "Test Message";
            var mockLogger = new TestableBaseLogger();
            int testEventId = 99;
            var ex = new Exception(TestsConstants.TestGuidName);

            //Act
            mockLogger.LogToOperations(ex, testMessage, testEventId, SandboxEventSeverity.Information, TestsConstants.AreasCategories);

            //Assert
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(testMessage));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains(TestsConstants.TestGuidName));
            Assert.AreEqual(TestsConstants.AreasCategories, mockLogger.Messages[0].Category);
            Assert.AreEqual(testEventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(SandboxEventSeverity.Information, mockLogger.Messages[0].SandboxEventSeverity);
        }

        [TestMethod]
        public void ExceptionMessagesAreFormatted()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int testEventId = 99;

            //Act
            try
            {
                ThrowException();
            }
            catch (Exception ex)
            {
                mockLogger.LogToOperations(ex, testEventId, EventSeverity.Error, TestsConstants.AreasCategories);
            }

            //Assert

            //// Make sure all the exception messages are displayed
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("Message1"));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("Message2"));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("Message3"));

            //// make sure the stacktraces are displayed
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("ThrowInnerException()"));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("ThrowException()"));

            //// make sure the exception types are displayed
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("ArgumentException"));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("AccessViolationException"));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("InvalidOperationException"));

            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("MyDataKey"));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("MyDataValue"));

            Assert.AreEqual(testEventId, mockLogger.Messages[0].EventId);
            Assert.AreEqual(TestsConstants.AreasCategories, mockLogger.Messages[0].Category);
        }

        [TestMethod]
        public void ExceptionsInDataPropertyShouldBeFormatted()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
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

            //Act
            mockLogger.LogToOperations(exception);

            //Assert
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("OtherException"));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("InvalidOperationException"));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("ThrowInnerException()"));

            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("MyDataKey"));
            Assert.IsTrue(mockLogger.Messages[0].Message.Contains("MyDataValue"));

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogMessageWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();

            //Act
            mockLogger.LogToOperations((string) null);

            //Assert caught by expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogExceptionWithNullExceptionThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();

            //Act
            mockLogger.LogToOperations((Exception)null);
 
            //Assert caught by expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogMessageSeverityWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();

            //Act
            mockLogger.LogToOperations((string)null, EventSeverity.Information);

            //Assert caught by expected exception.

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogExceptionSandboxSeverityWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();

            //Act
            mockLogger.LogToOperations((string)null, SandboxEventSeverity.Information);

            //Assert caught by expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogMessageEventIdWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;

            //Act
            mockLogger.LogToOperations((string)null, eventId);

            //Assert caught by expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogMessageCategoryWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();

            //Act
            mockLogger.LogToOperations((string)null, TestsConstants.AreasCategories);

            //Assert caught by expected exception.
        }

        [TestMethod]
        public void LogToOperations_DoesNotThrowArgumentNullExceptionWithNullCategory()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();

            //Act
            mockLogger.LogToOperations(TestsConstants.TestGuidName, null);

            //Assert caught by expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogExceptionCategoryWithNullExceptionThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();

            //Act
            mockLogger.LogToOperations((Exception)null, TestsConstants.AreasCategories);

            //Assert caught by expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogExceptionCategoryWithNullCategoryThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            var ex = new Exception("test exception");

            //Act
            mockLogger.LogToOperations(ex, null);

            //Assert caught by expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogMessageEventIdSeverityWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;

            //Act
            mockLogger.LogToOperations((string)null, eventId, EventSeverity.Information);

            //Assert caught by expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogMessageEventIdSandboxSeverityWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;

            //Act
            mockLogger.LogToOperations((string)null, eventId, SandboxEventSeverity.Information);

            //Assert caught by expected exception.
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogMessageEventIdCategoryWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;

            //Act
            mockLogger.LogToOperations((string)null, eventId, TestsConstants.AreasCategories);

            //Assert caught by expected exception.
        }

        [TestMethod]
        public void LogToOperations_NullCategoryDoesNotThrowExceptionWIthEvenId()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;

            //Act
            mockLogger.LogToOperations(TestsConstants.TestGuidName, eventId, null);

            //Assert caught by expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogMessageEventIdSeverityCategoryWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;

            //Act
            mockLogger.LogToOperations((string)null, eventId, EventSeverity.Information, TestsConstants.AreasCategories);

            //Assert caught by expected exception.
        }

        [TestMethod]
        public void LogToOperations_DoesNotThrowArgumentNullExceptionWithNullCategory_WithEventIdAndSeverity()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;

            //Act
            mockLogger.LogToOperations(TestsConstants.TestGuidName, eventId, EventSeverity.Information, null);

            //Assert caught by expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogMessageEventIdSandboxSeverityCategoryWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;

            //Act
            mockLogger.LogToOperations((string)null, eventId, SandboxEventSeverity.Information, TestsConstants.AreasCategories);

            //Assert caught by expected exception.
        }

        [TestMethod]
        public void LogToOperations_DoesNotThrowArgumentNullExceptionWithNullCategory_WithEventIdSandboxSeverity()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;

            //Act
            mockLogger.LogToOperations(TestsConstants.TestGuidName, eventId, SandboxEventSeverity.Information, null);

            //Assert caught by expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogExceptionMessageEventIdWithNullExceptionThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;
            

            //Act
            mockLogger.LogToOperations((Exception) null, TestsConstants.TestGuidName, eventId);

            //Assert caught by expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogExceptionMessageEventIdWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;
            var ex = new Exception("test exception");

            //Act
            mockLogger.LogToOperations(ex, null, eventId);

            //Assert caught by expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogExceptionEventIdSeverityWithNullExceptionThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;


            //Act
            mockLogger.LogToOperations((Exception)null, eventId, EventSeverity.Information, TestsConstants.AreasCategories);

            //Assert caught by expected exception.
        }

        [TestMethod]
        public void LogToOperations_DoesNotThrowArgumentNullExceptionWithNullCategory_WithEventSeverityandEventId()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;
            var ex = new Exception("test exception");

            //Act
            mockLogger.LogToOperations((Exception)ex, eventId, EventSeverity.Information, null);

            //Assert caught by expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogExceptionEventIdSandboxSeverityWithNullExceptionThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;


            //Act
            mockLogger.LogToOperations((Exception)null, eventId, SandboxEventSeverity.Information, TestsConstants.AreasCategories);

            //Assert caught by expected exception.
        }

        [TestMethod]
        public void LogToOperations_DoesNotThrowArgumentNullExceptionWithNullCategory_WithSandboxEventSeverityExceptionAndEventId()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;
            var ex = new Exception("test exception");

            //Act
            mockLogger.LogToOperations((Exception)ex, eventId, SandboxEventSeverity.Information, null);

            //Assert caught by expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogExceptionMessageEventIdSeverityWithNullExceptionThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;

            //Act
            mockLogger.LogToOperations((Exception)null, TestsConstants.TestGuidName, eventId, EventSeverity.Information, TestsConstants.AreasCategories);

            //Assert caught by expected exception.
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogExceptionMessageEventIdSeverityWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;
            var ex = new Exception("test Exception");

            //Act
            mockLogger.LogToOperations(ex, null, eventId, EventSeverity.Information, TestsConstants.AreasCategories);

            //Assert caught by expected exception.
        }


        [TestMethod]
        public void LogToOperations_DoesNotThrowArgumentNullExceptionWithNullCategory_ExceptionMessageEventIdSeverity()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;
            var ex = new Exception("test exception");

            //Act
            mockLogger.LogToOperations((Exception)ex, TestsConstants.TestGuidName, eventId, EventSeverity.Information, null);

            //Assert caught by expected exception.
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogExceptionMessageEventIdSandboxSeverityWithNullExceptionThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;


            //Act
            mockLogger.LogToOperations((Exception)null, TestsConstants.TestGuidName, eventId, SandboxEventSeverity.Information, TestsConstants.AreasCategories);

            //Assert caught by expected exception.
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteLogExceptionMessageEventIdSandboxSeverityWithNullMessageThrowsArgumentNullException()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;
            var ex = new Exception("test Exception");

            //Act
            mockLogger.LogToOperations(ex, null, eventId, SandboxEventSeverity.Information, TestsConstants.AreasCategories);

            //Assert caught by expected exception.
        }

        [TestMethod]
        public void LogToOperations_DoesNotThrowArgumentNullExceptionWithNullCategory_WithExceptionMessageEventIdAndSandboxSeverity()
        {
            //Arrange
            var mockLogger = new TestableBaseLogger();
            int eventId = 99;
            var ex = new Exception("test exception");

            //Act
            mockLogger.LogToOperations((Exception)ex, TestsConstants.TestGuidName, eventId, SandboxEventSeverity.Information, null);

            //Assert caught by expected exception.
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
}