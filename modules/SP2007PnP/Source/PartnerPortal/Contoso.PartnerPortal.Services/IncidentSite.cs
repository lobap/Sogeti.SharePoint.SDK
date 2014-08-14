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
using System.Globalization;
using System.Linq;
using System.Security.Permissions;
using System.Threading;
using System.Web;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SPG.Common.ServiceLocation;

using Contoso.Common.Repositories;
using Microsoft.Practices.SPG.SubSiteCreation;
using Contoso.PartnerPortal.PartnerDirectory;
using Contoso.PartnerPortal.Services.Security;

namespace Contoso.PartnerPortal.Services
{
    public class IncidentSite : IIncidentSite
    {
        public IncidentSite()
        {
            this.SecurityHelper = new SecurityHelper();
        }

        #region IIncidentManagement Members

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel=true)]
        public void UpdateIncidentStatus(string incidentId, string partner, string status)
        {
            this.SecurityHelper.DemandAuthorizedPermissions();

            IPartnerSiteDirectory partnerSiteDirectory = SharePointServiceLocator.Current.GetInstance<IPartnerSiteDirectory>();
            string partnerSiteCollectionUrl = partnerSiteDirectory.GetPartnerSiteCollectionUrl(partner);

            IBusinessEventTypeConfigurationRepository businessEventTypeConfigRepository
                = SharePointServiceLocator.Current.GetInstance<IBusinessEventTypeConfigurationRepository>();
            string topLevelSiteUrl = businessEventTypeConfigRepository.GetBusinessEventTypeConfiguration("incident").TopLevelSiteRelativeUrl;

            using (SPSite site = new SPSite(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", partnerSiteCollectionUrl, topLevelSiteUrl)))
            {
                using (SPWeb incidentsWeb = site.OpenWeb())
                {
                    foreach (SPWeb web in incidentsWeb.Webs)
                    {
                        try
                        {
                            if (web.Properties["IncidentId"] == incidentId)
                            {
                                web.Properties["Status"] = "Closed";
                                web.Properties.Update();

                                break;
                            }
                        }
                        finally
                        {
                            web.Dispose();
                        }
                    }
                }
            }
        }

        #endregion

        protected ISecurityHelper SecurityHelper { get; set; }
    }
}
