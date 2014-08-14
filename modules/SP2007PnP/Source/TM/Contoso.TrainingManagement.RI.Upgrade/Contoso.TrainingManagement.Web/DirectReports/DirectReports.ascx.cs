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
using System.Security.Permissions;
using System.Web.UI;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace Contoso.TrainingManagement.ControlTemplates
{
    /// <summary>
    /// Code-behind class for the DirectReports user control.
    /// </summary>
    public partial class DirectReports : UserControl, IDirectReportsView
    {
        #region Methods

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected void Page_Load(object sender, EventArgs e)
        {
            DirectReportsPresenter presenter = new DirectReportsPresenter(this);
            presenter.SetDirectReportsSource(Request.Url.ToString());
            DirectReportsList.DataBind();
        }

        #endregion

        #region IDirectReportsView Members

        Dictionary<int, string> IDirectReportsView.DirectReports
        {
            set
            {
                DirectReportsList.DataSource = value;
            }
        }

        public string UserDisplayUrl
        {
            get;
            set;
        }

        public string SourceUrl
        {
            get;
            set;
        }

        public string DirectReportsMessage
        {
            set
            {
                Message.Text = value;
            }
        }

        public SPWeb Web
        {
            get;
            set;
        }

        public string LoginName
        {
            get;
            set;
        }

        public bool ShowLogin
        {
            get;
            set;
        }

        #endregion
    }
}