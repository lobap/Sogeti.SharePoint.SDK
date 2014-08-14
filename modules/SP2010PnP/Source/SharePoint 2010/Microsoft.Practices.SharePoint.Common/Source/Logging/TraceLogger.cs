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
using System.Reflection;
using System.Security.Permissions;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;
using System.Collections.Generic;

namespace Microsoft.Practices.SharePoint.Common.Logging
{
    /// <summary>
    /// Class that can log messages into the SharePoint trace log. 
    /// </summary>
    public class TraceLogger : ITraceLogger
    { 
        /// <summary>
        /// Write messages into the SharePoint ULS. 
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the trace.</param>
        /// <param name="severity">How serious the trace is.</param>
        /// <param name="category">The category of the log message.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void Trace(string message, int eventId, TraceSeverity severity, string category)
        {
            DiagnosticsService diagnosticService = DiagnosticsService.Local;
            diagnosticService.LogTrace(message, eventId, severity, category);
        }

        /// <summary>
        /// Write messages into the SharePoint ULS using the default severity for
        /// the category. 
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the trace.</param>
        /// <param name="category">The category of the trace message.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void Trace(string message, int eventId, string category)
        {
            DiagnosticsService diagnosticService = DiagnosticsService.Local;
            diagnosticService.LogTrace(message, eventId, category);
        }
    }
}