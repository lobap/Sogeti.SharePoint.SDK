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
using System.ComponentModel;
using Microsoft.SharePoint.Workflow;

namespace ExecutionModels.Workflow.FullTrust.Activities
{
    public class CreateSubSiteActivity : SiteCreationBaseActivity
    {
        public static DependencyProperty UseUniquePermissionsProperty = DependencyProperty.Register("UseUniquePermissions", typeof(bool), typeof(CreateSubSiteActivity));
        
        [Description("This property use unique permissions.")]
        [Browsable(true)]
        [Category("Patterns and Practices")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool UseUniquePermissions
        {
            get { return ((bool)base.GetValue(UseUniquePermissionsProperty)); }
            set { base.SetValue(UseUniquePermissionsProperty, value); }
        }

        public static DependencyProperty ConvertIfExistsProperty = DependencyProperty.Register("ConvertIfExists", typeof(bool), typeof(CreateSubSiteActivity));

        [Description("This will convert if exists.")]
        [Browsable(true)]
        [Category("Patterns and Practices")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ConvertIfExists
        {
            get { return ((bool)base.GetValue(ConvertIfExistsProperty)); }
            set { base.SetValue(ConvertIfExistsProperty, value); }
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            try
            {
                ActivityExecutionStatus result = base.Execute(executionContext);

                // Pick off the last segment of the URL
                string existingWebUrl;
                string newWebUrl;
                GetSiteAndWeb(SiteUrl, out existingWebUrl, out newWebUrl);

                using (SPSite site = new SPSite(existingWebUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        using (SPWeb newWeb = web.Webs.Add(newWebUrl, SiteTitle, SiteDescription, (uint)LocaleID, SiteTemplateId, UseUniquePermissions, ConvertIfExists))
                        {
                            // Make sure the new web is disposed if created
                        }
                    }
                }
                return (result);
            }
            catch (Exception e)
            {
                ISharePointService service = executionContext.GetService<ISharePointService>();
                if (service != null)
                {
                    service.LogToHistoryList(executionContext.ContextGuid, SPWorkflowHistoryEventType.WorkflowError, 0, TimeSpan.Zero, "Exception", "An exception occured in the Create Sub Site activity", e.ToString());
                }
                throw;
            }

        }

        protected static void GetSiteAndWeb(string fullUrl, out string existingWebUrl, out string newWebUrl)
        {
            int lastslash = fullUrl.LastIndexOf('/', fullUrl.Length - 2); // skip the last character for slash detection

            if (lastslash == -1) throw new ArgumentException("fullUrl not valid, no slash");

            existingWebUrl = fullUrl.Substring(0, lastslash + 1);
            newWebUrl = fullUrl.Substring(lastslash + 1);
        }
    }
}
