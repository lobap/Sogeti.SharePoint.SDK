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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint;
using Microsoft.Practices.SPG.SubSiteCreation.Properties;

namespace Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities
{
    /// <summary>
    /// Workflow activity that will create the actual subsite as part of the Subsite creation workflow. 
    /// </summary>
    [Designer(typeof(CreateSubSiteActivityDesigner))]
    [ToolboxBitmap(typeof(CreateSubSiteActivity), "NewWebsite.bmp")]
    public partial class CreateSubSiteActivity : Activity
    {
        private const string SiteCollenctionUrlDescription = "The URL of the site collection where subsite should be created.";
        private const string MicrosoftSPGSiteCreationCategory = "Microsoft SPG Site Creation.";
        private const string SiteTemplateNameDescription = "The site template that should be to create subsite.";
        private const string BusinessEventDescription = "The name of the Business Event.";
        private const string BusinessEventIdDescription = "The identifier of the business event.";
        private const string SubSiteUrlDescription = "The URL of the newly created subsite";
        private const string BusinessEventIdKeyDescription = "The business Event Id in the property bag of the SPWeb that's created.";
        private const string DefaultSiteTemplate = "STS#1";
        private const string BusienssEventPopertyIndexKey = "businessevent";
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSubSiteActivity"/> class.
        /// </summary>
        public CreateSubSiteActivity()
        {
            InitializeComponent();
        }

        #region Properties

        /// <summary>
        /// Dependency Proeprty for the SiteCollection URL. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static DependencyProperty SiteCollectionUrlProperty = DependencyProperty.Register("SiteCollectionUrl", typeof(string), typeof(CreateSubSiteActivity));


