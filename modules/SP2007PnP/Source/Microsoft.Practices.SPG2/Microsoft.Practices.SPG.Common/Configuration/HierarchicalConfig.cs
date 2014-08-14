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
using System.Globalization;
using System.Security.Permissions;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SPG.Common.Configuration
{
    /// <summary>
    /// Class implements both <see cref="IConfigManager"/> (to read and write config settings in SharePoint property bags) and
    /// <see cref="IHierarchicalConfig"/>, to allow users to read config settings in a hierarchical fashion. 
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    public class HierarchicalConfig : IHierarchicalConfig, IConfigManager
    {
        private readonly IConfigSettingSerializer configSettingSerializer;
        private readonly SPContext currentContext;
        private ILogger logger;

        private IPropertyBag defaultPropertyBag;

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalConfig"/> class.
        /// </summary>
        /// <param name="defaultPropertyBag">The default property bag to use if no PropertyBag is specified.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public HierarchicalConfig(IPropertyBag defaultPropertyBag)
            : this(defaultPropertyBag, new ConfigSettingSerializer())
        {
            currentContext = SPContext.Current;
            this.defaultPropertyBag = defaultPropertyBag;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalConfig"/> class.
        /// </summary>
        /// <param name="defaultPropertyBag">The default property bag to use if no property bag is specified.</param>
        /// <param name="configSettingSerializer">The config setting serializer.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public HierarchicalConfig(IPropertyBag defaultPropertyBag, IConfigSettingSerializer configSettingSerializer)
        {
            if (configSettingSerializer == null) throw new ArgumentNullException("configSettingSerializer");
            currentContext = SPContext.Current;
            this.defaultPropertyBag = defaultPropertyBag;
            this.configSettingSerializer = configSettingSerializer;

            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalConfig"/> class.
        /// </summary>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public HierarchicalConfig() : this(null, new ConfigSettingSerializer())
        {
        }

        #region IHierarchicalConfig Members

        /// <summary>
        /// Determines if a config value with the specified key can be found in config.
        /// Will recursively look up to parent property bags to find the key.
        /// </summary>
        /// <param name="key">the specified key</param>
        /// <returns>
        /// 	<c>true</c> if the specified property bag contains key; otherwise, <c>false</c>.
        /// </returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public bool ContainsKey(string key)
        {
            return ContainsKey(key, GetDefaultPropertyBag());
        }

        /// <summary>
        /// Determines if a config value with the specified key can be found in the hierarchical config at
        /// the specified level in the current context or above.  Will recursively look up to parent property bags
        /// to find the key.
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
            return ContainsKey(key, GetIPropertyBag(level));
        }

        /// <summary>
        /// Reads a config value from the specified property bag, but will not look up the hierarchy.
        /// </summary>
        /// <typeparam name="TValue">The type of the config value.</typeparam>
        /// <param name="key">The key the config value was stored under.</param>
        /// <param name="propertyBag">The property bag to read the config value from.</param>
        /// <returns>The config value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1004:GenericMethodsShouldProvideTypeParameter"),
         SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public TValue GetFromPropertyBag<TValue>(string key, SPWeb propertyBag)
        {
            return GetFromPropertyBag<TValue>(key, GetIPropertyBag(propertyBag));
        }

        /// <summary>
        /// Reads a config value from the specified property bag, but will not look up the hierarchy.
        /// </summary>
        /// <typeparam name="TValue">The type of the config value.</typeparam>
        /// <param name="key">The key the config value was stored under.</param>
        /// <param name="propertyBag">The property bag to read the config value from.</param>
        /// <returns>The config value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1004:GenericMethodsShouldProvideTypeParameter"),
         SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public TValue GetFromPropertyBag<TValue>(string key, SPSite propertyBag)
        {
            return GetFromPropertyBag<TValue>(key, GetIPropertyBag(propertyBag));
        }

        /// <summary>
        /// Reads a config value from the specified property bag, but will not look up the hierarchy.
        /// </summary>
        /// <typeparam name="TValue">The type of the config value.</typeparam>
        /// <param name="key">The key the config value was stored under.</param>
        /// <param name="propertyBag">The property bag to read the config value from.</param>
        /// <returns>The config value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1004:GenericMethodsShouldProvideTypeParameter"),
         SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public TValue GetFromPropertyBag<TValue>(string key, SPWebApplication propertyBag)
        {
            return GetFromPropertyBag<TValue>(key, GetIPropertyBag(propertyBag));
        }

        /// <summary>
        /// Reads a config value from the specified property bag, but will not look up the hierarchy.
        /// </summary>
        /// <typeparam name="TValue">The type of the config value.</typeparam>
        /// <param name="key">The key the config value was stored under.</param>
        /// <param name="propertyBag">The property bag to read the config value from.</param>
        /// <returns>The config value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1004:GenericMethodsShouldProvideTypeParameter"),
         SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public TValue GetFromPropertyBag<TValue>(string key, SPFarm propertyBag)
        {
            return GetFromPropertyBag<TValue>(key, GetIPropertyBag(propertyBag));
        }

        /// <summary>
        /// Remove a config value from a specified location. This method will not look up in parent property bags.
        /// </summary>
        /// <param name="key">The config setting to remove</param>
        /// <param name="propertyBag">The property bag to remove it from</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void RemoveKeyFromPropertyBag(string key, SPFarm propertyBag)
        {
            RemoveKeyFromPropertyBag(key, GetIPropertyBag(propertyBag));
        }

        /// <summary>
        /// Remove a config value from a specified location. This method will not look up in parent property bags.
        /// </summary>
        /// <param name="key">The config setting to remove</param>
        /// <param name="propertyBag">The property bag to remove it from</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void RemoveKeyFromPropertyBag(string key, SPSite propertyBag)
        {
            RemoveKeyFromPropertyBag(key, GetIPropertyBag(propertyBag));
        }

        /// <summary>
        /// Remove a config value from a specified location. This method will not look up in parent property bags.
        /// </summary>
        /// <param name="key">The config setting to remove</param>
        /// <param name="propertyBag">The property bag to remove it from</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void RemoveKeyFromPropertyBag(string key, SPWebApplication propertyBag)
        {
            RemoveKeyFromPropertyBag(key, GetIPropertyBag(propertyBag));
        }

        /// <summary>
        /// Remove a config value from a specified location. This method will not look up in parent property bags.
        /// </summary>
        /// <param name="key">The config setting to remove</param>
        /// <param name="propertyBag">The property bag to remove it from</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void RemoveKeyFromPropertyBag(string key, SPWeb propertyBag)
        {
            RemoveKeyFromPropertyBag(key, GetIPropertyBag(propertyBag));
        }

        /// <summary>
        /// See if a particular property bag contains a config setting with that key.
        /// This method will not look up in parent property bags.
        /// </summary>
        /// <param name="key">The key to use to find the config setting</param>
        /// <param name="propertyBag">The property bag to look in</param>
        /// <returns>
        /// True if the key is found, else false
        /// </returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public bool ContainsKeyInPropertyBag(string key, SPFarm propertyBag)
        {
            return ContainsKeyInPropertyBag(key, GetIPropertyBag(propertyBag));
        }

        /// <summary>
        /// See if a particular property bag contains a config setting with that key.
        /// This method will not look up in parent property bags.
        /// </summary>
        /// <param name="key">The key to use to find the config setting</param>
        /// <param name="propertyBag">The property bag to look in</param>
        /// <returns>
        /// True if the key is found, else false
        /// </returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public bool ContainsKeyInPropertyBag(string key, SPSite propertyBag)
        {
            return ContainsKeyInPropertyBag(key, GetIPropertyBag(propertyBag));
        }

        /// <summary>
        /// See if a particular property bag contains a config setting with that key.
        /// This method will not look up in parent property bags.
        /// </summary>
        /// <param name="key">The key to use to find the config setting</param>
        /// <param name="propertyBag">The property bag to look in</param>
        /// <returns>
        /// True if the key is found, else false
        /// </returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public bool ContainsKeyInPropertyBag(string key, SPWebApplication propertyBag)
        {
            return ContainsKeyInPropertyBag(key, GetIPropertyBag(propertyBag));
        }

        /// <summary>
        /// See if a particular property bag contains a config setting with that key.
        /// This method will not look up in parent property bags.
        /// </summary>
        /// <param name="key">The key to use to find the config setting</param>
        /// <param name="propertyBag">The property bag to look in</param>
        /// <returns>
        /// True if the key is found, else false
        /// </returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public bool ContainsKeyInPropertyBag(string key, SPWeb propertyBag)
        {
            return ContainsKeyInPropertyBag(key, GetIPropertyBag(propertyBag));
        }

        /// <summary>
        /// Read a config value based on the key, from the default PropertyBag: SPContext.Current.Web
        /// If it can't find it in the default property bag it will look at it's parents.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to read. </typeparam>
        /// <param name="key">The key associated with the config value</param>
        /// <returns>The value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1004:GenericMethodsShouldProvideTypeParameter"),
         SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public TValue GetByKey<TValue>(string key)
        {
            return Get<TValue>(key, GetDefaultPropertyBag());
        }

        /// <summary>
        /// Read a config value based on the key, from the specified config level in the current context.
        /// If the value cannot be found at the specified level, it will look recursively up at it's parent.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to read.</typeparam>
        /// <param name="key">The key associated with the config value</param>
        /// <param name="level">The config level to start looking in.
        /// For example, <see cref="ConfigLevel.CurrentSPWebApplication"/> means that it's looking at the current 'SPWebApplication'
        /// and above.</param>
        /// <returns>The value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1004:GenericMethodsShouldProvideTypeParameter"),
         SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public TValue GetByKey<TValue>(string key, ConfigLevel level)
        {
            return Get<TValue>(key, GetIPropertyBag(level));
        }

        /// <summary>
        /// Sets a config value for a specific key in the specified property bag
        /// </summary>
        /// <param name="key">The key for this config setting</param>
        /// <param name="value">The value to give this config setting</param>
        /// <param name="propertyBag">The property bag to store the setting in</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void SetInPropertyBag(string key, object value, SPWeb propertyBag)
        {
            SetInPropertyBag(key, value, GetIPropertyBag(propertyBag));
        }

        /// <summary>
        /// Sets a config value for a specific key in the specified property bag
        /// </summary>
        /// <param name="key">The key for this config setting</param>
        /// <param name="value">The value to give this config setting</param>
        /// <param name="propertyBag">the property bag to store the setting in</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void SetInPropertyBag(string key, object value, SPSite propertyBag)
        {
            SetInPropertyBag(key, value, GetIPropertyBag(propertyBag));
        }

        /// <summary>
        /// Sets a config value for a specific key in the specified property bag
        /// </summary>
        /// <param name="key">The key for this config setting</param>
        /// <param name="value">The value to give this config setting</param>
        /// <param name="propertyBag">the property bag to store the setting in</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void SetInPropertyBag(string key, object value, SPWebApplication propertyBag)
        {
            SetInPropertyBag(key, value, GetIPropertyBag(propertyBag));
        }

        /// <summary>
        /// Sets a config value for a specific key in the specified property bag
        /// </summary>
        /// <param name="key">The key for this config setting</param>
        /// <param name="value">The value to give this config setting</param>
        /// <param name="propertyBag">the property bag to store the setting in</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void SetInPropertyBag(string key, object value, SPFarm propertyBag)
        {
            SetInPropertyBag(key, value, GetIPropertyBag(propertyBag));
        }

        #endregion

        /// <summary>
        /// Gets the default property bag to use if no property bag is specified. If not overwritten in the constructor, this value will use 
        /// SPContext.Current.Web.
        /// </summary>
        /// <value>The default property bag.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected IPropertyBag GetDefaultPropertyBag()
        {
            if (defaultPropertyBag == null)
            {
                if (currentContext == null)
                {
                    throw new NoSharePointContextException(
                        "The SPContext was not found. The default property bag needs access to the SPContext.Current because it wants to access the current Web. ");
                }

                defaultPropertyBag = new SPWebPropertyBag(currentContext.Web);
            }

            return defaultPropertyBag;
        }

        /// <summary>
        /// Determines if a config value with the specified key can be found in the specified property bag.
        /// Will recursively look up to parent property bags to find the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="propertyBag">The property bag.</param>
        /// <returns>
        /// 	<c>true</c> if the specified property bag contains key; otherwise, <c>false</c>.
        /// </returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected bool ContainsKey(string key, IPropertyBag propertyBag)
        {
            if (ContainsKeyInPropertyBag(key, propertyBag))
                return true;

            IPropertyBag parent = propertyBag.GetParent();
            if (parent == null)
                return false;

            return ContainsKey(key, parent);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private bool ContainsKeyInPropertyBag(string key, IPropertyBag propertyBag)
        {
            return propertyBag.Contains(key);
        }

        /// <summary>
        /// Remove a config value from a specified property bag. 
        /// </summary>
        /// <param name="key">The config setting to remove</param>
        /// <param name="propertyBag">The property bag to remove it from</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic"), SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void RemoveKeyFromPropertyBag(string key, IPropertyBag propertyBag)
        {
            if (propertyBag == null) throw new ArgumentNullException("propertyBag");
            propertyBag.Remove(key);
            propertyBag.Update();
        }

        /// <summary>
        /// Read a config value based on the key, from the specified property bag.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to read.</typeparam>
        /// <param name="key">The key associated with the config value</param>
        /// <param name="propertyBag">The property bag to read values from.</param>
        /// <returns>The value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1004:GenericMethodsShouldProvideTypeParameter"),
         SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public TValue Get<TValue>(string key, IPropertyBag propertyBag)
        {
            if (propertyBag == null) throw new ArgumentNullException("propertyBag");

            string configValueAsString = string.Empty;
            try
            {
                return GetValueFromPropertyBagOrParents<TValue>(key, propertyBag);
            }
            catch (ConfigurationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format(CultureInfo.CurrentCulture
                                                    ,
                                                    "Configsetting with key '{0}' could not be retrieved. The configured value could not be converted from '{1}' to an instance of '{2}'. The technical exception was: {3}: {4}"
                                                    , key
                                                    , configValueAsString
                                                    , typeof (TValue)
                                                    , ex.GetType().FullName
                                                    , ex.Message);

                throw new ConfigurationException(errorMessage, ex);
            }
        }

        private TValue GetValueFromPropertyBagOrParents<TValue>(string key, IPropertyBag propertyBag)
        {
            if (propertyBag.Contains(key))
            {
                return GetFromPropertyBag<TValue>(key, propertyBag);
            }

            IPropertyBag parent = propertyBag.GetParent();
            if (parent == null)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture,
                                                        "There was no value configured for key '{0}' in a propertyBag with level '{1}' or above.",
                                                        key, propertyBag.Level);
                throw new ConfigurationException(exceptionMessage);
            }

            return GetValueFromPropertyBagOrParents<TValue>(key, parent);
        }

        /// <summary>
        /// Reads a config value from the specified property bag, but will not look up the hierarchy.
        /// </summary>
        /// <typeparam name="TValue">The type of the config value.</typeparam>
        /// <param name="key">The key the config value was stored under.</param>
        /// <param name="propertyBag">The property bag to read the config value from.</param>
        /// <returns>The config value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public TValue GetFromPropertyBag<TValue>(string key, IPropertyBag propertyBag)
        {
            string configValueAsString = propertyBag[key];
            return (TValue) configSettingSerializer.Deserialize(typeof (TValue), configValueAsString);
        }

        /// <summary>
        /// Sets a config value for a specific key in the specified property bag
        /// </summary>
        /// <param name="key">The key for this config setting</param>
        /// <param name="value">The value to give this config setting</param>
        /// <param name="propertyBag">the property bag to store the setting in</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void SetInPropertyBag(string key, object value, IPropertyBag propertyBag)
        {
            if (propertyBag == null) throw new ArgumentNullException("propertyBag");

            ValidateKey(key);

            string valueAsString = "(null)";
            string typeName = "(null)";

            try
            {
                if (value == null)
                {
                    propertyBag[key] = null;
                }
                else
                {
                    // First, attempt to get the 'tostring' value. If serialization fails, then at least 
                    // in the error handler do we know 'something' about what we tried to serialize. 
                    valueAsString = value.ToString();
                    typeName = value.GetType().FullName;

                    // Now serialize the value. and set it in the PropertyBag. 
                    valueAsString = configSettingSerializer.Serialize(value.GetType(), value);
                    propertyBag[key] = valueAsString;
                }

                propertyBag.Update();

                WriteToTraceLog(propertyBag, key, valueAsString);
            }
            catch (Exception ex)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture
                                                        ,
                                                        "Configsetting with key '{0}' could not be set '{1}' with type '{2}'. The technical exception was: {3}: {4}"
                                                        , key
                                                        , valueAsString
                                                        , typeName
                                                        , ex.GetType().FullName
                                                        , ex.Message);

                throw new ConfigurationException(exceptionMessage, ex);
            }
        }

        private void WriteToTraceLog(IPropertyBag propertyBag, string key, string valueAsString)
        {
            string logmessage = string.Format(CultureInfo.CurrentCulture,
                                              "Set value in hierarchical config.\n\tKey: '{0}'\n\tLevel: '{1}'\n\tValue: '{2}'"
                                              , key, propertyBag.Level, valueAsString);

            this.Logger.TraceToDeveloper(logmessage, TraceSeverity.Verbose);
        }

        /// <summary>
        /// Validates the key to make sure it doesn't contain invalid values. An invalid value throws a <see cref="ConfigurationException"/>. 
        /// </summary>
        /// <param name="key">The key.</param>
        protected virtual void ValidateKey(string key)
        {
            if (key.StartsWith(SPSitePropertyBag.KeyPrefix, StringComparison.Ordinal))
            {
                string errorMessage = string.Format(CultureInfo.CurrentCulture,
                                                    "The key '{0}' cannot be used. Key's may not be prefixed with the text '{1}' because this is used by the SPSitePropertyBag to differentiate between properties of the SPWeb and the SPSite.",
                                                    key, SPSitePropertyBag.KeyPrefix);
                throw new ConfigurationException(errorMessage);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private IPropertyBag GetIPropertyBag(ConfigLevel level)
        {
            switch (level)
            {
                case ConfigLevel.CurrentSPFarm:
                    return new SPFarmPropertyBag();
                case ConfigLevel.CurrentSPWebApplication:
                    return new SPWebAppPropertyBag();
                case ConfigLevel.CurrentSPSite:
                    return new SPSitePropertyBag();
                case ConfigLevel.CurrentSPWeb:
                    return new SPWebPropertyBag();
                default:
                    throw new ArgumentException(
                        string.Format(CultureInfo.CurrentCulture,
                                      "The config level '{0}' was not supported. Use Farm, WebApplication, Site or Web.",
                                      level), "level");
            }
        }

        /// <summary>
        /// Get a <see cref="IPropertyBag"/> for the specified SharePoint object
        /// </summary>
        /// <param name="propertyBag">The SharePoint object to get the property bag for</param>
        /// <returns>The IPropertyBag</returns>
        protected virtual IPropertyBag GetIPropertyBag(SPFarm propertyBag)
        {
            if (propertyBag == null) throw new ArgumentNullException("propertyBag");
            return new SPFarmPropertyBag(propertyBag);
        }


        /// <summary>
        /// Get a <see cref="IPropertyBag"/> for the specified SharePoint object
        /// </summary>
        /// <param name="propertyBag">The SharePoint object to get the property bag for</param>
        /// <returns>The IPropertyBag</returns>
        protected virtual IPropertyBag GetIPropertyBag(SPWebApplication propertyBag)
        {
            if (propertyBag == null) throw new ArgumentNullException("propertyBag");
            return new SPWebAppPropertyBag(propertyBag);
        }

        /// <summary>
        /// Get a <see cref="IPropertyBag"/> for the specified SharePoint object
        /// </summary>
        /// <param name="propertyBag">The SharePoint object to get the property bag for</param>
        /// <returns>The IPropertyBag</returns>
        protected virtual IPropertyBag GetIPropertyBag(SPSite propertyBag)
        {
            if (propertyBag == null) throw new ArgumentNullException("propertyBag");
            return new SPSitePropertyBag(propertyBag);
        }

        /// <summary>
        /// Get a <see cref="IPropertyBag"/> for the specified SharePoint object
        /// </summary>
        /// <param name="propertyBag">The SharePoint object to get the property bag for</param>
        /// <returns>The IPropertyBag</returns>
        protected virtual IPropertyBag GetIPropertyBag(SPWeb propertyBag)
        {
            if (propertyBag == null) throw new ArgumentNullException("propertyBag");
            return new SPWebPropertyBag(propertyBag);
        }

        private ILogger Logger
        {
            get
            {
                if (this.logger == null)
                {
                    this.logger = SharePointServiceLocator.Current.GetInstance<ILogger>();
                }
                return this.logger;
            }
        }

    }
}