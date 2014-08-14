//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.Configuration;
using ListBasedConfig;
using Microsoft.SharePoint.WebPartPages;
using System.Collections.Generic;
using WebPart = System.Web.UI.WebControls.WebParts.WebPart;
using System.Xml;
using Microsoft.SharePoint.Navigation;

namespace ListBasedConfig.Features.Feature1
{
    /// <summary>
    /// Feature event receiver for installing list backed property bags.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("af8e7ab0-3319-48be-8a71-bcf2ed678c69")]
    public class Feature1EventReceiver : SPFeatureReceiver
    {
             public readonly string wpXml = "<webParts><webPart xmlns=\"http://schemas.microsoft.com/WebPart/v3\"><metaData><type name=\"" +
                typeof(ListBasedConfig.ListBackedConfigurationTests.ListBackedConfigurationTests).FullName + ", " +
                typeof(ListBasedConfig.ListBackedConfigurationTests.ListBackedConfigurationTests).Assembly.FullName +
                "\" /><importErrorMessage>$Resources:core,ImportErrorMessage;</importErrorMessage></metaData>" +
                "<data><properties><property name=\"Title\" type=\"string\">ListBackedConfigurationTests</property><property name=\"Description\" type=\"string\">My Visual WebPart</property>" +
                "</properties></data></webPart></webParts>";      
        /// <summary>
        /// Provisions the service locator settings, and ensures the list is created for storing configuration
        /// on the site collection root web.  The service locator configuration is defined for the site level.
        /// </summary>
        /// <param name="properties">The properties for this configuration</param>
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            string configListLocation = CentralSiteConfig.GetCentralConfigSiteUrl();

            // Configuration List Deployment
            using (var centralSite = new SPSite(configListLocation))
            {
                ConfigurationList.EnsureConfigurationList(centralSite);
            }

            var site = properties.Feature.Parent as SPSite;
            var slConfig = SharePointServiceLocator.GetCurrent(site).GetInstance<IServiceLocatorConfig>();
            slConfig.Site = site;
            slConfig.RegisterTypeMapping<IConfigManager, ListBackedConfigManager>();
            slConfig.RegisterTypeMapping<IHierarchicalConfig, ListBackedHierarchicalConfig>();
            SharePointServiceLocator.Reset();
            ConfigurationList.EnsureConfigurationList(site);

            // Add WP to new page and add to navigation


            AddWPToSiteNavigation(site.RootWeb, "listconfig.aspx", "List Configuration Test", wpXml, "left");

        }


        public static bool AddWPToSiteNavigation(SPWeb web, string url, string title, string wpXml, string wpZone)
        {
            SPFile newFile = null;

            newFile = web.GetFile(url);
            if (newFile != null && newFile.Exists)
            {   // Already exists
                return false;
            }

            newFile = CreatePageFromDefault(web, url);
            DeleteAllThenAddWebPartToPage(newFile, wpXml, wpZone);
            AddNavigationForPage(web, newFile, title);

            return true; // Page Created
        }

        public static SPFile CreatePageFromDefault(SPWeb web, string url)
        {
            SPFile defaultFile = web.GetFile("default.aspx");
            defaultFile.CopyTo(url);
            SPFile newFile = web.GetFile(url);
            return (newFile);
        }

        public static void DeleteAllThenAddWebPartToPage(SPFile page, string wpXml, string wpZone)
        {
            using (SPLimitedWebPartManager wpMgr = page.GetLimitedWebPartManager(System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared))
            {
                // Delete all web parts
                // Play the shell game because you can't delete from a collection in an enumerator
                List<WebPart> wpList = new List<WebPart>();
                foreach (WebPart wp in wpMgr.WebParts) wpList.Add(wp);
                foreach (WebPart wp in wpList) { wpMgr.DeleteWebPart(wp); }

                // Add our new web part
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(wpXml);
                using (XmlNodeReader reader = new XmlNodeReader(doc))
                {
                    string errorMessage = null;
                    System.Web.UI.WebControls.WebParts.WebPart wp = wpMgr.ImportWebPart(reader, out errorMessage);
                    wpMgr.AddWebPart(wp, wpZone, wp.ZoneIndex);
                }

            }
        }

        public static void AddNavigationForPage(SPWeb web, SPFile page, string linkName)
        {
            SPNavigationNodeCollection navItems = web.Navigation.QuickLaunch;
            SPNavigationNode newNode = new SPNavigationNode(linkName, page.Url, false);
            navItems.AddAsFirst(newNode);
            web.Update();
        }


        /// <summary>
        /// Removes the mappings for the configuration manager for list backed configuration.  This results
        /// in the default implementations being picked up for these services.
        /// </summary>
        /// <param name="properties">The properties related to the current context</param>
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            var slConfig = SharePointServiceLocator.GetCurrent().GetInstance<IServiceLocatorConfig>();
            var site = properties.Feature.Parent as SPSite;
            slConfig.Site = site;
            slConfig.RemoveTypeMapping<IConfigManager>(null);
            slConfig.RemoveTypeMapping<IHierarchicalConfig>(null);
            SharePointServiceLocator.Reset();
        }


        /// <summary>
        /// Provisions the centralized list for the configuration manager.  Web app and server
        /// settings need to be stored in the centralized list.  SetCentralConfigUrl stores the 
        /// location in config for this list that can subsequently be used by the property bags for
        /// web app and farm.
        /// </summary>
        /// <param name="properties">The context properties for feature installed</param>
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
            CentralSiteConfig.SetCentralConfigUrl("http://localhost/");
        }


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
