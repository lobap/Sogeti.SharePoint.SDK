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
using System.Data;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Security;
using System.Runtime.InteropServices;
using System;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.Practices.SPG.Common.ServiceLocation;

namespace Contoso.PartnerPortal.Promotions
{
    [Guid("6fd12cb8-f910-4cf4-b7d0-6b74769483ae")]
    public class PromotionSiteFeatureReceiver : SPFeatureReceiver
    {
        #region Private Constants

        private const string ProductCatalogSiteFeatureId = "946de646-f75e-4800-9694-d4426c586612";
        private const string ErrorWasEncouteredWhileAddingTheContosoProductMessage = "An error was encoutered while adding the ContosoProductCatalogSite Feature. The feature may not be available or may already be activated.";

        #endregion

        /// <summary>
        /// Occurs after a Feature is activated.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"></see> object that represents the properties of the event.</param>
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite spSite = properties.Feature.Parent as SPSite;

            if (spSite != null)
            {
                // Activate the ContosoProductCatalogSite Feature. The Promotions site requires Web parts from ContosoProductCatalogSite Feature
                // The feature may already be acitvated, or it may not be installed. If those conditions occur then log a warning and move on.
                try
                {
                    spSite.Features.Add(new Guid(ProductCatalogSiteFeatureId));
                }
                catch (DuplicateNameException ex)
                {
                    ILogger logger = SharePointServiceLocator.Current.GetInstance<ILogger>();
                    logger.LogToOperations(ex, ErrorWasEncouteredWhileAddingTheContosoProductMessage);
                }
            }
        }

        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
        }

        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
        }

        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
        }
    }
}
