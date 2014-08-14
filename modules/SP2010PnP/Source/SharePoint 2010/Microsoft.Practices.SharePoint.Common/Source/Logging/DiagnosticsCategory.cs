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
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;

namespace Microsoft.Practices.SharePoint.Common.Logging
{
    /// <summary>
    /// Class representing an area for diagnostic purpose.
    /// </summary>
    public class DiagnosticsCategory
    {
        /// <summary>
        /// The default event severity for a diagnostics category
        /// </summary>
        public const EventSeverity DefaultEventSeverity = EventSeverity.Warning;

        /// <summary>
        /// The default trace severity for a diagnostics category
        /// </summary>
        public const TraceSeverity DefaultTraceSeverity = TraceSeverity.Medium;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsCategory"/> class.
        /// </summary>
        public DiagnosticsCategory()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsCategory"/> class.
        /// </summary>
        /// <param name="name">The name of the diagnostic category.</param>
         public DiagnosticsCategory(string name):
            this(name, DefaultEventSeverity, DefaultTraceSeverity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsCategory"/> class.
        /// </summary>
        /// <param name="name">The name of the diagnostic category.</param>
        /// <param name="eventSeverity">The category's event severity.</param>
        /// <param name="traceSeverity">The category´s trace severity</param>
        public DiagnosticsCategory(string name, EventSeverity eventSeverity, TraceSeverity traceSeverity)
        {
            this.Name = name;
            this.EventSeverity = eventSeverity;
            this.TraceSeverity = traceSeverity;
        }

        private uint id;
        private string name;
        private TraceSeverity traceSeverity;
        private EventSeverity eventSeverity;

        /// <summary>
        /// The id of the diagnostic category
        /// </summary>
        public uint Id
        {
            get { return id; }

            set { id = value; }
        }

        /// <summary>
        /// The name of the diagnostic category
        /// </summary>
        public string Name
        {
             get { return name; }

            set
            {
                Validation.ArgumentNotNullOrEmpty(value, "name");
                name = value;
            }
        }

        /// <summary>
        /// Gets or sets the trace severity.
        /// </summary>
        public TraceSeverity TraceSeverity
        {
             get { return traceSeverity; }

            set { traceSeverity = value; }
        }

        /// <summary>
        /// Gets or sets the event severity.
        /// </summary>
        public EventSeverity EventSeverity
        {
            get { return eventSeverity; }

            set { eventSeverity = value; }
        }

        /// <summary>
        /// Converts from <see cref="DiagnosticsCategory"/> to <see cref="SPDiagnosticsCategory"/> instance.
        /// </summary>
        /// <returns></returns>
        public SPDiagnosticsCategory ToSPDiagnosticsCategory()
        {
            return new SPDiagnosticsCategory(
                this.Name, this.Name, this.TraceSeverity, this.EventSeverity, 0, this.Id, false, true);
        }

        /// <summary>
        /// Gets the default <see cref="SPDiagnosticsCategory"/> instance.
        /// </summary>
        public static SPDiagnosticsCategory DefaultSPDiagnosticsCategory
        {
            get
            {
                return new SPDiagnosticsCategory(Constants.DefaultCategoryName,
                    Constants.DefaultCategoryName, TraceSeverity.Medium,  EventSeverity.Information, 0, 0, false, true);
            }
        }
    }
}
