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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Web;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;
using System.Collections.Generic;

namespace Microsoft.Practices.SharePoint.Common.Logging
{
    /// <summary>
    /// Base class that makes it easier to implement a logger. 
    /// </summary>
    public abstract class BaseLogger : ILogger
    {
        /// <summary>
        /// The default event Id. Normally, you wouldn't want to use this, but provide an event Id for each error. 
        /// </summary>
        protected virtual int DefaultEventId
        {
            get { return 0; }
        }

        #region ILogger Members

        /// <summary>
        /// Writes a diagnostic message into the trace log, with default severity for the category.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
 
        public void TraceToDeveloper(string message)
        {
            Validation.ArgumentNotNull(message, "message");

            TraceToDeveloperDefaultSeverity(message, DefaultEventId, null);
        }


        /// <summary>
        /// Writes information about an exception into the log.
        /// </summary>
        /// <param name="exception">The exception to write into the log.</param>
        public void TraceToDeveloper(Exception exception)
        {
            Validation.ArgumentNotNull(exception, "exception");

            TraceToDeveloperDefaultSeverity(BuildExceptionMessage(exception, null), DefaultEventId, null);
        }


        /// <summary>
        /// Writes information about an exception into the log.
        /// </summary>
        /// <param name="exception">The exception to write into the log.</param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        public void TraceToDeveloper(Exception exception, string additionalMessage)
        {
            Validation.ArgumentNotNull(exception, "exception");
            Validation.ArgumentNotNull(additionalMessage, "additionalMessage");

            TraceToDeveloperDefaultSeverity(BuildExceptionMessage(exception, additionalMessage), DefaultEventId, 
                             null);
        }


        /// <summary>
        /// Write a diagnostic message into the trace log, with default severity for the category.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="severity">The severity of the exception.</param>
        public void TraceToDeveloper(string message, TraceSeverity severity)
        {
            Validation.ArgumentNotNull(message, "message");

            WriteTraceMessage(message, DefaultEventId, severity, null);
        }


        /// <summary>
        /// Write a diagnostic message into the trace log, with default severity for the category.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="severity">The severity of the exception.</param>
        public void TraceToDeveloper(string message, SandboxTraceSeverity severity)
        {
            Validation.ArgumentNotNull(message, "message");
            WriteTraceMessage(message, DefaultEventId, severity, null);
        }


        /// <summary>
        /// Writes a diagnostic message into the trace log, with  default severity for the category.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system.</param>
        public void TraceToDeveloper(string message, int eventId)
        {
            Validation.ArgumentNotNull(message, "message");

            TraceToDeveloperDefaultSeverity(message, eventId, null);
        }


        /// <summary>
        /// Writes a diagnostic message into the trace log, with default severity for the category.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="category">The category to write the message to.</param>
        public void TraceToDeveloper(string message, string category)
        {
            Validation.ArgumentNotNull(message, "message");

            TraceToDeveloperDefaultSeverity(message, DefaultEventId, category);
        }

        /// <summary>
        /// Writes a diagnostic message into the trace log, with the default severity for the category.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the event.</param>
        /// <param name="category">The category to write the message to.</param>
        public void TraceToDeveloper(string message, int eventId, string category)
        {
            Validation.ArgumentNotNull(message, "message");

            TraceToDeveloperDefaultSeverity(message, eventId, category);
        }

        /// <summary>
        /// Writes a diagnostic trace for the exception to the trace log with the default severity.
        /// </summary>
        /// <param name="exception">The exception to log</param>
        /// <param name="additionalMessage">Additional information to log</param>
        /// <param name="eventId">The event id to log</param>
        public void TraceToDeveloper(Exception exception, string additionalMessage, int eventId)
        {
            Validation.ArgumentNotNull(exception, "exception");
            Validation.ArgumentNotNull(additionalMessage, "additionalMessage");

             TraceToDeveloperDefaultSeverity(BuildExceptionMessage(exception, additionalMessage), eventId,
                             null);
        }

