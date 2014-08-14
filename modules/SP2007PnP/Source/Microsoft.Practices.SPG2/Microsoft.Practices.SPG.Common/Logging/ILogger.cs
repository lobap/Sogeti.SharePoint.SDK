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
using System.Security.Permissions;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SPG.Common.Logging
{
    /// <summary>
    /// Interface for logging implementations
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Writes an error message into the log
        /// </summary>
        /// <param name="message">The message to write</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void LogToOperations(string message);

        /// <summary>
        /// Writes an error message into the log
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="severity">How serious the event is. </param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void LogToOperations(string message, EventLogEntryType severity);

        /// <summary>
        /// Writes an error message into the log with specified event Id.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void LogToOperations(string message, int eventId);

        /// <summary>
        /// Writes an error message into the log
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="severity">How serious the event is. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void LogToOperations(string message, int eventId, EventLogEntryType severity);

        /// <summary>
        /// Writes an error message into the log with specified category.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="category">The category to write the message to.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void LogToOperations(string message, string category);

        /// <summary>
        /// Writes an error message into the log with specified event Id.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void LogToOperations(string message, int eventId, string category);

        /// <summary>
        /// Log a message with specified <paramref name="message"/>, <paramref name="eventId"/>, <paramref name="severity"/>
        /// and <paramref name="category"/>.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="severity">How serious the event is. </param>
        /// <param name="category">The category of the log message.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void LogToOperations(string message, int eventId, EventLogEntryType severity, string category);

        /// <summary>
        /// Write a diagnostic message into the log, with severity <see cref="TraceSeverity.Medium"/>
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void TraceToDeveloper(string message);

        /// <summary>
        /// Write a diagnostic message into the log, with severity <see cref="TraceSeverity.Medium"/>
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="severity">The severity of the exception.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void TraceToDeveloper(string message, TraceSeverity severity);

        /// <summary>
        /// Writes a diagnostic message into the trace log, with severity <see cref="TraceSeverity.Medium"/>. 
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void TraceToDeveloper(string message, int eventId);

        /// <summary>
        /// Writes a diagnostic message into the trace log, with severity <see cref="TraceSeverity.Medium"/>. 
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="category">The category to write the message to.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void TraceToDeveloper(string message, string category);

        /// <summary>
        /// Writes a diagnostic message into the trace log, with specified <see cref="TraceSeverity"/>. 
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event.
        /// </param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="category">The category to write the message to.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void TraceToDeveloper(string message, int eventId, TraceSeverity severity, string category);

        /// <summary>
        /// Writes a diagnostic message into the log. 
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event.
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void TraceToDeveloper(string message, int eventId, string category);

        /// <summary>
        /// Writes information about an exception into the log. 
        /// </summary>
        /// <param name="exception">The exception to write into the log to be read by operations. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void LogToOperations(Exception exception, string additionalMessage, int eventId,
                                      EventLogEntryType severity, string category);

        /// <summary>
        /// Writes information about an exception into the log. 
        /// </summary>
        /// <param name="exception">The exception to write into the log to be read by operations. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void LogToOperations(Exception exception, string additionalMessage, int eventId);

        /// <summary>
        /// Writes information about an exception into the log to be read by operations. 
        /// </summary>
        /// <param name="exception">The exception to write into the log. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        /// <param name="severity">The severity of the exception.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void LogToOperations(Exception exception, int eventId, EventLogEntryType severity, string category);

        /// <summary>
        /// Writes information about an exception into the log to be read by operations. 
        /// </summary>
        /// <param name="exception">The exception to write into the log. </param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void LogToOperations(Exception exception);

        /// <summary>
        /// Writes information about an exception into the log to be read by operations. 
        /// </summary>
        /// <param name="exception">The exception to write into the log. </param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void LogToOperations(Exception exception, string additionalMessage);


        /// <summary>
        /// Writes information about an exception into the log. 
        /// </summary>
        /// <param name="exception">The exception to write into the log. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. 
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        /// <param name="severity">The severity of the exception.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void TraceToDeveloper(Exception exception, int eventId, TraceSeverity severity, string category);

        /// <summary>
        /// Writes information about an exception into the log. 
        /// </summary>
        /// <param name="exception">The exception to write into the log. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. 
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void TraceToDeveloper(Exception exception, string additionalMessage, int eventId,
                                       TraceSeverity severity, string category);

        /// <summary>
        /// Writes information about an exception into the log. 
        /// </summary>
        /// <param name="exception">The exception to write into the log. </param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void TraceToDeveloper(Exception exception);

        /// <summary>
        /// Writes information about an exception into the log. 
        /// </summary>
        /// <param name="exception">The exception to write into the log. </param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void TraceToDeveloper(Exception exception, string additionalMessage);

        /// <summary>
        /// Writes information about an exception into the log. 
        /// </summary>
        /// <param name="exception">The exception to write into the log to be read by operations. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. 
        /// </param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void TraceToDeveloper(Exception exception, string additionalMessage, int eventId);
    }
}