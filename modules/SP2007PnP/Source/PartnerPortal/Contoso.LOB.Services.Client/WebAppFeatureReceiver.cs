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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Contoso.Common.Repositories;
using Contoso.LOB.Services.Client.Properties;
using Contoso.LOB.Services.Client.Repositories;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;
using System.Web;

namespace Contoso.LOB.Services.Client
{
    /// <summary>
    /// The feature receiver that installs the lob services into the system. 
    /// </summary>
    [CLSCompliant(false)]
    [Guid("8b0f085e-72a0-4d9f-ac74-0038dc0f6dd5")]
    public class UpdateServiceModelFeatureReceiver : SPFeatureReceiver
    {

        private SPFeatureDefinition featureDefinition;
        /// <summary>
        /// Initializes a new instance of the UpdateServiceModelFeatureReceiver class.
        /// </summary>
        public UpdateServiceModelFeatureReceiver()
        {
        }

        private string GetOwner()
        {
            return GetType().Assembly.GetName().ToString();
        }

        private string GetWcfServiceConfiguration()
        {
            string wcfConfig;
            
            // The WcfServiceConfiguration.xml file is deployed as part of the feature, and it is available in the feature folder.
            using (StreamReader reader = new StreamReader(featureDefinition.RootDirectory + "\\WcfServiceConfiguration.xml"))
            {
                wcfConfig = reader.ReadToEnd();
            }
			wcfConfig = wcfConfig.Replace("ServiceHostComputerName", System.Environment.MachineName);
            return wcfConfig;
        }

        private SPWebConfigModification GetModification()
        {
            return new SPWebConfigModification("system.serviceModel", "configuration")
            {
                Owner = GetOwner(),
                Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode,
                Value = GetWcfServiceConfiguration()
            };
        }

        /// <summary>
        /// Occurs after a Feature is activated. 
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPFeatureReceiverProperties object that represents properties of the event handler.
        /// </param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel=true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            featureDefinition = properties.Feature.Definition;

            SPWebApplication webApp = (SPWebApplication)properties.Feature.Parent;

            webApp.WebConfigModifications.Add(GetModification());

            webApp.Update();
            webApp.WebService.ApplyWebConfigModifications();

            ServiceLocatorConfig typeMappings = new ServiceLocatorConfig();
            typeMappings.RegisterTypeMapping<IIncidentManagementRepository, IncidentManagementRepository>();
            typeMappings.RegisterTypeMapping<IPricingRepository, PricingRepository>();
            typeMappings.RegisterTypeMapping<IProductCatalogRepository, CachedBdcProductCatalogRepository>();
        }
		
        /// <summary>
        /// Occurs when a Feature is deactivating.
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPFeatureReceiverProperties object that represents properties of the event handler.
        /// </param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = (SPWebApplication)properties.Feature.Parent;

            SPWebConfigModification modification = GetModification();

            // Try to remove the specific web.config modification
            if (webApp.WebConfigModifications.Contains(modification))
            {
                webApp.WebConfigModifications.Remove(modification);
            }

            // Also cleanup any orphaned web.config modifications that have been made by this feature receiver. 
            foreach(var mod in webApp.WebConfigModifications.ToArray())
            {
                if (mod.Owner == GetOwner())
                {
                    webApp.WebConfigModifications.Remove(mod);
                }
            }

            webApp.Update();
            webApp.WebService.ApplyWebConfigModifications();
        }
		
        /// <summary>
        /// Occurs after a Feature is installed.
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPFeatureReceiverProperties object that represents properties of the event handler.
        /// </param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
            ServiceLocatorConfig typeMappings = new ServiceLocatorConfig();
            typeMappings.RegisterTypeMapping<IIncidentManagementRepository, IncidentManagementRepository>();
            typeMappings.RegisterTypeMapping<IPricingRepository, PricingRepository>();
            typeMappings.RegisterTypeMapping<IProductCatalogRepository, CachedBdcProductCatalogRepository>();
        }
		
        /// <summary>
        /// Occurs when a Feature is uninstalling.
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPFeatureReceiverProperties object that represents properties of the event handler.
        /// </param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {

        }
    }
}