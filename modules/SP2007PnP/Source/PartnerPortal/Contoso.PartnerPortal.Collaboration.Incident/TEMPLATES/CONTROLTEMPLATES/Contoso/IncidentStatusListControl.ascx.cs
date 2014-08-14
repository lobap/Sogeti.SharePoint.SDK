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
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contoso.PartnerPortal.Collaboration.Incident.WebParts.IncidentStatusList;

namespace Contoso.PartnerPortal.Collaboration.Incident.Controls
{
    public partial class IncidentStatusListControl : UserControl, IIncidentStatusListView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.IncidentStatusRepeater.DataBind();
        }

        #region IIncidentStatusListView Members

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<IncidentStatus> Data
        {
            set
            {
                this.IncidentStatusRepeater.DataSource = value;
            }
        }

        #endregion
    }
}
