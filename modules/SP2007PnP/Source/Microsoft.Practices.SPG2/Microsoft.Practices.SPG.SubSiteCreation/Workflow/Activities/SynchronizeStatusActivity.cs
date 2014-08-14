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

namespace Microsoft.Practices.SPG.SubSiteCreation.Workflow.Activities
{
    /// <summary>
    /// Workflow activity that writes the status of the subsite creation request in the PropertyBag
    /// of the SPWeb that's created. 
    /// </summary>
    [Designer(typeof(SynchronizeStatusActivityDesigner))]
    [ToolboxBitmap(typeof(SynchronizeStatusActivity), "SynchronizeList.bmp")]
    public partial class SynchronizeStatusActivity : Activity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizeStatusActivity"/> class.
        /// </summary>
        public SynchronizeStatusActivity()
        {
            InitializeComponent();
        }

        #region Properties

        /// <summary>
        /// Dependency property for the TargetWebUrl
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static DependencyProperty TargetWebUrlProperty = DependencyProperty.Register("TargetWebUrl", typeof(string), typeof(SynchronizeStatusActivity));

        /// <summary>
        /// The URL that finds the SPWeb that's created by the workflow. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), Description("The URL of the target Web for business event.")]
        [Category("Microsoft SPG Site Creation.")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string TargetWebUrl
        {
            get
            {
                return ((string)(base.GetValue(SynchronizeStatusActivity.TargetWebUrlProperty)));
            }
            set
            {
                base.SetValue(SynchronizeStatusActivity.TargetWebUrlProperty, value);
            }
        }

        /// <summary>
        /// Dependency property for the Status field. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(SynchronizeStatusActivity));

        /// <summary>
        /// The status of the workflow, that will be stored in the SPWeb property bag. 
        /// </summary>
        [Description("The status of the business event")]
        [Category("Microsoft SPG Site Creation.")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Status
        {
            get
            {
                return ((string)(base.GetValue(SynchronizeStatusActivity.StatusProperty)));
            }
            set
            {
                base.SetValue(SynchronizeStatusActivity.StatusProperty, value);
            }
        }

        #endregion

        /// <summary>
        /// Called by the workflow runtime to execute an activity. This will update the status in the SPWeb PropertyBag. 
        /// </summary>
        /// <param name="executionContext">The <see cref="T:System.Workflow.ComponentModel.ActivityExecutionContext"/> to associate with this <see cref="T:System.Workflow.ComponentModel.Activity"/> and execution.</param>
        /// <returns>
        /// The <see cref="T:System.Workflow.ComponentModel.ActivityExecutionStatus"/> of the run task, which determines whether the activity remains in the executing state, or transitions to the closed state.
        /// </returns>
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            if (string.IsNullOrEmpty(this.TargetWebUrl))
            {
                throw new SubSiteCreationException("The target Web URL was not specified.");
            }

            using (SPSite site = new SPSite(this.TargetWebUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    web.Properties["Status"] = this.Status;
                    web.Properties.Update();
                }
            }

            return ActivityExecutionStatus.Closed;
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
            SubSiteCreationException applicationException
                = new SubSiteCreationException("An error occurred during the Synchronize Status activity", exception);

            return base.HandleFault(executionContext, applicationException);
        }
    }
}