        /// <summary>
        /// Writes a diagnostic message into the trace log, with  specified <see cref="TraceSeverity"/>.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the event.</param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="category">The category to write the message to.</param>
        public void TraceToDeveloper(string message, int eventId, TraceSeverity severity, string category)
        {
            Validation.ArgumentNotNull(message, "message");

            WriteTraceMessage(message, eventId, severity, category);
        }

        /// <summary>
        /// Writes a diagnostic message into the trace log, with specified <see cref="TraceSeverity"/>.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the event.</param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="category">The category to write the message to.</param>
        public void TraceToDeveloper(string message, int eventId, SandboxTraceSeverity severity, string category)
        {
            Validation.ArgumentNotNull(message, "message");


            WriteTraceMessage(message, eventId, severity, category);
        }

        /// <summary>
        /// Writes information about an exception into the log.
        /// </summary>
        /// <param name="exception">The exception to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the event.</param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="category">The category to write the message to.</param>
        public void TraceToDeveloper(Exception exception, int eventId, TraceSeverity severity, string category)
        {
            Validation.ArgumentNotNull(exception, "exception");

            TraceToDeveloper(BuildExceptionMessage(exception, null), eventId, severity, category);
        }

        /// <summary>
        /// Writes information about an exception into the log from the sandbox.
        /// </summary>
        /// <param name="exception">The exception to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the event.</param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="category">The category to write the message to.</param>
        public void TraceToDeveloper(Exception exception, int eventId, SandboxTraceSeverity severity, string category)
        {
            Validation.ArgumentNotNull(exception, "exception");

            TraceToDeveloper(BuildExceptionMessage(exception, null), eventId, severity, category);
        }


        /// <summary>
        /// Writes information about an exception into the log.
        /// </summary>
        /// <param name="exception">The exception to write into the log.</param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        /// <param name="eventId">The eventId that corresponds to the event.</param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="category">The category to write the message to.</param>
        public void TraceToDeveloper(Exception exception, string additionalMessage, int eventId,
                                              TraceSeverity severity, string category)
        {
            Validation.ArgumentNotNull(exception, "exception");
            Validation.ArgumentNotNull(additionalMessage, "additionalMessage"); 

            TraceToDeveloper(BuildExceptionMessage(exception, additionalMessage), eventId, severity, category);
        }

        /// <summary>
        /// Writes information about an exception into the log from the sandbox.
        /// </summary>
        /// <param name="exception">The exception to write into the log.</param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        /// <param name="eventId">The eventId that corresponds to the event.</param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="category">The category to write the message to.</param>
        public void TraceToDeveloper(Exception exception, string additionalMessage, int eventId,
                                              SandboxTraceSeverity severity, string category)
        {
            Validation.ArgumentNotNull(exception, "exception");
            Validation.ArgumentNotNull(additionalMessage, "additionalMessage");

            TraceToDeveloper(BuildExceptionMessage(exception, additionalMessage), eventId, severity, category);
        }

        /// <summary>
        /// Writes an error message into the log
        /// </summary>
        /// <param name="message">The message to write</param>
        public void LogToOperations(string message)
        {
            Validation.ArgumentNotNull(message, "message");

            WriteLogMessageDefaultSeverity(message, DefaultEventId, null);
        }


        /// <summary>
        /// Writes information about an exception into the log to be read by operations.
        /// </summary>
        /// <param name="exception">The exception to write into the log.</param>
        public void LogToOperations(Exception exception)
        {
            Validation.ArgumentNotNull(exception, "exception");

            WriteLogMessageDefaultSeverity(BuildExceptionMessage(exception, null), DefaultEventId, null);
        }

        /// <summary>
        /// Writes an error message into the log
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="severity">How serious the event is.</param>
        public void LogToOperations(string message, EventSeverity severity)
        {
            Validation.ArgumentNotNull(message, "message");

            WriteLogMessage(message, DefaultEventId, severity, null);
        }

        /// <summary>
        /// Writes an error message into the log from the sandbox.  WIll
        /// work from outside the sandbox as well.
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="severity">How serious the event is.</param>
        public void LogToOperations(string message, SandboxEventSeverity severity)
        {
            Validation.ArgumentNotNull(message, "message");

            WriteLogMessage(message, DefaultEventId, severity, null);
        }