        /// <summary>
        /// The URL of the site collection to create the subsite in. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        [Description(SiteCollenctionUrlDescription)]
        [Category(MicrosoftSPGSiteCreationCategory)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string SiteCollectionUrl
        {
            get
            {
                return ((string)(base.GetValue(CreateSubSiteActivity.SiteCollectionUrlProperty)));
            }
            set
            {
                base.SetValue(CreateSubSiteActivity.SiteCollectionUrlProperty, value);
            }
        }

        /// <summary>
        /// Dependency property for the TopLevelSiteRelativeUrl
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static DependencyProperty TopLevelSiteRelativeUrlProperty = DependencyProperty.Register("TopLevelSiteRelativeUrl", typeof(string), typeof(CreateSubSiteActivity));

        /// <summary>
        /// URL for the Top level site. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), Description("The site collection relative URL of the top level site where subsite should be created.")]
        [Category(MicrosoftSPGSiteCreationCategory)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string TopLevelSiteRelativeUrl
        {
            get
            {
                return ((string)(base.GetValue(CreateSubSiteActivity.TopLevelSiteRelativeUrlProperty)));
            }
            set
            {
                base.SetValue(CreateSubSiteActivity.TopLevelSiteRelativeUrlProperty, value);
            }
        }

        /// <summary>
        /// Dependency property of the SiteTemplateNameProperty
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static DependencyProperty SiteTemplateNameProperty = DependencyProperty.Register("SiteTemplateName", typeof(string), typeof(CreateSubSiteActivity));

        /// <summary>
        /// The name of the site template that the site to create on will be based on. 
        /// </summary>
        [Description(SiteTemplateNameDescription)]
        [Category(MicrosoftSPGSiteCreationCategory)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string SiteTemplateName
        {
            get
            {
                return ((string)(base.GetValue(CreateSubSiteActivity.SiteTemplateNameProperty)));
            }
            set
            {
                base.SetValue(CreateSubSiteActivity.SiteTemplateNameProperty, value);
            }
        }

        /// <summary>
        /// Dependency property for the Business Event
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static DependencyProperty BusinessEventProperty = DependencyProperty.Register("BusinessEvent", typeof(string), typeof(CreateSubSiteActivity));

        /// <summary>
        /// The name of the business event that has occurred. 
        /// </summary>
        [Description(BusinessEventDescription)]
        [Category(MicrosoftSPGSiteCreationCategory)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string BusinessEvent
        {
            get
            {
                return ((string)(base.GetValue(CreateSubSiteActivity.BusinessEventProperty)));
            }
            set
            {
                base.SetValue(CreateSubSiteActivity.BusinessEventProperty, value);
            }
        }


        /// <summary>
        /// Dependency property for the Business Event ID
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static DependencyProperty BusinessEventIdProperty = DependencyProperty.Register("BusinessEventId", typeof(string), typeof(CreateSubSiteActivity));

        /// <summary>
        /// The ID of the business event that has occurred
        /// </summary>
        [Description(BusinessEventIdDescription)]
        [Category(MicrosoftSPGSiteCreationCategory)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string BusinessEventId
        {
            get
            {
                return ((string)(base.GetValue(CreateSubSiteActivity.BusinessEventIdProperty)));
            }
            set
            {
                base.SetValue(CreateSubSiteActivity.BusinessEventIdProperty, value);
            }
        }

        /// <summary>
        /// Dependency property for the SubSiteUrl
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static DependencyProperty SubSiteUrlProperty = DependencyProperty.Register("SubSiteUrl", typeof(string), typeof(CreateSubSiteActivity));

        /// <summary>
        /// The URL of the site that's created by this activity. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        [Description(SubSiteUrlDescription)]
        [Category(MicrosoftSPGSiteCreationCategory)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SubSiteUrl
        {
            get
            {
                return ((string)(base.GetValue(CreateSubSiteActivity.SubSiteUrlProperty)));
            }
            set
            {
                base.SetValue(CreateSubSiteActivity.SubSiteUrlProperty, value);
            }
        }

        /// <summary>
        /// Dependency property fo the Business Event ID key property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static DependencyProperty BusinessEventIdKeyProperty = DependencyProperty.Register("BusinessEventIdKey", typeof(string), typeof(CreateSubSiteActivity));

        /// <summary>
        /// This key stores the business Event ID in the PropertyBag of the SPWeb that's created by this workflow. Use this key
        /// to read or write the Business Event ID in the PropertyBag of the SPWeb. 
        /// </summary>
        [Description(BusinessEventIdKeyDescription)]
        [Category(MicrosoftSPGSiteCreationCategory)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string BusinessEventIdKey
        {
            get
            {
                return ((string)(base.GetValue(CreateSubSiteActivity.BusinessEventIdKeyProperty)));
            }
            set
            {
                base.SetValue(CreateSubSiteActivity.BusinessEventIdKeyProperty, value);
            }
        }

        #endregion

        /// <summary>
        /// Called by the workflow runtime to execute an activity. This will create the actual subsite, based on the information that's set as
        /// the parameters. 
        /// </summary>
        /// <param name="executionContext">The <see cref="T:System.Workflow.ComponentModel.ActivityExecutionContext"/> to associate with this <see cref="T:System.Workflow.ComponentModel.Activity"/> and execution.</param>
        /// <returns>
        /// The <see cref="T:System.Workflow.ComponentModel.ActivityExecutionStatus"/> of the run task, which determines whether the activity remains in the executing state, or transitions to the closed state.
        /// </returns>
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            if (string.IsNullOrEmpty(this.SiteTemplateName)
                || string.IsNullOrEmpty(this.SiteCollectionUrl))
            {
                throw new SubSiteCreationException(Resources.NullOrEmptyDependencyProperties);
            }
            
            using (SPSite site = new SPSite(this.SiteCollectionUrl))
            {
                string subSiteUrl = Guid.NewGuid().ToString();

                if (string.IsNullOrEmpty(this.TopLevelSiteRelativeUrl))
                {
                    CreateNewSubSite(subSiteUrl, site.RootWeb);
                }
                else
                {
                    SPWeb topLevelWeb = null;

                    using (topLevelWeb = site.OpenWeb(this.TopLevelSiteRelativeUrl))
                    {
                        bool exists = topLevelWeb.Exists;

                        if (!exists)
                        {
                            using (SPWeb newTopLevelWeb = site.RootWeb.Webs.Add(this.TopLevelSiteRelativeUrl,
                                                                this.BusinessEvent + " Sites",
                                                                Resources.NewSiteDescription,
                                                                (uint)System.Threading.Thread.CurrentThread.CurrentUICulture.LCID,
                                                                DefaultSiteTemplate,
                                                                false,
                                                                false))
                            {
                                CreateNewSubSite(subSiteUrl, newTopLevelWeb);
                            }
                        }
                        else
                        {
                            CreateNewSubSite(subSiteUrl, topLevelWeb);
                        }
                    }
                }
            }

            return ActivityExecutionStatus.Closed;
        }

        private void CreateNewSubSite(string subSiteUrl, SPWeb topLevelWeb)
        {
             using (SPWeb newWeb = topLevelWeb.Webs.Add(subSiteUrl,
                                                this.BusinessEvent + ": " + this.BusinessEventId,
                                                Resources.NewSiteDescription,
                                                (uint)System.Threading.Thread.CurrentThread.CurrentUICulture.LCID,
                                                this.SiteTemplateName,
                                                false,
                                                false))
            {
                this.SetIdentifier(newWeb);
            }
        }

        /// <summary>
        /// Called when an exception is raised within the context of the execution of this instance.
        /// </summary>
        /// <param name="executionContext">The <see cref="T:System.Workflow.ComponentModel.ActivityExecutionContext"/> for this instance.</param>
        /// <param name="exception">The <see cref="T:System.Exception"/> that caused this fault.</param>
        /// <returns>
        /// The <see cref="T:System.Workflow.ComponentModel.ActivityExecutionStatus"/> resulting from an attempt to cancel this instance.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="executionContext"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="exception"/> is a null reference (Nothing).</exception>
        protected override ActivityExecutionStatus HandleFault(ActivityExecutionContext executionContext, Exception exception)
        {
            SubSiteCreationException applicationException = new SubSiteCreationException(Resources.ErrorInCreateSubSiteActivity, exception);
            return base.HandleFault(executionContext, applicationException);
        }

        private void SetIdentifier(SPWeb web)
        {
            web.Properties[BusienssEventPopertyIndexKey] = this.BusinessEvent;
            web.Properties[this.BusinessEventIdKey] = this.BusinessEventId;
            web.Properties.Update();

            this.SubSiteUrl = web.Url;
        }
    }
}
