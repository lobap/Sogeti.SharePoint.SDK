//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Web;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.SharePoint.Utilities;
using Microsoft.Practices.SharePoint.Common.ProxyArgs;
using Microsoft.SharePoint;

namespace Microsoft.Practices.SharePoint.Common.Logging
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
        private IConfigManager configMgr;
        private static int canAccessSandboxLogging = -1;
        private static int canAccessSandboxTracing = -1;

        /// <summary>
        /// The logger that get's trace messages written to. 
        /// </summary>
    
        public ITraceLogger TraceLogger
        {
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                if (this.traceLogger == null)
                {
                    this.traceLogger = SharePointServiceLocator.GetCurrent().GetInstance<ITraceLogger>();
                }

                return this.traceLogger;
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
                if (this.eventLogLogger == null)
                {
                    this.eventLogLogger = SharePointServiceLocator.GetCurrent().GetInstance<IEventLogLogger>();
                }


                return this.eventLogLogger;
            }
        }

        
        private IConfigManager ConfigurationManager
        {
            get
            {
                if (this.configMgr == null)
                    this.configMgr = SharePointServiceLocator.GetCurrent().GetInstance<IConfigManager>();
                return this.configMgr;
            }
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
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Need to catch exception to implement robust logging")]
        protected override void WriteToOperationsLog(string message, int eventId, EventSeverity severity, string category)
        {
            try
            {
                EventLogLogger.Log(message, eventId, severity, category);
            }
            catch (Exception ex)
            {
                // If the logging failed, throw an error that holds both the original error information and the 
                // reason why logging failed. Dont do this if only the tracing failed. 
                throw BuildLoggingException(message, ex); 
            }
        }

        /// <summary>
        /// Writes a log message using the default severity for the category.
        /// </summary>
        /// <param name="message">The message to write to the event log</param>
        /// <param name="eventId">The id of the event</param>
        /// <param name="category">The category for the event in the form 'area/category'</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected override void WriteToOperationsLog(string message, int eventId, string category)
        {
            try
            {
                if (SharePointEnvironment.InSandbox)
                {
                    WriteToOperationsLogSandbox(message, eventId, category);
                }
                else
                {
                    EventLogLogger.Log(message, eventId, category);
                }
            }
            catch (Exception ex)
            {
                // If the logging failed, throw an error that holds both the original error information and the 
                // reason why logging failed. Dont do this if only the tracing failed. 
                throw BuildLoggingException(message, ex);
            }
        }

        /// <summary>
        /// Writes the message to the operations log using a sadnbox sevlerity
        /// </summary>
        /// <param name="message"></param>
        /// <param name="eventId"></param>
        /// <param name="severity"></param>
        /// <param name="category"></param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected override void WriteToOperationsLog(string message, int eventId, SandboxEventSeverity severity, string category)
        {
            try
            {
                if (SharePointEnvironment.InSandbox)
                    WriteToOperationsLogSandbox(message, eventId, severity, category);
                else
                    WriteToOperationsLogFullTrust(message, eventId, severity, category);
            }
            catch (Exception ex)
            {
                // If the logging failed, throw an error that holds both the original error information and the 
                // reason why logging failed. Dont do this if only the tracing failed. 
                throw BuildLoggingException(message, ex);
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
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Need to catch exception to implement robust logging")]
        protected override void WriteToDeveloperTrace(string message, int eventId, TraceSeverity severity,
                                                      string category)
        {
            try
            {
                TraceLogger.Trace(message, eventId, severity, category);
            }
            catch (Exception ex)
            {
                string logMessage = string.Format(CultureInfo.CurrentCulture, Properties.Resources.ErrorWritingTrace, message);
                throw BuildLoggingException(logMessage, ex);
            }
        }

        /// <summary>
        /// Writes the trace message using the default severity for the category
        /// </summary>
        /// <param name="message">The message to trace</param>
        /// <param name="eventId">The id of the trace</param>
        /// <param name="category">The category of the trace</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected override void WriteToDeveloperTrace(string message, int eventId, string category)
        {
            try
            {
                if (SharePointEnvironment.InSandbox)
                {
                    WriteSandboxTrace(message, eventId, category);
                }
                else
                {
                    this.TraceLogger.Trace(message, eventId, category);
                }
            }
            catch (Exception ex)
            {
                string logMessage = string.Format(CultureInfo.CurrentCulture, Properties.Resources.ErrorWritingTrace, message);
                throw BuildLoggingException(logMessage, ex);
            }
        }

        /// <summary>
        /// Writes a trace message for sandbox to trace logger.
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="eventId">The id for this message</param>
        /// <param name="severity">The severity of this message</param>
        /// <param name="category">The category for this message</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected override void WriteToDeveloperTrace(string message, int eventId, SandboxTraceSeverity severity, string category)
        {
            try
            {
                if (SharePointEnvironment.InSandbox)
                {
                    WriteToTraceSandbox(message, eventId, severity, category);
                }
                else
                {
                    WriteToTraceFullTrust(message, eventId, severity, category);
                }
            }
            catch (Exception ex)
            {
                // If the logging failed, throw an error that holds both the original error information and the 
                // reason why logging failed. Dont do this if only the tracing failed. 
                throw BuildLoggingException(message, ex);
            }
        }
       
        private void WriteToOperationsLogSandbox(string message, int eventId, string category)
        {
            WriteToOperationsLogSandbox(message, eventId, null, category);
        }

        /// <summary>
        /// By default the logger will drop operations messages if the proxy is not installed.  You can provide
        /// custom behavior by overriding this method and storing the logged message somwhere else for the sandbox.
        /// You will then need to replace the SharePointLogger ILogger mapping with your derived class through
        /// the service locator.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="eventId">The id of the event</param>
        /// <param name="severity">The severity of the event</param>
        /// <param name="category">The category of the event</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected virtual void WriteToOperationsLogSandbox(string message, int eventId, SandboxEventSeverity? severity, string category)
        {
            if (SharePointLogger.canAccessSandboxLogging == -1)
            {
                if (SharePointEnvironment.ProxyInstalled(TracingOperationArgs.OperationAssemblyName, TracingOperationArgs.OperationTypeName))
                {
                    SharePointLogger.canAccessSandboxTracing = 1;
                }
                else
                {
                    SharePointLogger.canAccessSandboxTracing = 0;
                }
            }

            if (SharePointLogger.canAccessSandboxTracing == 1)
            {
                var args = new LoggingOperationArgs();
                args.Message = message;
                args.EventId = eventId;
                args.Category = category;
                args.Severity = (int?)severity;

                if (SPContext.Current != null)
                    args.SiteID = SPContext.Current.Site.ID;
                else
                    args.SiteID = null;

                var result = SPUtility.ExecuteRegisteredProxyOperation(
                                        LoggingOperationArgs.OperationAssemblyName,
                                         LoggingOperationArgs.OperationTypeName,
                                         args);

                if (result != null && result.GetType().IsSubclassOf(typeof(System.Exception)))
                {
                    var ex = new LoggingException(Properties.Resources.SandboxLoggingFailed, (Exception)result);
                    throw ex;
                }
            }

        }

        private void WriteToOperationsLogFullTrust(string message, int eventId, SandboxEventSeverity severity, string category)
        {
            EventLogLogger.Log(message, eventId, GetEventSeverity(severity), category);
        }

        private void WriteSandboxTrace(string message, int eventId, string category)
        {
            WriteToTraceSandbox(message, eventId, null, category);
        }

        /// <summary>
        /// By default the logger will drop trace messages if the proxy is not installed.  You can provide
        /// custom behavior by overriding this method and storing the traced message somwhere else for the sandbox.
        /// You will then need to replace the SharePointLogger ILogger mapping with your derived class through
        /// the service locator.
        /// </summary>
        /// <param name="message">The message to trace</param>
        /// <param name="eventId">The id of the trace</param>
        /// <param name="severity">The severity of the trace</param>
        /// <param name="category">The category of the trace</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected virtual void WriteToTraceSandbox(string message, int eventId, SandboxTraceSeverity? severity, string category)
        {
            if (SharePointLogger.canAccessSandboxTracing == -1)
            {
                if (SharePointEnvironment.ProxyInstalled(TracingOperationArgs.OperationAssemblyName, TracingOperationArgs.OperationTypeName))
                {
                    SharePointLogger.canAccessSandboxTracing = 1;
                }
                else
                {
                    SharePointLogger.canAccessSandboxTracing = 0;
                }
            }

            if (SharePointLogger.canAccessSandboxTracing == 1)
            {

                var args = new TracingOperationArgs();
                args.Message = message;
                args.EventId = eventId;
                args.Category = category;
                args.Severity = (int?)severity;
                if (SPContext.Current != null)
                    args.SiteID = SPContext.Current.Site.ID;
                else
                    args.SiteID = null;

                var result = SPUtility.ExecuteRegisteredProxyOperation(
                                        TracingOperationArgs.OperationAssemblyName,
                                         TracingOperationArgs.OperationTypeName,
                                         args);

                if (result != null && result.GetType().IsSubclassOf(typeof(System.Exception)))
                {
                    var ex = new LoggingException(Properties.Resources.SandboxTraceFailed, (Exception)result);
                    throw ex;
                }
            }
        }

        private void WriteToTraceFullTrust(string message, int eventId, SandboxTraceSeverity severity, string category)
        {
            TraceLogger.Trace(message, eventId, GetTraceSeverity(severity), category);
        }

        /// <summary>
        /// Translates a <see cref="SandboxEventSeverity"/> into an EventSeverity.
        /// SandboxEventSeverity uses same enum values.
        /// </summary>
        /// <param name="severity">The severity of the event</param>
        /// <returns>the equlivalent SharePoint EventSeverity value</returns>
        public EventSeverity GetEventSeverity(SandboxEventSeverity severity)
        {
            return (EventSeverity)((int)severity);
        }

        /// <summary>
        /// Translates a <see cref="SandboxTraceSeverity"/> into an TraceSeverity.
        /// SandboxTraceSeverity uses the same enum values.
        /// </summary>
        /// <param name="severity">The severity of the trace</param>
        /// <returns>the equlivalent SharePoint TraceSeverity value</returns>
        public TraceSeverity GetTraceSeverity(SandboxTraceSeverity severity)
        {
            return (TraceSeverity)((int)severity);
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
        protected override string BuildEventLogMessage(string message, int eventId, EventSeverity severity,
                                                       string category)
        {
            return base.BuildEventLogMessage(message, eventId, severity, category) + BuildContextualInformationMessage();
        }

        /// <summary>
        /// Builds the event message to log
        /// </summary>
        /// <param name="message">the message to log</param>
        /// <param name="eventId">th id to associate with th event</param>
        /// <param name="category">the category for the event</param>
        /// <returns></returns>
        protected override string BuildEventLogMessage(string message, int eventId, string category)
        {
            return base.BuildEventLogMessage(message, eventId, category) + BuildContextualInformationMessage();
        }


        private LoggingException BuildLoggingException(string originalMessage, Exception errorLogException)
        {
            string errorMessage = Properties.Resources.LoggingExceptionMessage1;
            Exception innerException = null;

            // Build a dictionary with exception data, that can later be added to the exception
            Dictionary<object, object> exceptionData = new Dictionary<object, object>();
            exceptionData["OriginalMessage"] = originalMessage;

            // If there was an exception with logging to the exception log, add that data here:
            if (errorLogException != null)
            {
                string exceptionMessage = BuildExceptionMessage(errorLogException, null);
                errorMessage += Properties.Resources.LoggingExceptionMessage2 + exceptionMessage + Properties.Resources.LoggingExceptionMessage3 + originalMessage;
                exceptionData["ErrorLogException"] = exceptionMessage;

                // Preferably, use the exception from the error log as the inner exception
                innerException = errorLogException;
            } 

            // Now build the exception with the gathered information. 
            LoggingException loggingException = new LoggingException(errorMessage, innerException);
            foreach (object key in exceptionData.Keys)
            {
                loggingException.Data.Add(key, exceptionData[key]);
            }

            return loggingException;
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
        /// The default area/category for logging.
        /// </summary>
        public static string DefaultCategory
        {
            get
            {
                return Constants.DefaultAreaName + Constants.CategoryPathSeparator + Constants.DefaultCategoryName;
            }
        }
    }
}