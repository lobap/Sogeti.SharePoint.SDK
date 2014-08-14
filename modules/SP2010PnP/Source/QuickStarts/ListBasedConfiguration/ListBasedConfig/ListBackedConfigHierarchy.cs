//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace ListBasedConfig
{
    /// <summary>
    /// Gets a configuration hierarchy based upon a list.
    /// </summary>
    public class ListBackedConfigHierarachy: PropertyBagHierarchy
    {
        /// <summary>
        /// Constructs a list based configuration hierarchy, using the web and url provided.
        /// </summary>
        /// <param name="currentWeb">The web to use for web and site settings.  Settings are stored on the root web of the site</param>
        /// <param name="centralSiteUrl">The url to the site whose root web contains a list for settings for farm and web app</param>
        public ListBackedConfigHierarachy(SPWeb currentWeb, string centralSiteUrl)
        {
            if(currentWeb == null)
                throw new ArgumentNullException("currentWeb");

            base.Bags.Add(new ListBackedPropertyBag(currentWeb.Site, currentWeb.ID.ToString(), ConfigLevel.CurrentSPWeb));
            base.Bags.Add(new ListBackedPropertyBag(currentWeb.Site, currentWeb.Site.ID.ToString(), ConfigLevel.CurrentSPSite));

            if (centralSiteUrl != null)
            {
                base.Bags.Add(new ListBackedUrlPropertyBag(centralSiteUrl, currentWeb.Site.WebApplication.Id.ToString(),
                                                        ConfigLevel.CurrentSPWebApplication));
                base.Bags.Add(new ListBackedUrlPropertyBag(centralSiteUrl, SPFarm.Local.Id.ToString(), ConfigLevel.CurrentSPFarm));
            }
        }
    }
}
