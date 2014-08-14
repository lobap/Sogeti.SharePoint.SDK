//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.IO;
using Microsoft.Practices.SharePoint.Common.Properties;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// Represents a property bag that is backed by a list, and constructed based upon a URL.
    /// For cases where the configuration list can't be identified by context - for example,
    /// a web application and a farm will need to rely upon a single centrally located list
    /// for their settings.  This class requires AllowUnsafeUpdates to be used, and therefore
    /// can not be used from the sandbox.  <see cref="ListBackedPropertyBag"/> to use within
    /// the sandbox.
    /// </summary>
    public class ListBackedUrlPropertyBag: IPropertyBag
    {
        private readonly ConfigLevel level;
        private readonly string contextId;
        
        Func<string, string> buildKeyFunc = null;

       
        private string siteUrl;

        /// <summary>
        /// The configuration level for this list.
        /// </summary>
        public ConfigLevel Level
        {
            get
            {
                return this.level;
            }
        }

        /// <summary>
        /// The context Id for this list.  This represents a unique ID for the given context.
        /// For example, use the Web.ID for a web, or a Site.ID for a site.  This is used in 
        /// combination with the key for lookup in the list to uniquely define the key setting 
        /// in a specific context.
        /// </summary>
        public string ContextId
        {
            get
            {
                return this.contextId;
            }
        }

        /// <summary>
        /// Constructs a property bag backed by a list using the site url to access the list.
        /// </summary>
        /// <param name="siteUrl">The site url where the configuration list is located</param>
        /// <param name="contextId">The unique ID for this context</param>
        /// <param name="level">The level of the list based property bag</param>
        public ListBackedUrlPropertyBag(string siteUrl, string contextId, ConfigLevel level)
        {
            this.level = level;
            this.contextId = contextId;
            this.siteUrl = siteUrl;
            buildKeyFunc = (key) => key;  //by default just return key.
        }

        /// <summary>
        /// Constructs a list based property bag backed by a list using the site url to access the list.
        /// </summary>
        /// <param name="siteUrl">The site url where the configuration list is located</param>
        /// <param name="contextId">The unique ID for this context</param>
        /// <param name="level">The level of the list based property bag</param>
        /// <param name="buildKeyFunc">the function to use for transforming the key value used for storage</param>
        public ListBackedUrlPropertyBag(string siteUrl, string contextId, ConfigLevel level, Func<string, string> buildKeyFunc)
        {
            this.level = level;
            this.contextId = contextId;
            this.siteUrl = siteUrl;
            this.buildKeyFunc = buildKeyFunc;
        }

        private SPSite CreateSite()
        {
            try
            {
                var site = new SPSite(this.siteUrl);
                return site;
            }
            catch(FileNotFoundException ex)
            {
                throw new ConfigurationException(string.Format(CultureInfo.CurrentCulture, Resources.BadSiteUrl, siteUrl), ex);
            }
        }

        /// <summary>
        /// Returns true if the property bag contains the key specified.
        /// </summary>
        /// <param name="key">The key to check for</param>
        /// <returns><c>true</c> if the key exists for the context, <c>false</c> otherwise</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public bool Contains(string key)
        {
            using (var site = CreateSite())
            {
                var list = new ConfigurationList(site);
                return list.ContainsKey(buildKeyFunc(key), this.contextId);
            }
        }

        /// <summary>
        /// Indexer for getting and setting values for the key specified.
        /// </summary>
        /// <param name="key">The key to check for in the property bag</param>
        /// <returns>The value for the key, null if not found</returns>
     
        public string this[string key]
        {
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                using (var site = CreateSite())
                {
                    var list = new ConfigurationList(site);
                    return list.Get(buildKeyFunc(key), this.contextId);
                }
            }
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            set
            {
                if (value == null)
                    Remove(key);
                else
                {
                    using (var site = CreateSite())
                    {
                        // AllowUnsafeUpdates required since the list may be located outside of 
                        // the current web/site.
                        site.RootWeb.AllowUnsafeUpdates = true;
                        var list = new ConfigurationList(site);
                        list.Save(buildKeyFunc(key), value, this.contextId);
                     }

                }
            }
        }

        /// <summary>
        /// Removes the key specified from the property bag.
        /// </summary>
        /// <param name="key">The key to remove from the property bag</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void Remove(string key)
        {
            using (var site = CreateSite())
            {
                site.RootWeb.AllowUnsafeUpdates = true;
                var list = new ConfigurationList(site);
                string fullKey = buildKeyFunc(key);

                if (list.ContainsKey(fullKey, this.contextId))
                {
                    list.Remove(fullKey, this.contextId);
                }
            }
        }
    }
}
