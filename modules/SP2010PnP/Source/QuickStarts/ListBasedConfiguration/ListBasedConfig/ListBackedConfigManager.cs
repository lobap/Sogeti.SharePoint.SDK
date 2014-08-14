//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.SharePoint;
using System;

namespace ListBasedConfig
{
    /// <summary>
    /// Overrides the default behavior to use list backed property bag hierarchy rather than the 
    /// default property bag hierarchy.
    /// </summary>
    public class ListBackedConfigManager: ConfigManager
    {
        /// <summary>
        /// Default constructor for list backed config manager.
        /// </summary>
        public ListBackedConfigManager():
            base()
        {
            
        }

        /// <summary>
        /// Constructor for injecting the web to use as basis for property bags.  In htis case
        /// the web is used for location of the list contain web and site level configuration.
        /// </summary>
        /// <param name="web">The web to use as the base for the property bags</param>
        public ListBackedConfigManager(SPWeb web) :
            base(web)
        {

        }

        /// <summary>
        /// Gets the hierarchy to use in retrieving and storing configuration values.
        /// </summary>
        /// <returns>The hierarchy to use, based upon config settings stored in lists</returns>
        protected override IPropertyBagHierarchy GetHierarchy()
        {
             return new ListBackedConfigHierarachy(base.WebContext, CentralSiteConfig.GetCentralConfigSiteUrl());
        }
    }
}
