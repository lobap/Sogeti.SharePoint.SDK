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
using System.Security.Permissions;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;
using System.Collections.Generic;

namespace Microsoft.Practices.SharePoint.Common.Logging
{
    /// <summary>
    /// Interface for logging implementations
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Writes an error message into the log, works from sandbox.
        /// </summary>
        /// <param name="message">The message to write</param>
        void LogToOperations(string message);

        /// <summary>
        /// Writes information about an exception into the log to be read by operations, 
        /// works from sandbox.
        /// </summary>
        /// <param name="exception">The exception to write into the log. </param>
        void LogToOperations(Exception exception);

        /// <summary>
        /// Writes an error message into the log, do not use in sandbox.
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="severity">How serious the event is. </param>
        void LogToOperations(string message, EventSeverity severity);


        /// <summary>
        /// Writes an error message into the log from the sandbox.
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="severity">How serious the event is. </param>
        void LogToOperations(string message, SandboxEventSeverity severity);

        /// <summary>
        /// Writes an error message into the log with specified event Id, works from sandbox.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        void LogToOperations(string message, int eventId);

        /// <summary>
        /// Writes information about an exception into the log to be read by operations, , works from sandbox.
        /// </summary>
        /// <param name="exception">The exception to write into the log. </param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        void LogToOperations(Exception exception, string additionalMessage);

        /// <summary>
        /// Writes an error message into the log with specified category, works from sandbox.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="category">The category to write the message to.</param>
        void LogToOperations(string message, string category);

        /// <summary>
        /// Writes an error message into the log, don't use in sandbox.
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="severity">How serious the event is. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
         void LogToOperations(string message, int eventId, EventSeverity severity);

        /// <summary>
        /// Writes an error message into the log from the sandbox.
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="severity">How serious the event is. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        void LogToOperations(string message, int eventId, SandboxEventSeverity severity);

        /// <summary>
        /// Writes an error message into the log with specified event Id, works from sandbox.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        void LogToOperations(string message, int eventId, string category);

        /// <summary>
        /// Writes information about an exception into the log, works from sandbox.
        /// </summary>
        /// <param name="exception">The exception to write into the log to be read by operations. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        void LogToOperations(Exception exception, string additionalMessage, int eventId);

        /// <summary>
        /// Log a message with specified <paramref name="message"/>, <paramref name="eventId"/>, <paramref name="severity"/>
        /// and <paramref name="category"/>.  Don't use in sandbox.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="severity">How serious the event is. </param>
        /// <param name="category">The category of the log message.</param>
        void LogToOperations(string message, int eventId, EventSeverity severity, string category);

        /// <summary>
        /// Log a message from the sandbox with specified <paramref name="message"/>, <paramref name="eventId"/>, <paramref name="severity"/>
        /// and <paramref name="category"/>.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="severity">How serious the event is. </param>
        /// <param name="category">The category of the log message.</param>
        void LogToOperations(string message, int eventId, SandboxEventSeverity severity, string category);

        /// <summary>
        /// Writes information about an exception into the log to be read by operations. Don't use from sandbox.
        /// </summary>
        /// <param name="exception">The exception to write into the log. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        /// <param name="severity">The severity of the exception.</param>
        void LogToOperations(Exception exception, int eventId, EventSeverity severity, string category);

        /// <summary>
        /// Writes information about an exception into the log to be read by operations from the sandbox. 
        /// </summary>
        /// <param name="exception">The exception to write into the log. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        /// <param name="severity">The severity of the exception.</param>
        void LogToOperations(Exception exception, int eventId, SandboxEventSeverity severity, string category);

