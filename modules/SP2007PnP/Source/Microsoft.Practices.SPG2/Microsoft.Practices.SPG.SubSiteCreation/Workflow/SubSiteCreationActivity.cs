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
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
//using Microsoft.Office.Workflow.Utility;

namespace Microsoft.Practices.SPG.SubSiteCreation.Workflow
{
    /// <summary>
    /// Workflow activity that creates subsites in the workflow. 
    /// </summary>
    public sealed partial class SubSiteCreationActivity : SequentialWorkflowActivity
    {
        private const string MicrosoftSPGSiteCreationCategory = "Microsoft SPG Site Creation.";
        private const string BusinessEventIndexKey = "BusinessEvent";
        private const string EventIdIndexKey = "EventId";
        private const string SiteCollectionUrlIndexKey = "SiteCollectionUrl";
        /// <summary>
        /// Initializes a new instance of the <see cref="SubSiteCreationActivity"/> class.
        /// </summary>
        public SubSiteCreationActivity()
        {
            InitializeComponent();
        }

        private Guid workflowId = default(System.Guid);

        /// <summary>
        /// The ID of the workflow. 
        /// </summary>
        public Guid WorkflowId
        {
            get
            {
                return workflowId;
            }
            set
            {
                workflowId = value;
            }
        }


        private SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

        /// <summary>
        /// The properties that are used by this workflow. 
        /// </summary>
        [CLSCompliant(false)]
        public SPWorkflowActivationProperties WorkflowProperties
        {
            get
            {
                return this.workflowProperties;
            }
            set
            {
                this.workflowProperties = value;
            }
        }

        private string businessEvent;

        private string eventId;

        private string siteCollectionUrl;

        /// <summary>
        /// The site collection that will hold the site to be created by this workflow. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "It's easier to work with strings in SharePoint.")]
        public string SiteCollectionUrl
        {
            get
            {
                return this.siteCollectionUrl;
            }
            set
            {
                this.siteCollectionUrl = value;
            }
        }

        /// <summary>
        /// The Id of the business event that has occurred
        /// </summary>
        public string EventId
        {
            get
            {
                return this.eventId;
            }
            set
            {
                this.eventId = value;
            }
        }

        /// <summary>
        /// The name of the type of business event that has occurred
        /// </summary>
        public string BusinessEvent
        {
            get
            {
                return this.businessEvent;
            }
            set
            {
                this.businessEvent = value;
            }
        }

        /// <summary>
        /// Dependency Property for <see cref="BusinessEventIdKey"/>
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static DependencyProperty BusinessEventIdKeyProperty = DependencyProperty.Register("BusinessEventIdKey", typeof(System.String), typeof(Microsoft.Practices.SPG.SubSiteCreation.Workflow.SubSiteCreationActivity));

        /// <summary>
        /// The name of the key to use to identify the business event.
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute(MicrosoftSPGSiteCreationCategory)]
        public String BusinessEventIdKey
        {
            get
            {
                return ((string)(base.GetValue(Microsoft.Practices.SPG.SubSiteCreation.Workflow.SubSiteCreationActivity.BusinessEventIdKeyProperty)));
            }
            set
            {
                base.SetValue(Microsoft.Practices.SPG.SubSiteCreation.Workflow.SubSiteCreationActivity.BusinessEventIdKeyProperty, value);
            }
        }

        /// <summary>
        /// Dependency Property for <see cref="SiteTemplateName"/>
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static DependencyProperty SiteTemplateNameProperty = DependencyProperty.Register("SiteTemplateName", typeof(System.String), typeof(Microsoft.Practices.SPG.SubSiteCreation.Workflow.SubSiteCreationActivity));

        /// <summary>
        /// The name of the site template to use during the subsite creation process.
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute(MicrosoftSPGSiteCreationCategory)]
        public String SiteTemplateName
        {
            get
            {
                return ((string)(base.GetValue(Microsoft.Practices.SPG.SubSiteCreation.Workflow.SubSiteCreationActivity.SiteTemplateNameProperty)));
            }
            set
            {
                base.SetValue(Microsoft.Practices.SPG.SubSiteCreation.Workflow.SubSiteCreationActivity.SiteTemplateNameProperty, value);
            }
        }

        /// <summary>
        /// Dependency Property for <see cref="TopLevelSiteRelativeUrl"/>
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static DependencyProperty TopLevelSiteRelativeUrlProperty = DependencyProperty.Register("TopLevelSiteRelativeUrl", typeof(System.String), typeof(Microsoft.Practices.SPG.SubSiteCreation.Workflow.SubSiteCreationActivity));

        /// <summary>
        /// The URL of the top level site for a business event.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute(MicrosoftSPGSiteCreationCategory)]
        public String TopLevelSiteRelativeUrl
        {
            get
            {
                return ((string)(base.GetValue(Microsoft.Practices.SPG.SubSiteCreation.Workflow.SubSiteCreationActivity.TopLevelSiteRelativeUrlProperty)));
            }
            set
            {
                base.SetValue(Microsoft.Practices.SPG.SubSiteCreation.Workflow.SubSiteCreationActivity.TopLevelSiteRelativeUrlProperty, value);
            }
        }

        /// <summary>
        /// Dependency Property for <see cref="SubSiteUrl"/>
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static DependencyProperty SubSiteUrlProperty = DependencyProperty.Register("SubSiteUrl", typeof(System.String), typeof(Microsoft.Practices.SPG.SubSiteCreation.Workflow.SubSiteCreationActivity));

        /// <summary>
        /// The URL of the subsite that has been created.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute(MicrosoftSPGSiteCreationCategory)]
        public String SubSiteUrl
        {
            get
            {
                return ((string)(base.GetValue(Microsoft.Practices.SPG.SubSiteCreation.Workflow.SubSiteCreationActivity.SubSiteUrlProperty)));
            }
            set
            {
                base.SetValue(Microsoft.Practices.SPG.SubSiteCreation.Workflow.SubSiteCreationActivity.SubSiteUrlProperty, value);
            }
        }

        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            this.businessEvent = (string)workflowProperties.Item[BusinessEventIndexKey];
            this.eventId = (string)workflowProperties.Item[EventIdIndexKey];
            this.siteCollectionUrl = (string)workflowProperties.Item[SiteCollectionUrlIndexKey];
        }
    }
}
