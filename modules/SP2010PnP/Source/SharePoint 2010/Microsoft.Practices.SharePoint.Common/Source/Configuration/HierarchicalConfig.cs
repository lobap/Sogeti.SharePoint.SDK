//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Globalization;
using Microsoft.SharePoint;
using System.Collections.Generic;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// Class implements the <see cref="IHierarchicalConfig"/>, to allow users to read config settings in a hierarchical fashion. 
    /// </summary>
    public class HierarchicalConfig : IHierarchicalConfig
    {
        private IConfigSettingSerializer configSettingSerializer;
        private SPWeb webForContext = null;
        private IPropertyBagHierarchy propBagHierarchy = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalConfig"/> class.
        /// </summary>
        public HierarchicalConfig()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalConfig"/> class using the provided web.
        /// This constructor supports injecting the web to use rather than relying upon SPContext.Current.Web.
        /// </summary>
        /// <param name="web">The web to use to construct the configuration hierarchy</param>
        public HierarchicalConfig(SPWeb web)
        {
            SetWeb(web);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalConfig"/> class.
        /// </summary>
        /// <param name="hierarchy">instance of <see cref="IPropertyBagHierarchy"/>.</param>
        public HierarchicalConfig(IPropertyBagHierarchy hierarchy)
        {
            Validation.ArgumentNotNull(hierarchy, "hierarchy");
            this.propBagHierarchy = hierarchy;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalConfig"/> class.
        /// </summary>
        /// <param name="hierarchy">instance of <see cref="IPropertyBagHierarchy"/>.</param>
        /// <param name="configSettingSerializer">The config setting serializer.</param>
        public HierarchicalConfig(IPropertyBagHierarchy hierarchy, IConfigSettingSerializer configSettingSerializer)
        {
            Validation.ArgumentNotNull(configSettingSerializer, "configSettingSerializer");
            Validation.ArgumentNotNull(hierarchy, "hierarchy");
            this.configSettingSerializer = configSettingSerializer;
            this.propBagHierarchy = hierarchy;
        }

        /// <summary>
        /// Sets the web to use for the configuration hierarchy.  This supports injecting the web to use (method injection) for
        /// looking up the hierarchical configuration.  This supports using the service locator to find the IHierarchicalConfig instance
        /// to use, then setting the SPWeb to use for a lookup where an SPContext.Current does not exist.
        /// </summary>
        /// <param name="web"></param>
        public void SetWeb(SPWeb web)
        {
            this.webForContext = web;
        }

        /// <summary>
        /// Gets the web context to use.  If no web was injected through constructor or method injection, then the web is retrieved 
        /// from SPContext.Current.  Returns null of the web was not injected, and SPContext.Current is null.
        /// </summary>
        protected SPWeb WebContext
        {
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                if (this.webForContext == null && SPContext.Current != null)
                {
                    webForContext = SPContext.Current.Web;
                }
                return webForContext;
            }   
        }

        /// <summary>
        /// Gets the hierarchy of property bags to use for looking up a value.
        /// </summary>
        /// <returns>The <see cref="IPropertyBagHierarchy"/> to use for the lookup</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected virtual IPropertyBagHierarchy GetHierarchy()
        {
            if(this.propBagHierarchy == null)
                this.propBagHierarchy = HierarchyBuilder.GetHierarchy(this.WebContext);

            return this.propBagHierarchy;
        }

  

        #region IHierarchicalConfig members

        /// <summary>
        /// Determines if a config value with the specified key can be found in config.
        /// Will walk the hierarchy of property bags to find the key.
        /// </summary>
        /// <param name="key">the specified key</param>
        /// <returns>
        /// 	<c>true</c> if the specified property bag contains key; otherwise, <c>false</c>.
        /// </returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public bool ContainsKey(string key)
        {
            Validation.ArgumentNotNull(key, "key");
            return this.Contains(ConfigManager.GetNamespacedKey(key));
        }

        /// <summary>
        /// Determines if a config value with the specified key can be found in the hierarchical config at
        /// the specified level in the current context or above.  Will walk the hierarchy of property bags to find the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="level">The level to start looking in.</param>
        /// <returns>
        /// 	<c>true</c> if the specified property bag contains key; otherwise, <c>false</c>.
        /// </returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public bool ContainsKey(string key, ConfigLevel level)
        {
            Validation.ArgumentNotNull(key, "key");
            return this.ContainsFrom(ConfigManager.GetNamespacedKey(key), level);
        }

        /// <summary>
        /// Read a config value based on the key.  It will walk the hierarchy to find the key, starting
        /// at the first property bag in the hierarchy (typically the SPWeb).  Throws <see cref="ConfigurationException"/>
        /// if they key is not found.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to read.</typeparam>
        /// <param name="key">The key associated with the config value</param>
        /// <returns>The value</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public TValue GetByKey<TValue>(string key)
        {
            Validation.ArgumentNotNull(key, "key");
            string serializedSetting = this.GetSetting(ConfigManager.GetNamespacedKey(key));
            return (DeserializeSetting<TValue>(key, serializedSetting));
        }

        /// <summary>
        /// Read a config value based on the key, from the specified config level in the current context.
        /// If the value cannot be found at the specified level, it will walk the hierarchy from the level specified
        /// upward.  Throws <see cref="ConfigurationException"/> if they key is not found.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to read.</typeparam>
        /// <param name="key">The key associated with the config value</param>
        /// <param name="level">The config level to start looking in.
        /// For example, <see cref="ConfigLevel.CurrentSPWebApplication"/> means that it's looking at the current 'SPWebApplication'
        /// and above.</param>
        /// <returns>The value</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public TValue GetByKey<TValue>(string key, ConfigLevel level)
        {
            Validation.ArgumentNotNull(key, "key");
            string serializedSetting = this.GetSettingFrom(ConfigManager.GetNamespacedKey(key), level);
            return (DeserializeSetting<TValue>(key, serializedSetting));
        }

 
        #endregion

        private TValue DeserializeSetting<TValue>(string key, string serializedData)
        {
            if (serializedData == null)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture,
                                            Properties.Resources.KeyNotConfigured,
                                            key);
                throw new ConfigurationException(exceptionMessage);
            }
            else
            {
                try
                {
                    return (TValue)Serializer.Deserialize(typeof(TValue), serializedData);
                }
                catch (Exception ex)
                {
                    throw new ConfigurationException(string.Format(CultureInfo.CurrentCulture,
                        Properties.Resources.FailedDeserialization, key, serializedData), ex);
                }
            }
        }


        private IConfigSettingSerializer Serializer
        {
            get
            {
                if (this.configSettingSerializer == null)
                {
                    this.configSettingSerializer = new ConfigSettingSerializer();
                }
                return this.configSettingSerializer;
            }
        }

        /// <summary>
        /// Gets the serialized string data for the key specified.
        /// </summary>
        /// <param name="key">The key to retrieve data for</param>
        /// <returns></returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected string GetSetting(string key)
        {
            return this.GetSettingFrom(key, ConfigLevel.CurrentSPWeb);
        }

        /// <summary>
        /// Looks up if one of the property bags in the full hierarchy contains the key specified.
        /// </summary>
        /// <param name="key">The to check for</param>
        /// <returns><c>true</c> if the key is found, <c>false</c> otherwise.</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected bool Contains(string key)
        {
            return this.ContainsFrom(key, ConfigLevel.CurrentSPWeb);
        }

        /// <summary>
        /// Gets the serialized data starting at the level specified and walking the
        /// hierarchy if found, returns null if not found.
        /// </summary>
        /// <param name="key">The key to walk the hierarchy for</param>
        /// <param name="level">The level to start walking at</param>
        /// <returns><c>true</c> if the key is found, <c>false</c> otherwise.</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected string GetSettingFrom(string key, ConfigLevel level)
        {
            Validation.ArgumentNotNull(key, "key");

            string value = null;
            IPropertyBagHierarchy hierarchy = GetHierarchy();

            foreach (IPropertyBag bag in hierarchy.PropertyBags)
            {
                //property bags are ordered by level in the hierarchy
                if (bag.Level >= level)
                {
                    value = bag[key];
                    if (value != null)
                        break;
                }
            }

            return value;
        }

        /// <summary>
        /// Looks up if one of the property bags in the hierarchy contains the key specified starting
        /// at the level specified.
        /// </summary>
        /// <param name="key">The key to walk the hierarchy for</param>
        /// <param name="level">The level to start walking at</param>
        /// <returns><c>true</c> if the key is found, <c>false</c> otherwise.</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected bool ContainsFrom(string key, ConfigLevel level)
        {
            Validation.ArgumentNotNull(key, "key");
            bool contains = false;
            IPropertyBagHierarchy hierarchy = GetHierarchy();

            foreach (IPropertyBag bag in hierarchy.PropertyBags)
            {
                //property bags are ordered by level in the hierarchy
                if (bag.Level >= level)
                {
                    contains = bag.Contains(key);
                    if (contains)
                        break;
                }
            }

            return contains;
        }
    }
}