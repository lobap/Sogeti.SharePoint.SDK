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
using System.Linq;
using System.Text;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.Globalization;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.ProxyArgs;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// The manager responsible for setting, updating, and deleting configuration values
    /// from property bags, and looking up values from a specific property bag level.  The
    /// manager always operates on specific property bags.  For hierarchical look <see cref="HierarchicalConfig"/>
    /// </summary>
    public class ConfigManager : IConfigManager
    {
        const int maxRetryCount = 3;

        private ILogger logger;
        private static readonly string pnPKeyNamespace = "PnP.Config.Key";
        private static readonly int maxKeyLength = 255;
        private IConfigSettingSerializer configSettingSerializer;
        private IPropertyBagHierarchy hierarchy = null;
        private SPWeb webForContext;

        /// <summary>
        /// The prefix value used to give the pnp keys a namespace in order to 
        /// distinguish keys managed by the application setting manager.
        /// </summary>
        static public string PnPKeyNamespace
        {
            get
            {
                return pnPKeyNamespace;
            }
        }

        /// <summary>
        /// The maximum key length that can be stored.
        /// </summary>
        static public int MaxKeyLength
        {
            get
            {
                return maxKeyLength - pnPKeyNamespace.Length - SPSitePropertyBag.KeySuffix.Length;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigManager"/> class.
        /// </summary>
        public ConfigManager()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigManager"/> class.
        /// </summary>
        public ConfigManager(SPWeb web)
        {
            this.webForContext = web;
        }

        /// <summary>
        /// Sets the web to use for configuration.  Call this method when operating in 
        /// an environment where SPContext.Current is not available, like a feature receiver,
        /// event receiver, or console application before using the configuration manager.  
        /// For the default implementation property bags will be created using the web for context 
        /// (web, web.Site, web.Site.WebApplication, SPLocal.Farm).
        /// </summary>
        /// <param name="web">The web to use as a basis for property bags</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void SetWeb(SPWeb web)
        {
            this.webForContext = web;
        }

        /// <summary>
        /// Gets the current web context to use for creating property bags.  If the web was not set
        /// through either constructor injection (using ConfigManager(SPWeb)) or through method injection
        /// (by calling SetWeb), then the context for SPContext.Current.Web will be used.
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
        /// Initializes a new instance of the <see cref="ConfigManager"/> class.
        /// </summary>
        /// <param name="hierarchy">instance of <see cref="IPropertyBagHierarchy"/>.</param>
        public ConfigManager(IPropertyBagHierarchy hierarchy)
        {
            Validation.ArgumentNotNull(hierarchy, "hierarchy");
            this.hierarchy = hierarchy;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigManager"/> class.
        /// </summary>
        /// <param name="configSettingSerializer">The config setting serializer.</param>
        public ConfigManager(IConfigSettingSerializer configSettingSerializer)
        {
            Validation.ArgumentNotNull(configSettingSerializer, "configSettingSerializer");
            this.configSettingSerializer = configSettingSerializer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigManager"/> class.
        /// </summary>
        /// <param name="hierarchy">instance of <see cref="IPropertyBagHierarchy"/>.</param>
        /// <param name="configSettingSerializer">The config setting serializer.</param>
        public ConfigManager(IPropertyBagHierarchy hierarchy, IConfigSettingSerializer configSettingSerializer)
        {
            Validation.ArgumentNotNull(configSettingSerializer, "configSettingSerializer");
            this.configSettingSerializer = configSettingSerializer;
            this.hierarchy = hierarchy;
        }

        #region IConfigManager Members
        /// <summary>
        /// Reads a config value from the specified property bag, but will not look up the hierarchy.  This method
        /// will not throw an exception if the value is not configured, it will instead return the default value.
        /// </summary>
        /// <typeparam name="TValue">The type of the config value.</typeparam>
        /// <param name="key">The key the config value was stored under.</param>
        /// <param name="propertyBag">The property bag to read the config value from.</param>
        /// <returns>The config value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1"),
         System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public TValue GetFromPropertyBag<TValue>(string key, IPropertyBag propertyBag)
        {
            Validation.ArgumentNotNullOrEmpty(key, "key");
            Validation.ArgumentNotNull(propertyBag, "propertyBag");

            object val = GetProperty(typeof(TValue), key, propertyBag);

            if (val == null)
                return default(TValue);
            return (TValue)val;
        }

        /// <summary>
        /// Read a config value based on the key, from the specified property bag.  This method will throw a <see cref="ConfigurationException"/>
        /// exception if the value is not configured in the property bag.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to read.</typeparam>
        /// <param name="key">The key associated with the config value</param>
        /// <param name="propertyBag">The property bag to read values from.</param>
        /// <returns>The value read</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public TValue Get<TValue>(string key, IPropertyBag propertyBag)
        {
            Validation.ArgumentNotNullOrEmpty(key, "key");
            Validation.ArgumentNotNull(propertyBag, "propertyBag");

            string configValueAsString = string.Empty;

            try
            {
                return (TValue)GetProperty(typeof(TValue), key, propertyBag);
            }
            catch (ConfigurationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format(CultureInfo.CurrentCulture
                                                    ,
                                                    Properties.Resources.InvalidConfigSetting
                                                    , key
                                                    , configValueAsString
                                                    , typeof(TValue)
                                                    , ex.GetType().FullName
                                                    , ex.Message);

                throw new ConfigurationException(errorMessage, ex);
            }
        }

        /// <summary>
        /// Gets an instance of the type provided associated with the key provided at the level provided.
        /// This method will return null if the property is not found in the property bag.
        /// </summary>
        /// <param name="type">The type of configuration instance to get</param>
        /// <param name="key">The key of the configuration data</param>
        /// <param name="level">The level to get the property at</param>
        /// <returns>the value found, null if no configuration item found</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public object GetFromPropertyBag(Type type, string key, ConfigLevel level)
        {
            Validation.ArgumentNotNull(type, "type");
            Validation.ArgumentNotNull(key, "key");

            var propertyBag = GetPropertyBag(level);
            return GetProperty(type, key, propertyBag);
        }

        /// <summary>
        /// Remove a config value from a specified property bag. 
        /// </summary>
        /// <param name="key">The config setting to remove</param>
        /// <param name="propertyBag">The property bag to remove the setting from</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public void RemoveKeyFromPropertyBag(string key, IPropertyBag propertyBag)
        {
            Validation.ArgumentNotNullOrEmpty(key, "key");
            Validation.ArgumentNotNull(propertyBag, "propertyBag");

            string fullKey = GetNamespacedKey(key);

            bool savedConfig = false;
            Exception savedException = null;

            for (int retryCnt = 0; retryCnt < maxRetryCount && !savedConfig; retryCnt++)
            {
                // if multiple writes occur concurrently to the property bag for an object at the same time, SharePoint will throw.
                // catch that exception and retry saving the configuration a few times before giving up.
                try
                {
                    propertyBag.Remove(fullKey);
                    savedConfig = true;
                    if (retryCnt > 0)
                    {
                        this.Logger.TraceToDeveloper(string.Format("ConfigManager: Remove to config after retry to '{0}' level, retry count: '{1}', key: '{2}'", propertyBag.Level, retryCnt, key),
                            SandboxTraceSeverity.High);
                    }
                }
                catch (Exception ex)
                {
                    if (ExceptionManaged(ex))
                    {
                        savedException = ex;
                        this.Logger.TraceToDeveloper(string.Format("ConfigManager: Remove saving config to '{0}' level, retry count: '{1}', key: '{2}'", propertyBag.Level, retryCnt, key),
                            SandboxTraceSeverity.High);
                    }
                    else
                        throw;
                }
            }

            if (savedConfig == false)
                throw savedException;
            this.Logger.TraceToDeveloper(string.Format("ConfigManager: Removed config setting for key: '{0}'",  key),  SandboxTraceSeverity.Medium);
        }

        /// <summary>
        /// Returns true if the IPropertyBag contains the key
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <param name="propertyBag">The property bag to check</param>
        /// <returns>true if the key is in the property bag</returns>
        public bool ContainsKeyInPropertyBag(string key, IPropertyBag propertyBag)
        {
            Validation.ArgumentNotNullOrEmpty(key, "key");
            Validation.ArgumentNotNull(propertyBag, "propertyBag");
            string fullKey = GetNamespacedKey(key);
            return propertyBag.Contains(fullKey);
        }

        /// <summary>
        /// Gets the property bag hierarchy to use for the given context.  Override
        /// this method to provide an implementation of configuration manager that uses
        /// property bags that are not backed by the default use of property bags assoicated with
        /// the SPWeb, SPWebApplication, and SPFarm objects.  For example, you would override
        /// this method if you wanted to provide a list backed property bag implementation.
        /// </summary>
        /// <returns>The property bag hierarchy to use</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected virtual IPropertyBagHierarchy GetHierarchy()
        {
            if (hierarchy == null)
                hierarchy = HierarchyBuilder.GetHierarchy(this.WebContext);
            return hierarchy;
        }

        /// <summary>
        /// Gets the property bag for the level specified.  The object related to the level must
        /// be accessible from context (SPFarm.Local for farm, SPContext.Current for all others).
        /// </summary>
        /// <param name="level">the configuration level to get the bag for</param>
        /// <returns>the property bag for the requested level</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public IPropertyBag GetPropertyBag(ConfigLevel level)
        {
            IPropertyBag bag = GetHierarchy().GetPropertyBagForLevel(level);

            if (bag == null)
            {
                throw new ConfigurationException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.PropertyBagNotValidForContext, level));
            }

            return bag;
        }

        /// <summary>
        /// Is true if farm level config can be accessed, false otherwise.
        /// </summary>
        public bool CanAccessFarmConfig
        {
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                return SharePointEnvironment.CanAccessFarmConfig;
            }
        }
        #endregion


        /// <summary>
        /// Sets a config value for a specific key in the specified property bag
        /// </summary>
        /// <param name="key">The key for this config setting</param>
        /// <param name="value">The value to give this config setting</param>
        /// <param name="propertyBag">the property bag to store the setting in</param>
           [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
           [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "2")]
        public void SetInPropertyBag(string key, object value, IPropertyBag propertyBag)
        {
            Validation.ArgumentNotNullOrEmpty(key, "key");
            Validation.ArgumentNotNull(propertyBag, "propertyBag");

            ValidateKey(key);
            string fullKey = GetNamespacedKey(key);
            string valueAsString = null;
            string typeName = "(null)";

            try
            {
                if (value == null)
                {
                    propertyBag[fullKey] = null;
                }
                else
                {
                    // Now serialize the value. and set it in the PropertyBag. 
                    valueAsString = Serializer.Serialize(value.GetType(), value);
                }

                bool savedConfig = false;
                Exception savedException = null;

                for (int retryCnt = 0; retryCnt < maxRetryCount && !savedConfig; retryCnt++)
                {
                    // if multiple writes occur concurrently to the property bag for an object at the same time, SharePoint will throw.
                    // catch that exception and retry saving the configuration a few times before giving up.
                    try
                    {
                         propertyBag[fullKey] = valueAsString;
                         savedConfig = true;
                         if (retryCnt > 0)
                         {
                             this.Logger.TraceToDeveloper(string.Format("ConfigManager: Saved to config after retry to '{0}' level, retry count: '{1}', key: '{2}'", propertyBag.Level, retryCnt, key),
                                 SandboxTraceSeverity.High);
                         }
                    }
                    catch (Exception ex)
                    {
                        if (ExceptionManaged(ex))
                        {
                            savedException = ex;
                            this.Logger.TraceToDeveloper(string.Format("ConfigManager: Failure saving config to '{0}' level, retry count: '{1}', key: '{2}'", propertyBag.Level, retryCnt, key),
                                SandboxTraceSeverity.High);
                        }
                        else
                            throw;
                    }
                }

                if (savedConfig == false)
                    throw savedException;

                WriteToTraceLog(propertyBag, key, valueAsString);

            }
            catch (Exception ex)
            {
                valueAsString = "Null";

                if (value != null)
                {
                    try
                    {
                        valueAsString = value.ToString();
                    }
                    catch (Exception)
                    {
                        valueAsString = "ToString() failed";
                    }
                }


                typeName = value.GetType().FullName;

                string exceptionMessage = string.Format(CultureInfo.CurrentCulture
                                                        ,
                                                        Properties.Resources.ConfigSettingNotSet
                                                        , key
                                                        , valueAsString
                                                        , typeName
                                                        , ex.GetType().FullName
                                                        , ex.Message);

                throw new ConfigurationException(exceptionMessage, ex);
            }
        }


        private bool ExceptionManaged(Exception ex)
        {
            return ex.GetType() == typeof(SPUpdatedConcurrencyException) ? true : false;
        }



        /// <summary>
        /// Validates the key to make sure it doesn't contain invalid values. An invalid value throws a <see cref="ConfigurationException"/>. 
        /// </summary>
        /// <param name="key">The key.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        protected virtual void ValidateKey(string key)
        {
            Validation.ArgumentNotNull(key, "key");

            if (key.StartsWith(PnPKeyNamespace, StringComparison.Ordinal))
            {
                string errorMessage = string.Format(CultureInfo.CurrentCulture,
                                                    Properties.Resources.InvalidKeyPrefix,
                                                    key, PnPKeyNamespace);
                throw new ConfigurationException(errorMessage);
            }
            else if (key.Length > MaxKeyLength || key.Length == 0)
            {
                string errorMessage = string.Format(CultureInfo.CurrentCulture,
                                                   Properties.Resources.InvalidKeyLength,
                                                   key, MaxKeyLength, key.Length);
                throw new ConfigurationException(errorMessage);
            }
            else if (key.EndsWith(SPSitePropertyBag.KeySuffix))
            {
                string errorMessage = string.Format(CultureInfo.CurrentCulture,
                                                   Properties.Resources.InvalidKeySuffix,
                                                   key, SPSitePropertyBag.KeySuffix);
                throw new ConfigurationException(errorMessage);
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

        private ILogger Logger
        {
           
            get
            {
                if (this.logger == null)
                {

                    this.logger = SharePointServiceLocator.GetCurrent().GetInstance<ILogger>();
                }
                return this.logger;
            }
        }
        
        private void WriteToTraceLog(IPropertyBag propertyBag, string key, string valueAsString)
        {
            string logmessage = string.Format(CultureInfo.CurrentCulture,
                                              Properties.Resources.ValueSetInConfig
                                              , key, propertyBag.Level, valueAsString);

            this.Logger.TraceToDeveloper(logmessage, SandboxTraceSeverity.Verbose);
        }


        static internal string GetNamespacedKey(string key)
        {
            return PnPKeyNamespace + "." + key;
        }


        private object GetProperty(Type settingType, string key, IPropertyBag propertyBag)
        {
            string configValueAsString = propertyBag[ConfigManager.GetNamespacedKey(key)];

            if (configValueAsString != null)
                return Serializer.Deserialize(settingType, configValueAsString);
            else
                return null;
        }
    }
}
