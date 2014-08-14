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
using Microsoft.SharePoint.Administration;
using System;
using Microsoft.Practices.SharePoint.Common;

namespace ListBasedConfig
{
    /// <summary>
    /// Creates an hierachical config that works with lists
    /// </summary>
    public class ListBackedHierarchicalConfig:HierarchicalConfig
    {
        /// <summary>
        /// default constructor required for creating through service location
        /// </summary>
        public ListBackedHierarchicalConfig() : base()
        {
        }

        /// <summary>
        /// Creates a configuration injecting the web to use to derive the hierarchy
        /// </summary>
        /// <param name="web"></param>
        public ListBackedHierarchicalConfig(SPWeb web)
            : base(web)
        {

        }

        /// <summary>
        /// Gets the hierarchy of property bags, returns a set of list based property bags.  Uses the 
        /// web in the current context for the root of the hierarchy.
        /// </summary>
        /// <returns>The hierarchy to use for configuration</returns>
        protected override IPropertyBagHierarchy GetHierarchy()
        {
            return new ListBackedConfigHierarachy(base.WebContext, CentralSiteConfig.GetCentralConfigSiteUrl());
        }
    }
}
