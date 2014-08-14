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
using System.Runtime.InteropServices;
using System.Web.UI;
using System.Text;
using Contoso.Common.Repositories;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint;
using System.Globalization;

namespace Contoso.PartnerPortal.Collaboration.Incident.WebParts
{
    [Guid("4e02ab7f-8fae-4835-822a-48b667c9d7cc")]
    public class IncidentInfoWebPart : System.Web.UI.WebControls.WebParts.WebPart
    {

        public IncidentInfoWebPart()
        {

        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            IIncidentManagementRepository incidentManagementRepository =
                SharePointServiceLocator.Current.GetInstance<IIncidentManagementRepository>();
                
            // The Identifier of the incident that the current site is collaborating on is located
            // in the property bag. The identifier is stored during the subsite creation workflow. 
            var incident = incidentManagementRepository.GetIncident(SPContext.Current.Web.Properties["incidentId"]);

            StringBuilder content =
                new StringBuilder("<h2>Incident Information (from incident management service)</h2>");

            if (incident == null)
            {
                content.AppendLine("incident information is unavailable.");
            }
            else
            {

                content.AppendLine(string.Format(CultureInfo.CurrentCulture, "<h3>Description: {0}</h3>", incident.Description));
                content.AppendLine(string.Format(CultureInfo.CurrentCulture, "<h3>Status: {0}</h3>", incident.Status));
                content.AppendLine(string.Format(CultureInfo.CurrentCulture, "<h3>Product: {0}</h3>", incident.Product));
                content.AppendLine("<h3>History:</h3>");
                foreach (string note in incident.History)
                {
                    content.AppendLine(string.Format(CultureInfo.CurrentCulture, "<li>{0}</li>", note));
                }
            }

            LiteralControl literal = new LiteralControl();
            literal.Text = content.ToString();
            this.Controls.Add(literal);
            
        }
    }
}
