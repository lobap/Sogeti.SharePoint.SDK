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
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
using Microsoft.SharePoint.Administration;
using System.ComponentModel;

namespace ExecutionModels.Workflow.FullTrust.Activities
{
    public class SiteExistsActivity : Activity
    {
        /// <summary>
        /// Returns whether the site exists or not -- signature to match SharePoint Designer Requirement
        /// </summary>
        /// <param name="workflowContext">Environment for activity (unused)</param>
        /// <param name="listId">ID of the list the workflow is running on (unused)</param>
        /// <param name="itemId">Item ID of the item the workflow is running on (unused)</param>
        /// <param name="siteUrl">The site to determine whether exists or not</param>
        /// <returns>True if site exists, false if not </returns>
        public static bool DoesSiteExistCondition(WorkflowContext workflowContext, string listId, int itemId, string siteUrl)
        {
            string exception;
            return (DoesSiteExist(siteUrl, out exception));
        }

        public static bool DoesSiteExist(string siteUrl, out string exception)
        {
            SPSite site = null;
            SPWeb web = null;
            try
            {
                site = new SPSite(siteUrl);
                web = site.OpenWeb();
                exception = null;
                if (string.Compare(web.Url, siteUrl, true) == 0)
                {
                    return (web.Exists);
                }
                else
                {
                    return (false);
                }
            }
            catch (Exception e)
            {
                exception = e.ToString();
                return(false);
            }
            finally
            {
                if (web != null) { web.Dispose(); web = null; }
                if (site != null) { site.Dispose(); site = null; }
            }

        }

        public static DependencyProperty SiteUrlProperty = DependencyProperty.Register("SiteUrl", typeof(string), typeof(SiteExistsActivity));

        [Description("The absolute URL of the site or site collection to create")]
        [Browsable(true)]
        [Category("Patterns and Practices")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string SiteUrl
        {
            get { return ((string)base.GetValue(SiteUrlProperty)); }
            set { base.SetValue(SiteUrlProperty, value); }
        }

        public static DependencyProperty ExistsProperty = DependencyProperty.Register("Exists", typeof(bool), typeof(SiteExistsActivity));
        [Description("The result of the operation indicating whether the site exists or not")]
        [Browsable(true)]
        [Category("Patterns and Practices")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Exists
        {
            get { return ((bool)base.GetValue(ExistsProperty)); }
            set { base.SetValue(ExistsProperty, value); }
        }

        public static DependencyProperty ExceptionProperty = DependencyProperty.Register("Exception", typeof(string), typeof(SiteExistsActivity));
        [Description("The exception generated while testing for the existance of the site")]
        [Browsable(true)]
        [Category("Patterns and Practices")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Exception
        {
            get { return ((string)base.GetValue(ExceptionProperty)); }
            set { base.SetValue(ExceptionProperty, value); }
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            string exception;
            Exists = DoesSiteExist(SiteUrl, out exception);
            Exception = exception;

            return base.Execute(executionContext);
        }

    }
}