        /// <summary>
        /// Writes information about an exception into the log, don't use from sandbox.
        /// </summary>
        /// <param name="exception">The exception to write into the log to be read by operations. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        void LogToOperations(Exception exception, string additionalMessage, int eventId,
                                      EventSeverity severity, string category);

        /// <summary>
        /// Writes information about an exception into the log from the sandbox. 
        /// </summary>
        /// <param name="exception">The exception to write into the log to be read by operations. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        void LogToOperations(Exception exception, string additionalMessage, int eventId,
                                     SandboxEventSeverity severity, string category);

        /// <summary>
        /// Write a diagnostic message into the log, with the default category, default id,
        /// and default severity, works from sandbox.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        void TraceToDeveloper(string message);

        /// <summary>
        /// Writes information about an exception into the log. with the default category, default id,
        /// and default severity, works from sandbox.
        /// </summary>
        /// <param name="exception">The exception to write into the log. </param>
        void TraceToDeveloper(Exception exception);

        /// <summary>
        /// Writes information about an exception into the log, works from sandbox.
        /// </summary>
        /// <param name="exception">The exception to write into the log. </param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        void TraceToDeveloper(Exception exception, string additionalMessage);
        
        /// <summary>
        /// Write a diagnostic message into the log for the default category with severity, don't use
        /// from sandbox.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="severity">The severity of the exception.</param>
        void TraceToDeveloper(string message, TraceSeverity severity);

        /// <summary>
        /// Write a diagnostic message into the log for the default category with default severity
        /// from the sandbox, works from sandbox.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="severity"></param>
        void TraceToDeveloper(string message, SandboxTraceSeverity severity);

        /// <summary>
        /// Writes a diagnostic message into the trace log, with severity <see cref="TraceSeverity.Medium"/>, 
        /// works from sandbox.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        void TraceToDeveloper(string message, int eventId);

        /// <summary>
        /// Writes a diagnostic message into the trace log, with severity <see cref="TraceSeverity.Medium"/>,
        /// works from sandbox.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="category">The category to write the message to.</param>
        void TraceToDeveloper(string message, string category);

        /// <summary>
        /// Writes a diagnostic message into the log, works from sandbox. 
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event.
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        void TraceToDeveloper(string message, int eventId, string category);

        /// <summary>
        /// Writes information about an exception into the log, works from sandbox.
        /// </summary>
        /// <param name="exception">The exception to write into the log to be read by operations. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. 
        /// </param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        void TraceToDeveloper(Exception exception, string additionalMessage, int eventId);

        /// <summary>
        /// Writes a diagnostic message into the trace log, with specified <see cref="TraceSeverity"/>. Don't
        /// use in sandbox.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event.
        /// </param>
        /// <param name="severity">The severity of the trace.</param>
        /// <param name="category">The category to write the message to.</param>
        void TraceToDeveloper(string message, int eventId, TraceSeverity severity, string category);

        /// <summary>
        /// Writes a diagnostic message into the trace log, with specified <see cref="SandboxTraceSeverity"/>. 
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event.
        /// </param>
        /// <param name="severity">The severity of the trace.</param>
        /// <param name="category">The category to write the message to.</param>
        void TraceToDeveloper(string message, int eventId, SandboxTraceSeverity severity, string category);

        /// <summary>
        /// Writes information about an exception into the log, don't use from sandbox.
        /// </summary>
        /// <param name="exception">The exception to write into the log. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. 
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        /// <param name="severity">The severity of the exception.</param>
        void TraceToDeveloper(Exception exception, int eventId, TraceSeverity severity, string category);

        /// <summary>
        /// Writes information about an exception into the log from the sandbox.  This will work
        /// from outside the sandbox as well.
        /// </summary>
        /// <param name="exception">The exception to write into the log. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. 
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        /// <param name="severity">The severity of the exception.</param>
        void TraceToDeveloper(Exception exception, int eventId, SandboxTraceSeverity severity, string category);

        /// <summary>
        /// Writes information about an exception into the log. Don't use from sandbox.
        /// </summary>
        /// <param name="exception">The exception to write into the log. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. 
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        void TraceToDeveloper(Exception exception, string additionalMessage, int eventId,
                                       TraceSeverity severity, string category);


        /// <summary>
        /// Writes information about an exception into the log from the sandbox.
        /// Will work from outside the sandbox as well.
        /// </summary>
        /// <param name="exception">The exception to write into the log. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. 
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        void TraceToDeveloper(Exception exception, string additionalMessage, int eventId,
                                        SandboxTraceSeverity severity, string category);
    }
}