        /// <summary>
        /// Writes an error message into the log with specified event Id.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system.</param>
        public void LogToOperations(string message, int eventId)
        {
            Validation.ArgumentNotNull(message, "message");

            WriteLogMessageDefaultSeverity(message, eventId, null);
        }

        /// <summary>
        /// Writes an error message into the log with specified category.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="category">The category to write the message to.</param>
        public void LogToOperations(string message, string category)
        {
            Validation.ArgumentNotNull(message, "message");

            WriteLogMessageDefaultSeverity(message, DefaultEventId, category);
        }


        /// <summary>
        /// Writes information about an exception into the log to be read by operations.
        /// </summary>
        /// <param name="exception">The exception to write into the log.</param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        public void LogToOperations(Exception exception, string additionalMessage)
        {
            Validation.ArgumentNotNull(exception, "exception");
            Validation.ArgumentNotNull(additionalMessage, "additionalMessage");

            WriteLogMessageDefaultSeverity(BuildExceptionMessage(exception, additionalMessage), DefaultEventId, null);
        }


        /// <summary>
        /// Writes an error message into the log
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system.</param>
        /// <param name="severity">How serious the event is.</param>
        public void LogToOperations(string message, int eventId, EventSeverity severity)
        {
            Validation.ArgumentNotNull(message, "message");

            WriteLogMessage(message, eventId, severity, null);
        }

        /// <summary>
        /// Writes an error message into the log in the sandbox
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system.</param>
        /// <param name="severity">How serious the event is.</param>
        public void LogToOperations(string message, int eventId, SandboxEventSeverity severity)
        {
            Validation.ArgumentNotNull(message, "message");

            WriteLogMessage(message, eventId, severity, null);
        }

        /// <summary>
        /// Writes a error message into the log with specified event Id.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The event Id that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        public void LogToOperations(string message, int eventId, string category)
        {
            Validation.ArgumentNotNull(message, "message");

            WriteLogMessageDefaultSeverity(message, eventId, category);
        }

 

        /// <summary>
        /// Writes the logged message to the operations log 
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="severity">How serious the event is. </param>
        /// <param name="category">The category of the log message.</param>
        public void LogToOperations(string message, int eventId, EventSeverity severity, string category)
        {
            Validation.ArgumentNotNull(message, "message");

            WriteLogMessage(message, eventId, severity, category);
        }

        /// <summary>
        /// Writes the logged message to the operations log from the sandbox.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="severity">How serious the event is. </param>
        /// <param name="category">The category of the log message.</param>
        public void LogToOperations(string message, int eventId, SandboxEventSeverity severity, string category)
        {
            Validation.ArgumentNotNull(message, "message");

            WriteLogMessage(message, eventId, severity, category);
        }


        /// <summary>
        /// Writes information about an exception into the log.
        /// </summary>
        /// <param name="exception">The exception to write into the log to be read by operations.</param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system.</param>
        public void LogToOperations(Exception exception, string additionalMessage, int eventId)
        {
            Validation.ArgumentNotNull(exception, "exception");
            Validation.ArgumentNotNull(additionalMessage, "additionalMessage"); 
            
            WriteLogMessageDefaultSeverity(BuildExceptionMessage(exception, additionalMessage), eventId, null);
        }

        /// <summary>
        /// Writes information about an exception into the log.
        /// </summary>
        /// <param name="exception">The exception to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system.</param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="category">The category to write the message to.</param>
        public void LogToOperations(Exception exception, int eventId, EventSeverity severity,
                                             string category)
        {
            Validation.ArgumentNotNull(exception, "exception");

            WriteLogMessage(BuildExceptionMessage(exception, null), eventId, severity, category);
        }

        /// <summary>
        /// Writes information about an exception into the log to the sandbox.
        /// </summary>
        /// <param name="exception">The exception to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system.</param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="category">The category to write the message to.</param>
        public void LogToOperations(Exception exception, int eventId, SandboxEventSeverity severity,
                                             string category)
        {
            Validation.ArgumentNotNull(exception, "exception");

            WriteLogMessage(BuildExceptionMessage(exception, null), eventId, severity, category);
        }

