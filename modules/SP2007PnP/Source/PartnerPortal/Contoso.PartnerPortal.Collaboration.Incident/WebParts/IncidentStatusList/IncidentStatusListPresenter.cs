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
using System.Text;
using Microsoft.SharePoint.Publishing.Navigation;
using Microsoft.SharePoint.Publishing;
using System.Globalization;

namespace Contoso.PartnerPortal.Collaboration.Incident.WebParts.IncidentStatusList
{
    public class IncidentStatusListPresenter
    {
        #region Private Constants

        private const string BusinessEventProperty = "businessevent";
        private const string StatusProperty = "status";

        #endregion

        IIncidentStatusListView view;

        public IncidentStatusListPresenter(IIncidentStatusListView view)
        {
            this.view = view;
        }

        public void SetIncidentStatusListData()
        {
            PortalSiteMapProvider provider = PortalSiteMapProvider.CombinedNavSiteMapProvider;
            if (provider.CurrentNode != null && provider.CurrentNode.HasChildNodes)
            {
                List<IncidentStatus> incidentStatuses = new List<IncidentStatus>();
                foreach (PortalWebSiteMapNode node in provider.GetChildNodes(provider.CurrentNode))
                {
                    if (node.GetProperty(BusinessEventProperty) != null && node.GetProperty(BusinessEventProperty).ToString().ToLower(CultureInfo.CurrentCulture) == "incident")
                    {
                        IncidentStatus incidentStatus = new IncidentStatus();
                        incidentStatus.Title = node.Title;
                        incidentStatus.Url = node.Url;
                        incidentStatus.Status = (string)node.GetProperty(StatusProperty);
                        incidentStatus.CreatedDate = node.CreatedDate.ToString();
                        incidentStatus.LastModifiedDate = node.LastModifiedDate.ToString();
                        
                        incidentStatuses.Add(incidentStatus);
                    }
                }

                view.Data = incidentStatuses;
            }
        }
    }
}
