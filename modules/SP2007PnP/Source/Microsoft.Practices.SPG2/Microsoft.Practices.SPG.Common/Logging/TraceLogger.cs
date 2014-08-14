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
using System.Reflection;
using System.Security.Permissions;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SPG.Common.Logging
{
    /// <summary>
    /// Class that can log messages into the SharePoint ULS trace log. 
    /// </summary>
    public class TraceLogger : ITraceLogger
    {
        private static readonly object syncRoot = new object();

        /// <summary>
        /// Write messages into the SharePoint ULS. 
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system.</param>
        /// <param name="severity">How serious the event is.</param>
        /// <param name="category">The category of the log message.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands"), SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void Trace(string message, int eventId, TraceSeverity severity, string category)
        {
            //Only users with an interactive logon session can write to the EventLog.
            //So we have to run this under elevated privileges.
            SPSecurity.RunWithElevatedPrivileges(() => WriteLogMessage(severity, category, message, eventId));
        }

        private void WriteLogMessage(TraceSeverity severity, string category, string message, int eventId)
        {
            lock (syncRoot)
            {
                uint tag = (uint)eventId;
                string exeName =
                    Assembly.GetCallingAssembly().GetName().Name;
                string productName = "Custom SharePoint application";
                ULSTraceProvider.WriteTrace(tag, severity,
                                            GetCorrelationGuid(), exeName,
                                            productName, category,
                                            message);
            }
        }

        /// <summary>
        /// The correlation guid is used to match this trace message with others. 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected virtual Guid GetCorrelationGuid()
        {
            if (System.Diagnostics.Trace.CorrelationManager.ActivityId == Guid.Empty)
            {
                System.Diagnostics.Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            }

            return System.Diagnostics.Trace.CorrelationManager.ActivityId;
        }
    }
}