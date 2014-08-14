//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// A property bag that is backed by a list.  
    /// </summary>
    public class ListBackedPropertyBag: IPropertyBag
    {
        private readonly ConfigLevel level;
        private readonly string contextId;
        private ConfigurationList configList;
        Func<string, string> buildKeyFunc = null;

        /// <summary>
        /// Gets the configuration level for the property bag.
        /// </summary>
        public ConfigLevel Level
        {
            get
            {
                return this.level;
            }
        }

        /// <summary>
        /// Gets the context ID for this property bag.  The context ID is used in combination with a 
        /// key to uniquely identify a value.  The context ID identifies the configuration "partition", for 
        /// example, a web.ID would be the ContextId for all key values stored for a web instance.
        /// </summary>
        public string ContextId
        {
            get
            {
                return this.contextId;
            }
        }

        /// <summary>
        /// Constructs a list backed property bag.
        /// </summary>
        /// <param name="configListSite">The site to use for retrieving the configuration list</param>
        /// <param name="contextId">The context Id for this property bag, for example, web.ID</param>
        /// <param name="level">The level for this property bag</param>
        public ListBackedPropertyBag(SPSite configListSite, string contextId, ConfigLevel level)
        {
            this.level = level;
            this.contextId = contextId;
            this.configList = new ConfigurationList(configListSite);
            buildKeyFunc = (key) => key;  //by default just return key.
        }

        /// <summary>
        /// Constructs a list backed property bag with a function for transforming the key.
        /// </summary>
        /// <param name="configListSite">The site to use for retrieving the configuration list</param>
        /// <param name="contextId">The context Id for this property bag, for example, web.ID</param>
        /// <param name="level">The level for this property bag</param>
        /// <param name="buildKeyFunc">The function to use to transform the key to use to actually store</param>
        public ListBackedPropertyBag(SPSite configListSite, string contextId, ConfigLevel level, Func<string, string> buildKeyFunc)
        {
            this.level = level;
            this.contextId = contextId;
            this.configList = new ConfigurationList(configListSite);
            this.buildKeyFunc = buildKeyFunc;
        }

        /// <summary>
        /// Checks if a key is in the property bag.
        /// </summary>
        /// <param name="key">The key to check for</param>
        /// <returns><c>true</c> if the key is in the property bag, <c>false</c> otherwise</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public bool Contains(string key)
        {
            return this.configList.ContainsKey(buildKeyFunc(key), this.contextId);

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
                return this.configList.Get(buildKeyFunc(key), this.contextId);
            }
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            set
            {
                if (value != null)
                    this.configList.Save(buildKeyFunc(key), value, this.contextId);
                else
                    this.configList.Remove(buildKeyFunc(key), this.contextId);
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
            if (this.Contains(key))
            {
                this[key] = null;
            }
        }
    }
}
