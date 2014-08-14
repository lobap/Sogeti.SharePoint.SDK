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
using System.Runtime.InteropServices;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using Contoso.Common.Repositories;
using Contoso.PartnerPortal.Collaboration.Incident.Repositories;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace Contoso.PartnerPortal.Collaboration.Incident
{
    [Guid("86dfa338-3643-49c3-a935-26b043547326")]
    [CLSCompliant(false)]
    public class ContosoPartnerPortalIncidentFeatureReceiver : SPFeatureReceiver
    {
        #region Private Constants

        private const string IncidentDashboardFileName = "incidentdashboard.aspx";
        private const string DefaultFileName = "default.aspx";

        #endregion

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb incidentsWeb = properties.Feature.Parent as SPWeb;
            SPFile file = incidentsWeb.Files[IncidentDashboardFileName];
            file.MoveTo(DefaultFileName, true);

            ServiceLocatorConfig config = new ServiceLocatorConfig();
            config.RegisterTypeMapping<IIncidentTaskRepository, FullTextSearchIncidentTaskRepository>();
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {

        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {

        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {

        }
    }
}
