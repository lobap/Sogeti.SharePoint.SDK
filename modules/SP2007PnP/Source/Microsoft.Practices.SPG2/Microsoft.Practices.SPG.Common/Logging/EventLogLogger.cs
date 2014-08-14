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
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;
using Microsoft.Practices.SPG.Common.Properties;

namespace Microsoft.Practices.SPG.Common.Logging
{
    /// <summary>
    /// Loging implementation that writes the log messages to the EventLog.
    /// </summary>
    [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)] 
    public class EventLogLogger : IEventLogLogger
    {
        private string eventSource;
        private const string DefaultEventSource = "Office SharePoint Server";

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogLogger"/> class.
        /// </summary>
        public EventLogLogger()
        {
            IHierarchicalConfig config = SharePointServiceLocator.Current.GetInstance<IHierarchicalConfig>();

            // Assume default event soruce
            eventSource = DefaultEventSource;

            if (SPContext.Current == null)
            {
                // There is no SharePoint context: Get the config from the farm. 
                if (config.ContainsKey(Constants.EventSourceNameConfigKey, ConfigLevel.CurrentSPFarm))
                {
                    eventSource = config.GetByKey<string>(Constants.EventSourceNameConfigKey, ConfigLevel.CurrentSPFarm);
                }
            }
            else
            {
                // There is a SharePoint context. Get the config from the current SPWeb
                if (config.ContainsKey(Constants.EventSourceNameConfigKey))
                {
                    eventSource = config.GetByKey<string>(Constants.EventSourceNameConfigKey);
                }
            }
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
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void Log(string message, int eventId, EventLogEntryType severity, string category)
        {
            RunElevated( delegate
                      {
                          WriteEventLogEntry(message, severity, eventId, category);
                      });
        }

        private void WriteEventLogEntry(string message, EventLogEntryType severity, int eventId, string category)
        {
            EnsureEventSource();

            message = string.Format(CultureInfo.CurrentCulture, "Category: {0}\n{1}", category, message);

            // When using Office SharePoint Server as the event source, the event Id must be 0. 
            // If the event Id is not zero, the message value does not get written to the description.
            // The event Id shows up in the description instead.
            if (eventSource == DefaultEventSource)
            {
                EventLog.WriteEntry(eventSource, message, severity, 0);
            }
            else
            {
                EventLog.WriteEntry(eventSource, message, severity, eventId);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void EnsureEventSource()
        {
            try
            {
                if (eventSource == DefaultEventSource)
                {
                    // Custom applications should not be logging to the Office SharePoint Server's event log.
                    // If the eventSource is still DefaultEventSource, then log a warning indicating
                    // that a custom event source should be specified.
                    string warning = string.Format(CultureInfo.CurrentCulture, Resources.CreateEventSourceWarning, DefaultEventSource);
                    EventLog.WriteEntry(DefaultEventSource, warning, EventLogEntryType.Warning, 0);
                    return;
                }

                if (!EventLog.SourceExists(eventSource))
                {
                    EventLog.CreateEventSource(eventSource, "Application");
                }
            }
            catch (Exception ex)
            {
                string originalEventSource = eventSource;
                // Reset the eventsource to the default one. 
                eventSource = DefaultEventSource;

                string errorMessage = string.Format(CultureInfo.CurrentCulture, Resources.CouldNotCreateEventSource, originalEventSource, ex);
                EventLog.WriteEntry(DefaultEventSource, errorMessage, EventLogEntryType.Error, 0);
            }
        }


        /// <summary>
        /// Runs the method in an elevated SharePoint context. 
        /// </summary>
        /// <param name="method">method to run</param>
        protected virtual void RunElevated(SPSecurity.CodeToRunElevated method)
        {
            SPSecurity.RunWithElevatedPrivileges(method);
        }
    }
}