        /// <summary>
        /// Writes information about an exception into the log.
        /// </summary>
        /// <param name="exception">The exception to write into the log to be read by operations.</param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system.</param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="category">The category to write the message to.</param>
        public void LogToOperations(Exception exception, string additionalMessage, int eventId,
                                             EventSeverity severity, string category)
        {
            Validation.ArgumentNotNull(exception, "exception");
            Validation.ArgumentNotNull(additionalMessage, "additionalMessage"); 

            WriteLogMessage(BuildExceptionMessage(exception, additionalMessage), eventId, severity, category);
        }


        /// <summary>
        /// Writes information about an exception into the log from the sandbox.
        /// </summary>
        /// <param name="exception">The exception to write into the log to be read by operations.</param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system.</param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="category">The category to write the message to.</param>
        public void LogToOperations(Exception exception, string additionalMessage, int eventId,
                                             SandboxEventSeverity severity, string category)
        {
            Validation.ArgumentNotNull(exception, "exception");
            Validation.ArgumentNotNull(additionalMessage, "additionalMessage");

            WriteLogMessage(BuildExceptionMessage(exception, additionalMessage), eventId, severity, category);
        }

        #endregion

        /// <summary>
        /// Method that derived classes must implement to do the logging. 
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
         /// <param name="category">The category of the log message.</param>
        private void WriteLogMessageDefaultSeverity(string message, int eventId, string category)
        {
            WriteToOperationsLog(BuildEventLogMessage(message, eventId, category), eventId, category);
        }

        /// <summary>
        /// Writes information about an exception into the log.
        /// </summary>
        /// <param name="message">The message to write into the log to be read by operations.</param>
        /// <param name="eventId">The eventId that corresponds to the event.</param>
        /// <param name="category">The category for the trace.</param>
        private void TraceToDeveloperDefaultSeverity(string message, int eventId, string category)
        {
            string traceMsg = BuildTraceMessage(message, eventId, category);
            WriteToDeveloperTrace(traceMsg, eventId, category);
        }

        /// <summary>
        /// Safe method to trace to the sandbox.
        /// </summary>
        /// <param name="message">The message to trace</param>
        /// <param name="eventId">the event id to trace</param>
        /// <param name="severity">the sandbox severity to use</param>
        /// <param name="category">The category to trace under</param>
        private void WriteTraceMessage(string message, int eventId, TraceSeverity severity, string category)
        {
            string traceMsg = BuildTraceMessage(message, eventId, severity, category);
            WriteToDeveloperTrace(traceMsg, eventId, severity, category);
        }

        /// <summary>
        /// Safe method to trace to the sandbox.
        /// </summary>
        /// <param name="message">The message to trace</param>
        /// <param name="eventId">the event id to trace</param>
        /// <param name="severity">the sandbox severity to use</param>
        /// <param name="category">The category to trace under</param>
        private void WriteTraceMessage(string message, int eventId, SandboxTraceSeverity severity, string category)
        {
            string traceMsg = BuildSandboxTraceMessage(message, eventId, severity, category);
            WriteToDeveloperTrace(traceMsg, eventId, severity, category);
        }

        /// <summary>
        /// Safe method to trace to the sandbox.
        /// </summary>
        /// <param name="message">The message to trace</param>
        /// <param name="eventId">the event id to trace</param>
        /// <param name="severity">the sandbox severity to use</param>
        /// <param name="category">The category to trace under</param>
        private void WriteLogMessage(string message, int eventId, EventSeverity severity, string category)
        {
            string logMsg = BuildEventLogMessage(message, eventId, severity, category);
            WriteToOperationsLog(logMsg, eventId, severity, category);
        }

        /// <summary>
        /// Safe method to trace to the sandbox.
        /// </summary>
        /// <param name="message">The message to trace</param>
        /// <param name="eventId">the event id to trace</param>
        /// <param name="severity">the sandbox severity to use</param>
        /// <param name="category">The category to trace under</param>
        private void WriteLogMessage(string message, int eventId, SandboxEventSeverity severity, string category)
        {
            string logMsg = BuildEventLogMessage(message, eventId, severity, category);
            WriteToOperationsLog(logMsg, eventId, severity, category);
        }

