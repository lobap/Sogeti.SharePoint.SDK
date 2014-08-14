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
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Web;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SPG.Common.Logging
{
    /// <summary>
    /// Class that does the logging for a SharePoint environment.
    /// 
    /// This class will write operations messages to BOTH the EventLog and the trace log. Messages for the
    /// developer will only be written to the trace log. If something goes wrong while logging, an exception
    /// with both the original log message and the reason why logging failed is thrown. If tracing fails, it will
    /// attempt to write this to the EventLog, but will silently fail. 
    /// </summary>
    public class SharePointLogger : BaseLogger
    {
        private IEventLogLogger eventLogLogger;
        private ITraceLogger traceLogger;

        /// <summary>
        /// The logger that get's trace messages written to. 
        /// </summary>
        public ITraceLogger TraceLogger
        {
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                if (traceLogger == null)
                {
                    traceLogger = SharePointServiceLocator.Current.GetInstance<ITraceLogger>();
                }

                return traceLogger;
            }
        }

        /// <summary>
        /// The logger that get's error messages written to. 
        /// </summary>
        public IEventLogLogger EventLogLogger
        {
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                if (eventLogLogger == null)
                {
                    eventLogLogger = SharePointServiceLocator.Current.GetInstance<IEventLogLogger>();
                }

                return eventLogLogger;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "We have to ignore all exceptions here. It's a best attempt at logging.")]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private void AttemptToWriteTraceExceptionToEventLog(Exception exception)
        {
            try
            {
                string exceptionMessage = BuildExceptionMessage(exception,
                                                                "An error occurred while writing tot the Trace Log.");

                EventLogLogger.Log(exceptionMessage, 0, EventLogEntryType.Error, "Logging");
            }
            catch (Exception)
            {
                // We are only attempting to write the fact that the tracing failed to the EventLog. If that fails
                // we'll just bubble up the original exception
            }
        }

        private Exception BuildLoggingException(string originalMessage, Exception errorLogException,
                                                Exception traceLogException)
        {
            string errorMessage = "One or more error occurred while writing messages into the log.";
            Exception innerException = null;

            // Build a dictionary with exception data, that can later be added to the exception
            Dictionary<object, object> exceptionData = new Dictionary<object, object>();
            exceptionData["OriginalMessage"] = originalMessage;

            // If there was an exception with logging to the exception log, add that data here:
            if (errorLogException != null)
            {
                string exceptionMessage = BuildExceptionMessage(errorLogException, null);
                errorMessage += "\r\nThe error while writing to the EventLog was:" + exceptionMessage;
                exceptionData["ErrorLogException"] = exceptionMessage;

                // Preferably, use the exception from the error log as the inner exception
                innerException = errorLogException;
            }

            // If there was an exception while logging to the trace log, add that data here. 
            if (traceLogException != null)
            {
                string exceptionMessage = BuildExceptionMessage(traceLogException, null);
                errorMessage += "\r\nThe error while writing to the trace log was:" + exceptionMessage;

                exceptionData["TraceLogException"] = exceptionMessage;

                // If the exception log didn't throw an exception, then use the exception 
                if (innerException == null)
                    innerException = traceLogException;
            }

            // Now build the exception with the gathered information. 
            LoggingException loggingException = new LoggingException(errorMessage, innerException);
            foreach (object key in exceptionData.Keys)
            {
                loggingException.Data.Add(key, exceptionData[key]);
            }

            return loggingException;
        }

        /// <summary>
        /// Writes messages targeted to operations into the EventLog, to be read by operations. It will also attempt
        /// to write the message into the trace log. 
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system.</param>
        /// <param name="severity">How serious the event is.</param>
        /// <param name="category">The category of the log message.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Need to catch exception to implement robust logging"), SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected override void WriteToOperationsLog(string message, int eventId, EventLogEntryType severity,
                                                     string category)
        {
            Exception loggingException = null;
            Exception tracingException = null;
            try
            {
                EventLogLogger.Log(message, eventId, severity, category);
            }
            catch (Exception ex)
            {
                loggingException = ex;
            }

            try
            {
                TraceLogger.Trace(message, eventId, MapEventLogEntryTypesToTraceLogSeverity(severity), category);
            }
            catch (Exception ex)
            {
                tracingException = ex;
                AttemptToWriteTraceExceptionToEventLog(ex);
            }

            // If the logging failed, throw an error that holds both the original error information and the 
            // reason why logging failed. Dont do this if only the tracing failed. 
            if (loggingException != null)
            {
                throw BuildLoggingException(message, loggingException, tracingException);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private TraceSeverity MapEventLogEntryTypesToTraceLogSeverity(EventLogEntryType severity)
        {
            switch (severity)
            {
                case EventLogEntryType.Error:
                    return TraceSeverity.High;

                case EventLogEntryType.Warning:
                    return TraceSeverity.Medium;

                case EventLogEntryType.Information:
                case EventLogEntryType.FailureAudit:
                case EventLogEntryType.SuccessAudit:
                    return TraceSeverity.Verbose;

                default:
                    // assume worst case scenario if unknown.
                    return TraceSeverity.High;
            }
        }

        /// <summary>
        /// Writes a trace message to be read by developers into the trace log. 
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system.</param>
        /// <param name="severity">How serious the event is.</param>
        /// <param name="category">The category of the log message.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Need to catch exception to implement robust logging"), SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected override void WriteToDeveloperTrace(string message, int eventId, TraceSeverity severity,
                                                      string category)
        {
            try
            {
                TraceLogger.Trace(message, eventId, severity, category);
            }
            catch (Exception ex)
            {
                AttemptToWriteTraceExceptionToEventLog(ex);
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private string BuildContextualInformationMessage()
        {
            if (HttpContext.Current == null)
                return string.Empty;

            StringBuilder builder = new StringBuilder();
            builder.Append("\nAdditional Information:");
            builder.AppendFormat("\n\tRequest TimeStamp: '{0}'",
                                 HttpContext.Current.Timestamp.ToString("o", CultureInfo.CurrentCulture));

            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity != null)
            {
                builder.AppendFormat("\n\tUserName: '{0}'", HttpContext.Current.User.Identity.Name);
            }

            HttpRequest request = HttpContext.Current.Request;
            if (request != null)
            {
                builder.AppendFormat("\n\tRequest URL: '{0}'", request.Url);
                builder.AppendFormat("\n\tUser Agent: '{0}'", request.UserAgent);
                builder.AppendFormat("\n\tOriginating IP address: '{0}'", request.UserHostAddress);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Add contextual information to the EventLog message.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system.</param>
        /// <param name="severity">How serious the event is.</param>
        /// <param name="category">The category of the log message.</param>
        /// <returns>The message.</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected override string BuildEventLogMessage(string message, int eventId, EventLogEntryType severity,
                                                       string category)
        {
            return base.BuildEventLogMessage(message, eventId, severity, category) + BuildContextualInformationMessage();
        }
    }
}