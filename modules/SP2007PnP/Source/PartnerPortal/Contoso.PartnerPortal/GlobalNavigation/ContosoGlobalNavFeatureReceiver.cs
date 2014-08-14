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
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Practices.SPG.Common;
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Contoso.PartnerPortal.Properties;
using Microsoft.SharePoint.Security;
using Constants = Contoso.Common.Constants;

namespace Contoso.PartnerPortal.GlobalNavigation
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Nav"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Contoso"), CLSCompliant(false)]
    [Guid("2f9a50ad-6165-4a3d-8b37-cb2666a62dbd")]
    public class ContosoGlobalNavFeatureReceiver : SPFeatureReceiver
    {
        #region Private Constants

        private const string HierarchicalConfigSiteMapName = "add[@name=\"HierarchicalConfigSiteMapProvider\"]";
        private const string HierachicalConfigSiteMapXpath = "configuration/system.web/siteMap/providers";

        #endregion

        private List<SPWebConfigModification> modifications;
        private string owner;
        private IConfigManager configManager;

        /// <summary>
        /// Initializes a new instance of the ContosoGlobalNavFeatureReceiver class.
        /// </summary>
        public ContosoGlobalNavFeatureReceiver()
        {
            modifications = new List<SPWebConfigModification>();
            owner = GetType().Assembly.GetName().ToString();

            SPWebConfigModificationHelper.CreateAndAddWebConfigModification(modifications,
                HierarchicalConfigSiteMapName,
                HierachicalConfigSiteMapXpath,
                SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode,
                Resources.WebConfigModForNavProvider,
                owner);

            configManager = SharePointServiceLocator.Current.GetInstance<IConfigManager>();
        }

        /// <summary>
        /// Occurs after a Feature is activated. 
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPFeatureReceiverProperties object that represents properties of the event handler.
        /// </param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = (SPWebApplication)properties.Feature.Parent;

            foreach (SPWebConfigModification modification in modifications)
            {
                webApp.WebConfigModifications.Add(modification);
            }

            webApp.Update();
            webApp.WebService.ApplyWebConfigModifications();

            // Provide a default sitemap in the configuration. This value could later be changed by using a PropertyBag editor.
            // Note, the IConfigManager interface is used to write config settings to a particular location. The IHierarchicalConfig interface
            // is used to read config settings in a hierarchical way. 
            configManager.SetInPropertyBag(Constants.SiteMapXmlConfigKey, Resources.DefaultSiteMapXml, webApp);
        }

        /// <summary>
        /// Occurs when a Feature is deactivating.
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPFeatureReceiverProperties object that represents properties of the event handler.
        /// </param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = (SPWebApplication)properties.Feature.Parent;

            SPWebConfigModificationHelper.CleanUpWebConfigModifications(webApp, owner);

            webApp.Update();
            webApp.WebService.ApplyWebConfigModifications();
        }


        /// <summary>
        /// Occurs after a Feature is installed.
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPFeatureReceiverProperties object that represents properties of the event handler.
        /// </param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {

        }

        /// <summary>
        /// Occurs when a Feature is uninstalling.
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPFeatureReceiverProperties object that represents properties of the event handler.
        /// </param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {

        }
    }
}