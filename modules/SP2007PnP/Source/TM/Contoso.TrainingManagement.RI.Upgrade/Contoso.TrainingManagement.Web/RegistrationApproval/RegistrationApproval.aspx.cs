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
using System.Web;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;

namespace Contoso.TrainingManagement.Forms
{
    /// <summary>
    /// Code-behind class for the RegistrationApproval.aspx form.
    /// </summary>
    public partial class RegistrationApproval : LayoutsPageBase, IRegistrationApprovalView
    {
        #region Private Fields

        private const string taskIdViewStateKey = "ra_taskID";
        private const string sourceUrlViewStateKey = "ra_sourceUrl";
        private const string managerDashboardPageFileName = "managerdashboard.aspx";

        #endregion

        #region Constructors

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public RegistrationApproval(){ }

        #endregion

        #region Methods

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Validate();
            if ( !Page.IsPostBack || !Page.IsValid )
            {
                string sourceUrl = String.Empty;
                if ( Request.QueryString["Source"] != null )
                {
                    sourceUrl = Request.QueryString["Source"];
                }

                RenderView(SPContext.Current.Web, Request.Params["ID"], sourceUrl);
            }
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected void Submit_Click(object sender, EventArgs e)
        {
            if ( Page.IsValid )
            {
                ProcessApproval(SPContext.Current.Web, (string)ViewState[taskIdViewStateKey], (string)ViewState[sourceUrlViewStateKey]);
            }
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private void RenderView(SPWeb web, string taskID, string sourceUrl)
        {
            RegistrationApprovalPresenter presenter = new RegistrationApprovalPresenter(this);
            bool success = presenter.RenderRegApprovalView(web, taskID);

            ContentMessage.Text = ContentMessage.Text.Replace("\r\n", "<br />");

            if ( success )
            {
                ViewState[taskIdViewStateKey] = taskID;
                if (String.IsNullOrEmpty(sourceUrl))
                {                    
                    sourceUrl = String.Format("{0}/{1}", web.Url, managerDashboardPageFileName);
                }
                ViewState[sourceUrlViewStateKey] = sourceUrl;

                Status.DataBind();
            }
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private void ProcessApproval(SPWeb web, string taskID, string sourceUrl)
        {
            RegistrationApprovalPresenter presenter = new RegistrationApprovalPresenter(this);
            bool success = presenter.ProcessApproval(web, taskID, Status.SelectedValue);

            if ( success )
            {
                SPUtility.Redirect(sourceUrl, SPRedirectFlags.DoNotEncodeUrl, HttpContext.Current);
            }
        }

        #endregion

        #region IRegistrationApprovalView Members

        public string PageTitle
        {
            set
            {
                ListFormPageTitle.Text = value;
            }
        }

        public string HeaderTitle
        {
            set
            {
                LinkTitle.Text = value;
            }
        }

        public string HeaderSubtitle
        {
            set
            {
                ItemProperty.Text = value;
            }
        }

        public string Message
        {
            set
            {
                ContentMessage.Text = value;
            }
        }

        IList<string> IRegistrationApprovalView.Status
        {
            set
            {
                Status.DataSource = value;
            }
        }

        public bool ShowConfirmationControls
        {
            set
            {
                Confirmation.Visible = value;
            }
        }

        #endregion
    }
}
