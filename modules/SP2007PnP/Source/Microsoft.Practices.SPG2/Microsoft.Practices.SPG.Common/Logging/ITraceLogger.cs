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
using System.Linq;
using System.Security.Permissions;
using System.Text;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SPG.Common.Logging
{
    /// <summary>
    /// Interface for classes that log to the trace log.
    /// 
    /// This interface is primarily used to be able to register an <see cref="ITraceLogger"/> in the service locator.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = "Service locator doesn't work with custom attributes")]
    public interface ITraceLogger
    {
        /// <summary>
        /// Log a message with specified <paramref name="message"/>, <paramref name="eventId"/>, <paramref name="severity"/>
        /// and <paramref name="category"/>.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event.
        /// </param>
        /// <param name="severity">How serious the event is. </param>
        /// <param name="category">The category of the log message.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void Trace(string message, int eventId, TraceSeverity severity, string category);
    }
}
