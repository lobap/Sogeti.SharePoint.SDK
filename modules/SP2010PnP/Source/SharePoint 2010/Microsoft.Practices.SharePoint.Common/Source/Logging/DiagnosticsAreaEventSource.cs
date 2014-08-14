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
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.Configuration;
using System.Diagnostics;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;

namespace Microsoft.Practices.SharePoint.Common.Logging
{
    /// <summary>
    /// Creates the event sources for configured diagnostic areas.
    /// </summary>
    public class DiagnosticsAreaEventSource
    {
        /// <summary>
        /// Ensures that all configured DiagnosticAreas are registered as event sources.
        /// </summary>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public static void EnsureConfiguredAreasRegistered()
        {
            var mgr = SharePointServiceLocator.GetCurrent().GetInstance<IConfigManager>();
            var areas = new DiagnosticsAreaCollection(mgr);
            RegisterAreas(areas);
        }

        /// <summary>
        /// Takes all the configured areas and set them as event sources.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public static void RegisterAreas(DiagnosticsAreaCollection areas)
        {
            Validation.ArgumentNotNull(areas, "areas");

            foreach (DiagnosticsArea area in areas)
            {
                if (!EventLog.SourceExists(area.Name))
                {
                    EventLog.CreateEventSource(area.Name, Constants.EventLogName);
                }
            }

            if (!EventLog.SourceExists(DiagnosticsArea.DefaultSPDiagnosticsArea.Name))
            {
                EventLog.CreateEventSource(DiagnosticsArea.DefaultSPDiagnosticsArea.Name, Constants.EventLogName);
            }
        }
    }
}
