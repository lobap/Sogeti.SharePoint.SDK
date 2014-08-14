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
using System.Web.UI;
using Contoso.PartnerPortal.PartnerCentral.PartnerRollup;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

namespace Contoso.PartnerPortal.PartnerCentral.Controls
{
    public partial class PartnerRollupControl : UserControl, IPartnerRollupView
    {
        public PartnerRollupControl()
        {
            
        }

        #region IPartnerRollupView Members

        public  void SetData(IEnumerable<PartnerRollupSearchResult> data)
        {
            this.PartnerRepeater.DataSource = data;
            this.PartnerRepeater.DataBind();
        }

        #endregion
    }
}
