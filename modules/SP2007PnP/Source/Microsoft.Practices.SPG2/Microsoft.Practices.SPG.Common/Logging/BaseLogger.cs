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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Web;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SPG.Common.Logging
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
        /// Method that derived classes must implement to do the logging. 
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
        public void LogToOperations(string message, int eventId, EventLogEntryType severity, string category)
        {
            WriteToOperationsLog(BuildEventLogMessage(message, eventId, severity, category)
                            , eventId, severity, category);
        }

        /// <summary>
        /// Writes a diagnostic message into the trace log, with severity <see cref="TraceSeverity.Medium"/>
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void TraceToDeveloper(string message)
        {
            TraceToDeveloper(message, DefaultEventId, TraceSeverity.Medium, null);
        }

        /// <summary>
        /// Write a diagnostic message into the trace log, with specified <see cref="TraceSeverity"/>
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="severity">The severity of the exception.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void TraceToDeveloper(string message, TraceSeverity severity)
        {
            TraceToDeveloper(message, DefaultEventId, severity, null);
        }

        /// <summary>
        /// Writes a diagnostic message into the trace log, with severity <see cref="TraceSeverity.Medium"/>.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void TraceToDeveloper(string message, int eventId)
        {
            TraceToDeveloper(message, eventId, TraceSeverity.Medium, null);
        }

        /// <summary>
        /// Writes a diagnostic message into the trace log, with severity <see cref="TraceSeverity.Medium"/>.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="category">The category to write the message to.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void TraceToDeveloper(string message, string category)
        {
            TraceToDeveloper(message, DefaultEventId, TraceSeverity.Medium, category);
        }

        /// <summary>
        /// Writes a diagnostic message into the trace log, with specified <see cref="TraceSeverity"/>.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the event.</param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="category">The category to write the message to.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void TraceToDeveloper(string message, int eventId, TraceSeverity severity, string category)
        {
            WriteToDeveloperTrace(BuildTraceMessage(message, eventId, severity, category), eventId, severity, category);
        }

        /// <summary>
        /// Writes a diagnostic message into the trace log, with severity <see cref="TraceSeverity.Medium"/>.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the event.</param>
        /// <param name="category">The category to write the message to.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void TraceToDeveloper(string message, int eventId, string category)
        {
            TraceToDeveloper(message, eventId, TraceSeverity.Medium, category);
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
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void LogToOperations(Exception exception, string additionalMessage, int eventId,
                                             EventLogEntryType severity, string category)
        {
            LogToOperations(BuildExceptionMessage(exception, additionalMessage), eventId, severity, category);
        }

        /// <summary>
        /// Writes information about an exception into the log.
        /// </summary>
        /// <param name="exception">The exception to write into the log to be read by operations.</param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void LogToOperations(Exception exception, string additionalMessage, int eventId)
        {
            LogToOperations(BuildExceptionMessage(exception, additionalMessage), eventId, EventLogEntryType.Error, null);
        }

        /// <summary>
        /// Writes information about an exception into the log.
        /// </summary>
        /// <param name="exception">The exception to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system.</param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="category">The category to write the message to.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void LogToOperations(Exception exception, int eventId, EventLogEntryType severity,
                                             string category)
        {
            LogToOperations(BuildExceptionMessage(exception, null), eventId, severity, category);
        }

        /// <summary>
        /// Writes information about an exception into the log to be read by operations.
        /// </summary>
        /// <param name="exception">The exception to write into the log.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void LogToOperations(Exception exception)
        {
            LogToOperations(BuildExceptionMessage(exception, null), DefaultEventId, EventLogEntryType.Error, null);
        }

        /// <summary>
        /// Writes information about an exception into the log to be read by operations.
        /// </summary>
        /// <param name="exception">The exception to write into the log.</param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void LogToOperations(Exception exception, string additionalMessage)
        {
            LogToOperations(BuildExceptionMessage(exception, additionalMessage), DefaultEventId, EventLogEntryType.Error,
                            null);
        }

        /// <summary>
        /// Writes information about an exception into the log.
        /// </summary>
        /// <param name="exception">The exception to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the event.</param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="category">The category to write the message to.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void TraceToDeveloper(Exception exception, int eventId, TraceSeverity severity, string category)
        {
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
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void TraceToDeveloper(Exception exception, string additionalMessage, int eventId,
                                              TraceSeverity severity, string category)
        {
            TraceToDeveloper(BuildExceptionMessage(exception, additionalMessage), eventId, severity, category);
        }

        /// <summary>
        /// Writes information about an exception into the log.
        /// </summary>
        /// <param name="exception">The exception to write into the log.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void TraceToDeveloper(Exception exception)
        {
            TraceToDeveloper(BuildExceptionMessage(exception, null), DefaultEventId, TraceSeverity.Medium, null);
        }

        /// <summary>
        /// Writes information about an exception into the log.
        /// </summary>
        /// <param name="exception">The exception to write into the log.</param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void TraceToDeveloper(Exception exception, string additionalMessage)
        {
            TraceToDeveloper(BuildExceptionMessage(exception, additionalMessage), DefaultEventId, TraceSeverity.Medium,
                             null);
        }

        /// <summary>
        /// Writes information about an exception into the log.
        /// </summary>
        /// <param name="exception">The exception to write into the log to be read by operations.</param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        /// <param name="eventId">The eventId that corresponds to the event.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void TraceToDeveloper(Exception exception, string additionalMessage, int eventId)
        {
            TraceToDeveloper(BuildExceptionMessage(exception, additionalMessage), eventId, TraceSeverity.Medium,
                 null);
        }
        
        /// <summary>
        /// Writes an error message into the log
        /// </summary>
        /// <param name="message">The message to write</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void LogToOperations(string message)
        {
            LogToOperations(message, DefaultEventId, EventLogEntryType.Error, null);
        }

        /// <summary>
        /// Writes an error message into the log
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="severity">How serious the event is.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void LogToOperations(string message, EventLogEntryType severity)
        {
            LogToOperations(message, DefaultEventId, severity, null);
        }

        /// <summary>
        /// Writes an error message into the log with specified event Id.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void LogToOperations(string message, int eventId)
        {
            LogToOperations(message, eventId, EventLogEntryType.Error, null);
        }

        /// <summary>
        /// Writes an error message into the log
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system.</param>
        /// <param name="severity">How serious the event is.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void LogToOperations(string message, int eventId, EventLogEntryType severity)
        {
            LogToOperations(message, eventId, severity, null);
        }

        /// <summary>
        /// Writes an error message into the log with specified category.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="category">The category to write the message to.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void LogToOperations(string message, string category)
        {
            LogToOperations(message, DefaultEventId, EventLogEntryType.Error, category);
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
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void LogToOperations(string message, int eventId, string category)
        {
            LogToOperations(message, eventId, EventLogEntryType.Error, category);
        }

        #endregion

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
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected abstract void WriteToOperationsLog(string message, int eventId, EventLogEntryType severity, string category);

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
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected virtual string BuildTraceMessage(string message, int eventId, TraceSeverity severity, string category)
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
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected virtual string BuildEventLogMessage(string message, int eventId, EventLogEntryType severity,
                                                   string category)
        {
            return message;
        }

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
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected abstract void WriteToDeveloperTrace(string message, int eventId, TraceSeverity severity, string category);

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
        [SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
            MessageId = "level+1", Justification = "Not an issue.")]
        protected virtual void WriteExceptionDetails(StringBuilder messageBuilder, Exception exception, int level)
        {
            int nextLevel = level + 1;

            messageBuilder.AppendFormat("{0}ExceptionType: '{1}'\r\n", Indent(level), exception.GetType().Name);
            messageBuilder.AppendFormat("{0}ExceptionMessage: '{1}'\r\n", Indent(level),
                                        EnsureIndentation(exception.Message, level));
            messageBuilder.AppendFormat("{0}StackTrace: '{1}'\r\n", Indent(level),
                                        EnsureIndentation(exception.StackTrace, level));
            messageBuilder.AppendFormat("{0}Source: '{1}'\r\n", Indent(level),
                                        EnsureIndentation(exception.Source, level));
            messageBuilder.AppendFormat("{0}TargetSite: '{1}'\r\n", Indent(level),
                                        EnsureIndentation(exception.TargetSite, level));


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