        /// <summary>
        /// Override this method to change the way the trace message is created. 
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="severity">How serious the event is. </param>
        /// <param name="category">The category of the log message.</param>
        /// <returns>The message.</returns>
        protected virtual string BuildTraceMessage(string message, int eventId, TraceSeverity severity, string category)
        {
            return message;
        }

        /// <summary>
        /// Override this method to change the way the trace message is created. 
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="category">The category of the log message.</param>
        /// <returns>The message.</returns>
        protected virtual string BuildTraceMessage(string message, int eventId, string category)
        {
            return message;
        }

        /// <summary>
        /// Override this method to change the way the trace message is created. 
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="severity">How serious the event is. </param>
        /// <param name="category">The category of the log message.</param>
        /// <returns>The message.</returns>
        protected virtual string BuildSandboxTraceMessage(string message, int eventId, SandboxTraceSeverity severity, string category)
        {
            return message;
        }

        /// <summary>
        /// Override this method to change the way the log message is created. 
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The event Id that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="severity">How serious the event is. </param>
        /// <param name="category">The category of the log message.</param>
        /// <returns>The message.</returns>
         protected virtual string BuildEventLogMessage(string message, int eventId, EventSeverity severity,
                                                   string category)
        {
            return message;
        }

         /// <summary>
         /// Override this method to change the way the log message is created for the sandbox 
         /// </summary>
         /// <param name="message">The message to write into the log.</param>
         /// <param name="eventId">
         /// The event Id that corresponds to the event. This value, coupled with the EventSource is often used by
         /// administrators and IT PRo's to monitor the EventLog of a system. 
         /// </param>
         /// <param name="severity">How serious the event is. </param>
         /// <param name="category">The category of the log message.</param>
         /// <returns>The message.</returns>
         protected virtual string BuildEventLogMessage(string message, int eventId, SandboxEventSeverity severity,
                                                   string category)
         {
             return message;
         }

        /// <summary>
        /// Builds an event log message without a severity specified.
        /// </summary>
         /// <param name="message">The message to write into the log.</param>
         /// <param name="eventId">
         /// The event Id that corresponds to the event. This value, coupled with the EventSource is often used by
         /// administrators and IT PRo's to monitor the EventLog of a system. 
         /// </param>
         /// <param name="category">The category of the log message.</param>
         /// <returns>The message.</returns>
         protected virtual string BuildEventLogMessage(string message, int eventId,
                                           string category)
         {
             return message;
         }

        /// <summary>
        /// Override this method to implement how to write to a log message.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="severity">How serious the event is. </param>
        /// <param name="category">The category of the log message.</param>
        protected abstract void WriteToOperationsLog(string message, int eventId, EventSeverity severity, string category);

        /// <summary>
        /// Override this method to implement how to write to a log message.  This writes using the default severity
        /// for the underlying logging system.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="category">The category of the log message.</param>
        protected abstract void WriteToOperationsLog(string message, int eventId, string category);

        /// <summary>
        /// Override this method to implement how to write to a log message for the sandbox
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="severity">How serious the event is. </param>
        /// <param name="category">The category of the log message.</param>
        protected abstract void WriteToOperationsLog(string message, int eventId, SandboxEventSeverity severity, string category);


        /// <summary>
        /// Override this method to implement how to write to a trace message.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="severity">How serious the event is. </param>
        /// <param name="category">The category of the log message.</param>
        protected abstract void WriteToDeveloperTrace(string message, int eventId, TraceSeverity severity, string category);

        /// <summary>
        /// Override this method to implement how to write to a trace message. This writes using the default severity
        /// for the underlying trace system.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="category">The category of the log message.</param>
        protected abstract void WriteToDeveloperTrace(string message, int eventId, string category);

        /// <summary>
        /// Writes a sandbox safe developer trace message.
        /// </summary>
        /// <param name="message">The message to trace</param>
        /// <param name="eventId">The event id of the message to trace</param>
        /// <param name="severity">The severity of the message to trace</param>
        /// <param name="category">The category of the message being traced</param>
        protected abstract void WriteToDeveloperTrace(string message, int eventId, SandboxTraceSeverity severity, string category);

