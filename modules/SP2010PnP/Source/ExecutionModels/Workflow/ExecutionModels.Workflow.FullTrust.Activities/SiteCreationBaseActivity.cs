//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.ComponentModel;

namespace ExecutionModels.Workflow.FullTrust.Activities
{
    public abstract class SiteCreationBaseActivity : Activity
    {
        public static DependencyProperty SiteUrlProperty = DependencyProperty.Register("SiteUrl", typeof(string), typeof(SiteCreationBaseActivity));

        [Description("The absolute URL of the site or site collection to create")]
        [Browsable(true)]
        [Category("Patterns and Practices")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string SiteUrl
        {
            get { return ((string)base.GetValue(SiteUrlProperty)); }
            set { base.SetValue(SiteUrlProperty, value); }
        }

        public static DependencyProperty SiteTemplateIdProperty = DependencyProperty.Register("SiteTemplateId", typeof(string), typeof(SiteCreationBaseActivity));

        [Description("Site Template Id.")]
        [Browsable(true)]
        [Category("Patterns and Practices")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string SiteTemplateId
        {
            get { return ((string)base.GetValue(SiteTemplateIdProperty)); }
            set { base.SetValue(SiteTemplateIdProperty, value); }
        }

        public static DependencyProperty SiteTitleProperty = DependencyProperty.Register("SiteTitle", typeof(string), typeof(SiteCreationBaseActivity));

        [Description("Site Title")]
        [Browsable(true)]
        [Category("Patterns and Practices")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string SiteTitle
        {
            get { return ((string)base.GetValue(SiteTitleProperty)); }
            set { base.SetValue(SiteTitleProperty, value); }
        }

        public static DependencyProperty SiteDescriptionProperty = DependencyProperty.Register("SiteDescription", typeof(string), typeof(SiteCreationBaseActivity));

        [Description("Site Description")]
        [Browsable(true)]
        [Category("Patterns and Practices")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string SiteDescription
        {
            get { return ((string)base.GetValue(SiteDescriptionProperty)); }
            set { base.SetValue(SiteDescriptionProperty, value); }
        }

        public static DependencyProperty LocaleIDProperty = DependencyProperty.Register("LocaleID", typeof(int), typeof(SiteCreationBaseActivity));

        [Description("Locale ID")]
        [Browsable(true)]
        [Category("Patterns and Practices")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int LocaleID
        {
            get { return ((int)base.GetValue(LocaleIDProperty)); }
            set { base.SetValue(LocaleIDProperty, value); }
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            return base.Execute(executionContext);
        }

        public SPWebApplication GetWebApplicationForUrl(string url)
        {
            Uri uri = new Uri(url);
            SPWebApplication app = SPWebApplication.Lookup(uri);
            return (app);
        }
    }
}
