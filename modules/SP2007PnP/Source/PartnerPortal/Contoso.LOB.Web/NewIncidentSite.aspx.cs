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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Contoso.LOB.Services.Data;
//using Contoso.LOB.Services.BusinessEntities;
using Contoso.LOB.Services.IncidentManagement;
using Contoso.PartnerPortal.Services.SubSiteCreation;
using Contoso.PartnerPortal.Services.IncidentSite;
using System.Globalization;
using System.Net;

namespace Contoso.LOB.Web
{
    public partial class NewIncidentSite : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Incident[] incidents;
                using (IncidentManagementClient incidentManagement = new IncidentManagementClient())
                {
                    incidentManagement.ClientCredentials.UserName.UserName = "ContosoWinServiceAccount";
                    incidentManagement.ClientCredentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
                    incidents = incidentManagement.GetAllIncidents();
                }

                foreach (Incident incident in incidents)
                {
                    ListItem item = new ListItem();
                    item.Value = incident.Id.ToString(CultureInfo.CurrentCulture);
                    item.Text = string.Format(CultureInfo.CurrentCulture, "[Partner:{0}] [IncidentId:{1}] - {2}: {3}", incident.Partner, incident.Id, incident.Product, incident.Description);
                    this.incidentsDropDownList.Items.Add(item);
                }
            }
        }

        protected void CreateSite_Click(object sender, EventArgs e)
        {
            string id = incidentsDropDownList.SelectedItem.Value;
            Incident incident;
            using (IncidentManagementClient incidentManagement = new IncidentManagementClient())
            {
                incidentManagement.ClientCredentials.UserName.UserName = "ContosoWinServiceAccount";
                incidentManagement.ClientCredentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
                incident = incidentManagement.GetIncident(id);
            }
            
            SubSiteCreationClient client = new SubSiteCreationClient();
            client.CreateSubSite("Incident", id, incident.Partner);
        }

        protected void CloseIncident_Click(object sender, EventArgs e)
        {
            string id = incidentsDropDownList.SelectedItem.Value; 
            Incident incident;
            using (IncidentManagementClient incidentManagement = new IncidentManagementClient())
            {
                incidentManagement.ClientCredentials.UserName.UserName = "ContosoWinServiceAccount";
                incidentManagement.ClientCredentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
                incident = incidentManagement.GetIncident(id);
            }

            IncidentSiteClient client = new IncidentSiteClient();
            client.UpdateIncidentStatus(id, incident.Partner, "closed");
        }
    }
}