        /// <summary>
        /// Build an exception message for an exception that contains more information than the
        /// normal Exception.ToString().
        /// </summary>
        /// <param name="exception">The exception to format.</param>
        /// <param name="customErrorMessage">Any custom error information to include.</param>
        /// <returns>The error message.</returns>
        protected virtual string BuildExceptionMessage(Exception exception, string customErrorMessage)
        {
            StringBuilder messageBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(customErrorMessage))
            {
                messageBuilder.AppendLine(customErrorMessage);
            }
            else
            {
                messageBuilder.AppendLine("An exception has occurred.");
            }

            WriteExceptionDetails(messageBuilder, exception, 1);


            return messageBuilder.ToString();
        }

        /// <summary>
        /// Write the details of an exception to the string builder. This method is called recursively to 
        /// format all the inner exceptions. 
        /// </summary>
        /// <param name="messageBuilder">The stringbuilder that will hold the full exception message.</param>
        /// <param name="exception">The exception (and all it's inner exceptions) to add.</param>
        /// <param name="level">How far should the exceptions be indented.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "level+1", Justification = "Not an issue.")]
        protected virtual void WriteExceptionDetails(StringBuilder messageBuilder, Exception exception, int level)
        {
            Validation.ArgumentNotNull(messageBuilder, "messageBuilder");
            Validation.ArgumentNotNull(exception, "exception");

            int nextLevel = level + 1;

            messageBuilder.AppendFormat("{0}ExceptionType: '{1}'\r\n", Indent(level), exception.GetType().Name);
            messageBuilder.AppendFormat("{0}ExceptionMessage: '{1}'\r\n", Indent(level),
                                        EnsureIndentation(exception.Message, level));
            messageBuilder.AppendFormat("{0}StackTrace: '{1}'\r\n", Indent(level),
                                        EnsureIndentation(exception.StackTrace, level));
            messageBuilder.AppendFormat("{0}Source: '{1}'\r\n", Indent(level),
                                        EnsureIndentation(exception.Source, level));

            try
            {
                messageBuilder.AppendFormat("{0}TargetSite: '{1}'\r\n", Indent(level),
                                                EnsureIndentation(exception.TargetSite, level));
            }
            catch (TypeLoadException)
            {
                // in some cases exceptions may originate from a proxy in full trust with types
                // that are not allowed in the sandbox.  In this case a TypeLoadException will be
                // thrown.  We do not want to mask the original error if this gets thrown.
            }



            if (exception.Data != null && exception.Data.Count > 0)
            {
                messageBuilder.AppendLine(Indent(level) + "Additional Data:");
                foreach (string key in exception.Data.Keys)
                {
                    WriteAdditionalExceptionData(level, messageBuilder, exception, key, nextLevel);
                }
            }

            if (exception.InnerException != null)
            {
                messageBuilder.AppendLine(Indent(level) + "------------------------------------------------------------");
                messageBuilder.AppendLine(Indent(level) + "Inner exception:");
                messageBuilder.AppendLine(Indent(level) + "------------------------------------------------------------");
                WriteExceptionDetails(messageBuilder, exception.InnerException, nextLevel);
            }
        }

        private void WriteAdditionalExceptionData(int level, StringBuilder messageBuilder, Exception exception,
                                                  string key, int nextLevel)
        {
            object value = exception.Data[key];
            if (value != null)
            {
                Exception valueAsException = value as Exception;
                if (valueAsException != null)
                {
                    messageBuilder.AppendFormat("{0}{1} is an exception. Exception Details:\r\n", Indent(nextLevel), key);
                    WriteExceptionDetails(messageBuilder, valueAsException, nextLevel + 1);
                }
                else
                {
                    messageBuilder.AppendFormat("{0}'{1}' : '{2}'\r\n", Indent(nextLevel), key,
                                                EnsureIndentation(value, level));
                }
            }
        }

        private string EnsureIndentation(object obj, int indentationLevel)
        {
            if (obj == null)
                return string.Empty;

            return obj.ToString().Replace("\n", "\n" + Indent(indentationLevel + 1));
        }

        private string Indent(int indentationLevel)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < indentationLevel; i++)
            {
                builder.Append("\t");
            }
            return builder.ToString();
        }

    }
}