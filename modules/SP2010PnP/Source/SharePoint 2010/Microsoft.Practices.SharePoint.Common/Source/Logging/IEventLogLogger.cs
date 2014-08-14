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
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SharePoint.Common.Logging
{

    /// <summary>
    /// Interface for classes that log to the EventLog. 
    /// 
    /// This interface is primarily used to be able to register an <see cref="IEventLogLogger"/> in the service locator.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = "Service locator doesn't work with attributes")]
    public interface IEventLogLogger
    {
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
        void Log(string message, int eventId, EventSeverity severity, string category);

        /// <summary>
        /// Log a message with specified <paramref name="message"/>, <paramref name="eventId"/>, 
        /// and <paramref name="category"/> using the default severity for the category.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="category">The category of the log message.</param>
        void Log(string message, int eventId, string category);
    }
}
