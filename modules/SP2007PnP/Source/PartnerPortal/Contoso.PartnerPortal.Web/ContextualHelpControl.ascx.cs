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
using Microsoft.SharePoint;

namespace Contoso.PartnerPortal.ContextualHelp
{
    public partial class ContextualHelpControl : System.Web.UI.UserControl, IContextualHelpView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ContextualHelpPresenter presenter = new ContextualHelpPresenter(this);
            presenter.SetContent(HttpContext.Current.Request.FilePath);
        }

        #region IContextualHelpView Members

        public void SetContent(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                this.Visible = false;
            }
            else
            {
                this.Literal1.Text = content;
            }
        }

        #endregion
    }
}