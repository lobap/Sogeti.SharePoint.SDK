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
using System.Workflow.Activities;

using Microsoft.SharePoint.Workflow;

namespace Contoso.TrainingManagement.Workflows.RegistrationApproval
{
	public sealed partial class Workflow : SequentialWorkflowActivity
    {
        #region Private Fields

        private bool managerApprovalTaskComplete;

        #endregion

        #region Public Fields

        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public Guid taskId = default(Guid);
        public SPWorkflowTaskProperties taskProperties = new SPWorkflowTaskProperties();   

        #endregion

        #region Contructor

        /// <summary>
        /// The default constructor
        /// </summary>
        public Workflow()
		{
			InitializeComponent();
        }

        #endregion

        #region Workflow Activity Methods

        private void CreateManagerApprovalTask_MethodInvoking(object sender, EventArgs e)
        {
            //assign new Guid to taskId
            taskId = Guid.NewGuid();

            Controller controller = new Controller();
            controller.PopulateManagerApprovalTaskProperties(taskProperties, workflowProperties.Web, workflowProperties.Item);
        }
        
        private void ManagerApprovalTaskCompleteCondition(object sender, ConditionalEventArgs e)
        {            
            e.Result = !managerApprovalTaskComplete;
        }

        private void OnRegistrationChanged_Invoked(object sender, ExternalDataEventArgs e)
        {
            managerApprovalTaskComplete = Controller.IsManagerApprovalTaskComplete(workflowProperties.Item);            
        }

        private void ManagerApprovedCondition(object sender, ConditionalEventArgs e)
        {
            e.Result = Controller.IsManagerApprovalTaskApproved(workflowProperties.Item);
        }

        private void CodeChargeAccountingActivity_ExecuteCode(object sender, EventArgs e)
        {
            Controller controller = new Controller();
            controller.ChargeAccounting(workflowProperties.Web, workflowProperties.Item);
        }

        #endregion
    }
}
