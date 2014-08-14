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
using System.Runtime.InteropServices;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Serialization;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;

using Contoso.PartnerPortal.Collaboration.Incident.Controls;

namespace Contoso.PartnerPortal.Collaboration.Incident.WebParts.IncidentStatusList
{
    [Guid("f8df25d5-4f7c-41fa-985f-10caa88bb874")]
    public class IncidentStatusListWebPart : System.Web.UI.WebControls.WebParts.WebPart
    {
        public IncidentStatusListWebPart()
        {
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            IncidentStatusListControl incidentStatusListControl = (IncidentStatusListControl)Page.LoadControl("~/_controltemplates/Contoso/IncidentStatusListControl.ascx");
            IncidentStatusListPresenter presenter = new IncidentStatusListPresenter(incidentStatusListControl);
            presenter.SetIncidentStatusListData();

            this.Controls.Add(incidentStatusListControl);
        }
    }
}
