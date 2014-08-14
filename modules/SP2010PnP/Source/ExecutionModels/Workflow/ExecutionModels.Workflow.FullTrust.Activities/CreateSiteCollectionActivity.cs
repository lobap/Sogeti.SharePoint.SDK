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
using System.Security.Permissions;
using System.Text;
using System.Workflow;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Workflow;

namespace ExecutionModels.Workflow.FullTrust.Activities
{
    
    public class CreateSiteCollectionActivity : Activity
    {
        public static DependencyProperty QuotaNameProperty = DependencyProperty.Register("QuotaName", typeof(string), typeof(CreateSiteCollectionActivity));
        public string QuotaName
        {
            get { return ((string)base.GetValue(QuotaNameProperty)); }
            set { base.SetValue(QuotaNameProperty, value); }
        }

        public static DependencyProperty OwnerNameProperty = DependencyProperty.Register("OwnerName", typeof(string), typeof(CreateSiteCollectionActivity));
        public string OwnerName
        {
            get { return ((string)base.GetValue(OwnerNameProperty)); }
            set { base.SetValue(OwnerNameProperty, value); }
        }

        public static DependencyProperty OwnerLogonProperty = DependencyProperty.Register("OwnerLogon", typeof(string), typeof(CreateSiteCollectionActivity));
        public string OwnerLogon
        {
            get { return ((string)base.GetValue(OwnerLogonProperty)); }
            set { base.SetValue(OwnerLogonProperty, value); }
        }

        public static DependencyProperty OwnerEmailProperty = DependencyProperty.Register("OwnerEmail", typeof(string), typeof(CreateSiteCollectionActivity));
        public string OwnerEmail
        {
            get { return ((string)base.GetValue(OwnerEmailProperty)); }
            set { base.SetValue(OwnerEmailProperty, value); }
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            ActivityExecutionStatus result = base.Execute(executionContext);

            try
            {
                // Get Web Application
                SPWebApplication webApp = null;//GetWebApplicationForUrl(SiteUrl);

                Uri siteUri = new Uri("SiteUrl");

                // Create Site
                using (SPSite newSite = webApp.Sites.Add(siteUri.PathAndQuery, "SiteTitle", "SiteDescription", (uint)123, "1234", OwnerLogon, OwnerName, OwnerEmail))
                {
                    // Set Quota
                    if (!string.IsNullOrEmpty(QuotaName))
                    {
                        newSite.Quota = SPWebService.ContentService.QuotaTemplates[QuotaName];
                    }
                }
            }
            catch (Exception e)
            {
                ISharePointService service = executionContext.GetService<ISharePointService>();
                if (service != null)
                {
                    service.LogToHistoryList(executionContext.ContextGuid, SPWorkflowHistoryEventType.WorkflowError, 0, TimeSpan.Zero, "Exception", "An exception occurred in the Create Site Collection activity", e.ToString());
                }
                throw;
            }

            return (result);
        }

    }
}
