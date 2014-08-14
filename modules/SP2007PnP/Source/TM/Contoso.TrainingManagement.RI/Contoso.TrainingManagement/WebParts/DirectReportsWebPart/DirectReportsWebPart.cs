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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using WebPart = System.Web.UI.WebControls.WebParts.WebPart;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;

using Contoso.TrainingManagement.ControlTemplates;

namespace Contoso.TrainingManagement.WebParts
{
    /// <summary>
    /// The DirectsRreportWebPart displays information about
    /// current user's direct reports.
    /// </summary>
    [Guid("fa39abe9-4672-4be3-b9d2-290c4d7a8822")]
    public class DirectReportsWebPart : WebPart
    {
        #region Private Fields

        private bool showLogin;

        #endregion

        #region Properties

        /// <summary>
        /// The ShowLogin property indicates that the current
        /// user's direct reports' login information be displayed.
        /// </summary>
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]                
        [WebDisplayName("Show Login Name")]
        [WebDescription("Show login name for direct reports.")]
        [SPWebCategoryName("Contoso")]
        public bool ShowLogin
        {
            get
            {
                return showLogin;
            }
            set
            {
                this.showLogin = value;
            }
        }

        #endregion

        #region Methods

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            try
            {
                //Loads a user control
                DirectReports directReports = (DirectReports)Page.LoadControl("~/_controltemplates/TrainingManagement/DirectReports.ascx");
                directReports.Web = SPContext.Current.Web;
                directReports.LoginName = SPContext.Current.Web.CurrentUser.LoginName;
                directReports.ShowLogin = this.showLogin;
                this.Controls.Add(directReports);
            }
            catch ( HttpException ex )
            {
                this.Controls.Add(new LiteralControl("<br />An unexpected error occurred loading Web Part. " + ex.Message));
            }
        }

        #endregion
    }
}
