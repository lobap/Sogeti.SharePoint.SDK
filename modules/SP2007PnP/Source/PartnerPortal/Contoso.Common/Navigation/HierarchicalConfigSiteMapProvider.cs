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
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Xml;
using Microsoft.Practices.SPG.Common;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace Contoso.Common.Navigation
{
    /// <summary>
    /// SiteMapProvider that reads the XML of the sitemap from the hierarchical config.
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    public class HierarchicalConfigSiteMapProvider : StaticSiteMapProvider
    {
        private static readonly object syncRoot = new object();
        private SiteMapNode rootNode;
        private ILogger logger;
        // Some basic state to help track the initialization state of the provider.
        private bool initialized;


        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalConfigSiteMapProvider"/> class.
        /// </summary>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public HierarchicalConfigSiteMapProvider()
        {
            logger = SharePointServiceLocator.Current.GetInstance<ILogger>();
        }

        /// <summary>
        /// Return the root node of the current site map.
        /// </summary>
        public override SiteMapNode RootNode
        {
            get
            {
                return BuildSiteMap();
            }
        }


        /// <summary>
        /// Retrieves the root node of all the nodes that are currently managed by the current provider.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.SiteMapNode"/> that represents the root node of the set of nodes that the current provider manages.
        /// </returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected override SiteMapNode GetRootNodeCore()
        {
            return RootNode;
        }

        /// <summary>
        /// Loads the site map information from persistent storage and builds it in memory.
        /// </summary>
        /// <returns>
        /// The root <see cref="T:System.Web.SiteMapNode"/> of the site map navigation structure.
        /// </returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override SiteMapNode BuildSiteMap()
        {
            if (!initialized)
            {
                lock (syncRoot)
                {
                    if (!initialized)
                    {
                        rootNode = BuildSiteMapSynchronized();

                        if (rootNode != null)
                        {
                            initialized = true;
                        }

                    }
                }
            }

            return rootNode;

        }

        private SiteMapNode BuildSiteMapSynchronized()
        {
            SiteMapNode siteMapRootNode = null;
            try
            {
                // Get the XML that holds all the sitemapnodes from the config store. The default value for this is set by the
                // ContosoGlobalNavFeatureReceiver. Because the SiteMapProvider is instantiated only once per WebApplication, 
                // the configsetting is also only read from the Current SPWebApplication or above. 
                // Note, The IHierarchicalConfig interface is used to read config settings in a hierarchical way. The IConfigManager 
                // interface is used to write config settings to a particular location. 
                IHierarchicalConfig hierarchicalConfig = SharePointServiceLocator.Current.GetInstance<IHierarchicalConfig>();
                string siteMapXml =
                    hierarchicalConfig.GetByKey<string>(Constants.SiteMapXmlConfigKey, ConfigLevel.CurrentSPWebApplication);

                if (string.IsNullOrEmpty(siteMapXml))
                {
                    logger.LogToOperations("SiteMap Xml value retrieved from configuration ws null or empty.");
                    return null;
                }

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(siteMapXml);

                XmlNode rootXmlNode = xmlDocument.SelectSingleNode("/siteMap/siteMapNode");
                if (rootXmlNode == null)
                {
                    logger.LogToOperations("Can not find siteMapNode in navigation xml.");
                    return null;
                }

                siteMapRootNode = BuildSiteMapTree(rootXmlNode);
            }
            catch (Exception ex)
            {
                logger.LogToOperations(ex, "Could not build up the sitemap from setting in HierarchicalConfig.", 0, EventLogEntryType.Error, null);
                throw;
            }

            return siteMapRootNode;
        }

        private SiteMapNode BuildSiteMapTree(XmlNode xmlNode)
        {
            SiteMapNode siteMapNode = new SiteMapNode(this, xmlNode.Attributes["url"].Value, xmlNode.Attributes["url"].Value, xmlNode.Attributes["title"].Value);
            if (xmlNode.HasChildNodes)
            {
                XmlNode currentNode = xmlNode.FirstChild;
                // Note. You must use the AddNode method, not the siteMapNode.Children.Add() method, because that method will not work and cause
                // a stackoverflow exception. 
                AddNode(BuildSiteMapTree(xmlNode.FirstChild), siteMapNode);

                currentNode = currentNode.NextSibling;
                while (currentNode != null)
                {
                    // Note. You must use the AddNode method, not the siteMapNode.Children.Add() method, because that method will not work and cause
                    // a stackoverflow exception. 
                    AddNode(BuildSiteMapTree(currentNode), siteMapNode);
                    currentNode = currentNode.NextSibling;
                }
            }
            return siteMapNode;
        }
    }
}