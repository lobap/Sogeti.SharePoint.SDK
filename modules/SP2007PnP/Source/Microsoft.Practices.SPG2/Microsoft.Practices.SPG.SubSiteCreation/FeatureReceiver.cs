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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.Globalization;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;
using Microsoft.Practices.SPG.Common.Configuration;

namespace Microsoft.Practices.SPG.SubSiteCreation
{
    [Guid("d962e88b-1ce2-44f3-b30d-8d8f23dfc2e2")]
    [CLSCompliant(false)]
    public class FeatureReceiver : SPFeatureReceiver
    {
        #region Private consts and ReadOnly fields

        private const string taskListName = "Sub Site Creation Request Tasks";
        private const string workflowHistoryName = "Sub Site Creation Request Workflow History";
        private const string subSiteRequestsListName = "Sub Site Creation Requests";
        private const string workflowTemplateName = "Microsoft SPG Sub Site Creation Workflow";
        private readonly Guid workflowTemplateId = new Guid("3B15AE78-175B-438f-B14C-53CC9A054252");

        #endregion

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel=true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;
            
            IConfigManager configManager = ServiceLocator.Current.GetInstance<IConfigManager>();
            configManager.SetByKey(Constants.SubSiteCreationConfigSiteKey, web.Url, SPFarm.Local);

            SPList taskList = web.Lists[taskListName];
            SPList historyList = web.Lists[workflowHistoryName];
            SPList subSiteCreationRequestList = web.Lists[subSiteRequestsListName];

            var existingAssociation = subSiteCreationRequestList.WorkflowAssociations.GetAssociationByName(workflowTemplateName, CultureInfo.CurrentCulture);
            if (existingAssociation == null)
            {
                SPWorkflowManager workflowManager = web.Site.WorkflowManager;
                SPWorkflowTemplateCollection templates = workflowManager.GetWorkflowTemplatesByCategory(web, null);
                SPWorkflowTemplate template = templates.GetTemplateByBaseID(workflowTemplateId);
                SPWorkflowAssociation association = SPWorkflowAssociation.CreateListAssociation(template, template.Name, taskList, historyList);
                association.AllowManual = true;
                association.AutoStartCreate = true;
                subSiteCreationRequestList.AddWorkflowAssociation(association);
                subSiteCreationRequestList.Update();
                association.Enabled = true;
            }

            ServiceLocatorConfig typeMappings = new ServiceLocatorConfig();
            typeMappings.RegisterTypeMapping<IBusinessEventTypeConfigurationRepository, BusinessEventTypeConfigurationRepository>();
            typeMappings.RegisterTypeMapping<ISubSiteCreationRequestRepository, SubSiteCreationRequestsRepository>();
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
            ServiceLocatorConfig typeMappings = new ServiceLocatorConfig();
            typeMappings.RegisterTypeMapping<IBusinessEventTypeConfigurationRepository, BusinessEventTypeConfigurationRepository>();
            typeMappings.RegisterTypeMapping<ISubSiteCreationRequestRepository, SubSiteCreationRequestsRepository>();

        }

        #region Unused

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [CLSCompliant(false)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
        }

        #endregion
    }
}