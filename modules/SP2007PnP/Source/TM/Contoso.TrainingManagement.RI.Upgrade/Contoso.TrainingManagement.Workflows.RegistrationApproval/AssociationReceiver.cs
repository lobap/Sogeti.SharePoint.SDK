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
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Globalization;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Workflow;

using Contoso.TrainingManagement.Common.Constants;

namespace Contoso.TrainingManagement.Workflows.RegistrationApproval
{
    [Guid("eb964011-05be-4c54-be14-0c237143fde8")]
    public class AssociationReceiver : SPFeatureReceiver
    {
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            if (web != null)
            {
                SPList taskList = web.Lists[Lists.RegistrationApprovalTasks];
                SPList historyList = web.Lists[Lists.WorkflowHistory];
                SPList registrationList = web.Lists[Lists.Registrations];

                SPWorkflowAssociation existingAssociation = null;
                existingAssociation = registrationList.WorkflowAssociations.GetAssociationByName(WorkflowTemplates.RegistrationApprovalNameV2, CultureInfo.CurrentCulture);
                if (existingAssociation == null)
                {
                    //Create a worklow manager and associate the Course Registration Approval Workflow template
                    //to our Registrations list.
                    SPWorkflowManager workflowManager = web.Site.WorkflowManager;
                    SPWorkflowTemplateCollection templates = workflowManager.GetWorkflowTemplatesByCategory(web, null);
                    SPWorkflowTemplate template = templates.GetTemplateByBaseID(new Guid(WorkflowTemplates.RegistrationApprovalBaseIdV2));
                    SPWorkflowAssociation association = SPWorkflowAssociation.CreateListAssociation(template, template.Name, taskList, historyList);
                    association.AllowManual = true;
                    association.AutoStartCreate = true;
                    registrationList.AddWorkflowAssociation(association);
                    registrationList.Update();
                    association.Enabled = true;
                }
            }
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
        }
    }
}