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
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;
using Microsoft.Practices.SharePoint.Common.Properties;
using System.Collections.Generic;

namespace Microsoft.Practices.SharePoint.Common.Logging
{
    /// <summary>
    /// Loging implementation that writes the log messages to the EventLog.
    /// </summary>
    public class EventLogLogger : IEventLogLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogLogger"/> class.
        /// </summary>
        public EventLogLogger()
        {
        }

        /// <summary>
        /// Overrides the Log method to write messages to the EventLog. 
        /// </summary>
        /// <param name="message">Message to write</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        /// <param name="severity">The severity of the exception.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void Log(string message, int eventId, EventSeverity severity, string category)
        {
            DiagnosticsService diagnosticService = DiagnosticsService.Local;
            diagnosticService.LogEvent(message, eventId, severity, category);
        }

        /// <summary>
        /// Overrides the Log method to write messages to the EventLog, writes the event using the 
        /// default severity for the category.
        /// </summary>
        /// <param name="message">Message to write</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void Log(string message, int eventId, string category)
        {
            DiagnosticsService diagnosticService = DiagnosticsService.Local;
            diagnosticService.LogEvent(message, eventId, category);
        }
    }
}