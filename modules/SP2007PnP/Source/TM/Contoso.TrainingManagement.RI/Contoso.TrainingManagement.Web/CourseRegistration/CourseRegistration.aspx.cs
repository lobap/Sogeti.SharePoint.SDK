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
using System.Security.Permissions;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

using Contoso.TrainingManagement.Repository.BusinessEntities;

namespace Contoso.TrainingManagement.Forms
{
    /// <summary>
    /// Code-behind file for the CourseRegistration.aspx form.
    /// </summary>
    public partial class CourseRegistration : Page, ICourseRegistrationView
    {
        #region Methods

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected void Page_Load(object sender, EventArgs e)
        {
            if ( !Page.IsPostBack )
            {
                CourseRegistrationPresenter courseRegistrationPresenter = new CourseRegistrationPresenter(this);
                courseRegistrationPresenter.RenderCourseRegistrationView(SPContext.Current.Web, SPContext.Current.Web.CurrentUser.LoginName);

                this.CourseList.DataValueField = "Id";
                this.CourseList.DataTextField = "Code";
                this.CourseList.DataBind();
            }
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected void Submit_Click(object sender, EventArgs e)
        {            
            CourseRegistrationPresenter presenter = new CourseRegistrationPresenter(this);
            presenter.Register(SPContext.Current.Web, SPContext.Current.Web.CurrentUser.LoginName);
        }

        #endregion

        #region ICourseRegistrationView Members

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

        public string ContentMessage
        {
            set
            {
                MainContent.Text = value;
            }
        }

        public bool ShowConfirmationControls
        {
            set
            {
                Confirmation.Visible = value;
            }
        }

        public IList<TrainingCourse> Courses
        {
            set
            {
                CourseList.DataSource = value;
            }
        }

        public bool ShowCourseSelectionControls
        {
            set
            {
                CourseSelect.Visible = value;
            }
        }

        public NameValueCollection QueryString
        {
            get
            {
                return Request.QueryString;
            }
        }

        public string SelectedCourse
        {
            get
            {
                return CourseList.SelectedValue;
            }
        }

        public string SiteLink
        {
            set
            {
                GoBackToSiteLink.NavigateUrl = value;
            }
        }

        #endregion
    }
}
