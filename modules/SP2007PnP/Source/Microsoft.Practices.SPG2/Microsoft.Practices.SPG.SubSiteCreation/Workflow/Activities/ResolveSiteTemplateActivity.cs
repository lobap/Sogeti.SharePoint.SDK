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
using System.Globalization;
using System.Workflow.ComponentModel;
using Microsoft.Practices.SPG.SubSiteCreation;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using System.Drawing;
using Microsoft.Practices.SPG.SubSiteCreation.Properties;

namespace Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities
{
    /// <summary>
    /// Workflow activity that's part of the Subsite creation workflow. This determines which 
    /// site template to use. 
    /// </summary>
	[Designer(typeof(ResolveSiteTemplateActivityDesigner))]
    [ToolboxBitmap(typeof(ResolveSiteTemplateActivity), "SearchInFolder.bmp")]
    public partial class ResolveSiteTemplateActivity: Activity
	{
        private const string BusinessEventNameDescription = "The name of the Business Event for resolving the site template.";
        private const string MicrosoftSubSiteCreationCategory = "Microsoft SPG Site Creation.";
        private const string SiteTemplateNameDescription = "The name of the site template to use when creating site";
        private const string BusinessEventIdKeyDescription = "Key for the business event identifier";
        /// <summary>
        /// Create an instance of the ResolveSiteTemplateActivity().
        /// </summary>
		public ResolveSiteTemplateActivity()
		{
			InitializeComponent();
        }

        #region Properties

        /// <summary>
        /// A Dependency Property that determines what the name of the Business Event is. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static DependencyProperty BusinessEventNameProperty = DependencyProperty.Register("BusinessEventName", typeof(string), typeof(ResolveSiteTemplateActivity));

        /// <summary>
        /// The name of the Business Event for resolving the site template.
        /// </summary>
        [Description(BusinessEventNameDescription)]
        [Category(MicrosoftSubSiteCreationCategory)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string BusinessEventName
        {
            get
            {
                return ((string)(base.GetValue(ResolveSiteTemplateActivity.BusinessEventNameProperty)));
            }
            set
            {
                base.SetValue(ResolveSiteTemplateActivity.BusinessEventNameProperty, value);
            }
        }

        /// <summary>
        /// The Dependendy Property for the name of the Business Event for resolving the site template.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static DependencyProperty SiteTemplateNameProperty = DependencyProperty.Register("SiteTemplateName", typeof(string), typeof(ResolveSiteTemplateActivity));

        /// <summary>
        /// The name of the Business Event for resolving the site template.
        /// </summary>
        [Description(SiteTemplateNameDescription)]
        [Category(MicrosoftSubSiteCreationCategory)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string SiteTemplateName
        {
            get
            {
                return ((string)(base.GetValue(ResolveSiteTemplateActivity.SiteTemplateNameProperty)));
            }
            set
            {
                base.SetValue(ResolveSiteTemplateActivity.SiteTemplateNameProperty, value);
            }
        }

        /// <summary>
        /// The Dependency Property for the Top Level Site Relative URL
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static DependencyProperty TopLevelSiteRelativeUrlProperty = DependencyProperty.Register("TopLevelSiteRelativeUrl", typeof(string), typeof(ResolveSiteTemplateActivity));

        /// <summary>
        /// Top Level Site Relative URL. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), Description("Relative URL of the top level site for business event subsites.")]
        [Category(MicrosoftSubSiteCreationCategory)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string TopLevelSiteRelativeUrl
        {
            get
            {
                return ((string)(base.GetValue(ResolveSiteTemplateActivity.TopLevelSiteRelativeUrlProperty)));
            }
            set
            {
                base.SetValue(ResolveSiteTemplateActivity.TopLevelSiteRelativeUrlProperty, value);
            }
        }

        /// <summary>
        /// The Dependency property for the key that identifies the type of business Event that has occurred. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static DependencyProperty BusinessEventIdKeyProperty = DependencyProperty.Register("BusinessEventIdKey", typeof(string), typeof(ResolveSiteTemplateActivity));
        
        /// <summary>
        /// The key that identifies the type of business event that has occurred. 
        /// </summary>
        [Description(BusinessEventIdKeyDescription)]
        [Category(MicrosoftSubSiteCreationCategory)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string BusinessEventIdKey
        {
            get
            {
                return ((string)(base.GetValue(ResolveSiteTemplateActivity.BusinessEventIdKeyProperty)));
            }
            set
            {
                base.SetValue(ResolveSiteTemplateActivity.BusinessEventIdKeyProperty, value);
            }
        }

        #endregion

        /// <summary>
        /// Executes the workflow activity. This will query the Business Event type configuration list through the 
        /// <see cref="IBusinessEventTypeConfigurationRepository"/>.
        /// </summary>
        /// <param name="executionContext">The excecution context.</param>
        /// <returns>Returns the status after the workflow activity has run. </returns>
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            IBusinessEventTypeConfigurationRepository configurationRepository
                = SharePointServiceLocator.Current.GetInstance<IBusinessEventTypeConfigurationRepository>();

            BusinessEventTypeConfiguration configuration = configurationRepository.GetBusinessEventTypeConfiguration(this.BusinessEventName);

            this.SiteTemplateName = configuration.SiteTemplate;
            this.TopLevelSiteRelativeUrl = configuration.TopLevelSiteRelativeUrl;
            this.BusinessEventIdKey = configuration.BusinessEventIdKey;

            return ActivityExecutionStatus.Closed;
        }

        /// <summary>
        /// Method that adds error information when an exception has occurred. 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected override ActivityExecutionStatus HandleFault(ActivityExecutionContext executionContext, Exception exception)
        {
            string errorMessage = string.Format(CultureInfo.CurrentCulture,
                                                Resources.ResolveSiteTemplateFaultMessage
                                                , exception.Message, this.BusinessEventName, this.SiteTemplateName, this.TopLevelSiteRelativeUrl, this.BusinessEventIdKey);

            SubSiteCreationException applicationException
                = new SubSiteCreationException(errorMessage, exception);

            return base.HandleFault(executionContext, applicationException);
        }
    }
}
