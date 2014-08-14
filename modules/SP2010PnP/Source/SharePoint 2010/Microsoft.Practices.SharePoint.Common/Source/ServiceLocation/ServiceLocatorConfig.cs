//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System.Collections.Generic;
using System.Security.Permissions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.SharePoint.Security;
using System;
using Microsoft.SharePoint;
using Microsoft.Practices.SharePoint.Common.Properties;
using System.Globalization;

namespace Microsoft.Practices.SharePoint.Common.ServiceLocation
{

    /// <summary>
    /// Class that reads and writes the <see cref="SharePointServiceLocator"/>'s configration in hierarchical config. It uses the 
    /// <see cref="HierarchicalConfig"/> to read and write values into the Farm level of hierarchical config. 
    /// </summary>
    public class ServiceLocatorConfig : IServiceLocatorConfig
    {
        private IConfigManager manager;
        private SPSite site = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocatorConfig"/> class that reads and writes values into 
        /// a config manager that reads and writes to the SPFarm.
        /// </summary>
        public ServiceLocatorConfig()
        {
            this.manager = new ConfigManager();
        }

        /// <summary>
        /// Creates a service locator config instance using the manager provided to access config information.
        /// </summary>
        /// <param name="manager"></param>
        public ServiceLocatorConfig(IConfigManager manager)
        {
            Validation.ArgumentNotNull(manager, "manager");
            this.manager = manager;
        }

        /// <summary>
        /// Sets the site to configure type mappings for.  When set, configuration will be
        /// retrieved and stored from the site collection provided.  If not set, then configuration
        /// will be stored and retrieved from the farm.
        /// </summary>
        public SPSite Site
        {
            get
            {
                return this.site;
            }
            set
            {
                Validation.ArgumentNotNull(value, "site");
                this.site = value;
                
            }
        }

        private IConfigManager Manager
        {
            get
            {
                if(this.manager == null)
                {
                    this.manager = new ConfigManager();         
                }

                return this.manager;
            }
        }

        /// <summary>
        /// Gets the interval to cache a site locator for
        /// </summary>
        /// <returns>the interval value, -1 if not configured</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public int GetSiteCacheInterval()
        {
            int interval = -1;

            if (SharePointEnvironment.CanAccessFarmConfig)
            {
                IPropertyBag bag = this.Manager.GetPropertyBag(ConfigLevel.CurrentSPFarm);

                if (bag != null && this.Manager.ContainsKeyInPropertyBag(this.GetSiteCacheIntervalConfigKey(), bag))
                {
                    interval = this.Manager.GetFromPropertyBag<int>(this.GetSiteCacheIntervalConfigKey(), bag);
                }
            }
            return interval;
        }

        /// <summary>
        /// Sets the site cache interval to the value provided.
        /// </summary>
        /// <param name="interval">The interval to set.  Must be greater than zero</param>
        public void SetSiteCacheInterval(int interval)
        {
            if(interval < 0)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, 
                    Resources.ArgumentMustBeGreaterThanZero, "SiteCachingTimeoutInSeconds", interval));
            }

            IPropertyBag bag = this.Manager.GetPropertyBag(ConfigLevel.CurrentSPFarm);

            if (bag != null)
            {
                this.Manager.SetInPropertyBag(this.GetSiteCacheIntervalConfigKey(), interval, bag);
            }
        }

        /// <summary>
        /// Register a type mapping between two types. When asking for a TFrom, an instance of TTo is returned.
        /// </summary>
        /// <typeparam name="TFrom">The type that can be requested.</typeparam>
        /// <typeparam name="TTo">The type of object that should be returned when asking for a type.</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void RegisterTypeMapping<TFrom, TTo>()
            where TTo : TFrom, new()
        {
            RegisterTypeMapping<TFrom, TTo>(null, InstantiationType.NewInstanceForEachRequest);
        }

        /// <summary>
        /// Register a type mapping between two types. When asking for a TFrom, an instance of TTo is returned.
        /// </summary>
        /// <typeparam name="TFrom">The type that can be requested.</typeparam>
        /// <typeparam name="TTo">The type of object that should be returned when asking for a type.</typeparam>
        /// <param name="key">The key that's used to store the type mapping.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void RegisterTypeMapping<TFrom, TTo>(string key)
            where TTo : TFrom, new()
        {
            RegisterTypeMapping<TFrom, TTo>(key, InstantiationType.NewInstanceForEachRequest);
        }

        /// <summary>
        /// Register a type mapping between TFrom and TTo with specified key. When <see cref="IServiceLocator.GetInstance(System.Type)"/> with
        /// parameter TFrom is called, an instance of type TTO is returned. 
        /// </summary>
        /// <typeparam name="TFrom">The type to register type mappings for. </typeparam>
        /// <typeparam name="TTo">The type to create if <see cref="IServiceLocator.GetInstance(System.Type)"/> is called with TFrom. </typeparam>
        /// <param name="instantiationType">Determines how the type should be created. </param>
        /// <param name="key">The key that's used to store the type mapping.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public virtual void RegisterTypeMapping<TFrom, TTo>(string key, InstantiationType instantiationType)
            where TTo : TFrom, new()
        {
            List<TypeMapping> typeMappings = GetConfigData();

            TypeMapping newTypeMapping = TypeMapping.Create<TFrom, TTo>(key, instantiationType);

            RemovePreviousMappingsForFromType(typeMappings, newTypeMapping);
            typeMappings.Add(newTypeMapping);

            SetTypeMappingsList(typeMappings);
        }


        /// <summary>
        /// Remove all type mappings for a specified type. 
        /// </summary>
        /// <typeparam name="T">The type to remove type mappings for. </typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public virtual void RemoveTypeMappings<T>()
        {
            List<TypeMapping> typeMappings = GetConfigData();

            foreach (TypeMapping mapping in typeMappings.ToArray())
            {
                if (mapping.FromType == typeof(T).AssemblyQualifiedName)
                {
                    typeMappings.Remove(mapping);
                }
            }

            SetTypeMappingsList(typeMappings);
        }

        /// <summary>
        /// Remove type mapping for type with specified key. 
        /// </summary>
        /// <typeparam name="T">The type to remove type mapping for. </typeparam>
        /// <param name="key">The key that was used to register the type mapping. Use null for default key. </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public virtual void RemoveTypeMapping<T>(string key)
        {
            List<TypeMapping> typeMappings = GetConfigData();

            foreach (TypeMapping mapping in typeMappings.ToArray())
            {
                if (mapping.FromType == typeof(T).AssemblyQualifiedName
                    && mapping.Key == key)
                {
                    typeMappings.Remove(mapping);
                }
            }

            SetTypeMappingsList(typeMappings);
        }

        /// <summary>
        /// Removes a specified type mapping from the list of type mappings. 
        /// </summary>
        /// <param name="mapping"></param>
        /// 
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public virtual void RemoveTypeMapping(TypeMapping mapping)
        {
            List<TypeMapping> typeMappings = GetConfigData();

            int index = typeMappings.FindIndex((t) => t.Equals(mapping));

            if(index >= 0)
            {
                typeMappings.RemoveAt(index);
            }

            SetTypeMappingsList(typeMappings);
        }

        /// <summary>
        /// Returns the list of type mappings that's stored in the config. 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public List<TypeMapping> GetTypeMappings()
        {
            return this.GetConfigData();
        }


        private static void RemovePreviousMappingsForFromType(List<TypeMapping> mappings, TypeMapping newTypeMapping)
        {
            foreach(TypeMapping mapping in mappings.ToArray())
            {
                if (mapping.FromType == newTypeMapping.FromType 
                    && mapping.Key == newTypeMapping.Key)
                {
                    mappings.Remove(mapping);
                }
            }
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private void SetTypeMappingsList(List<TypeMapping> typeMappings)
        {
            // We need to deal with low level serialization to break the dependency between service location and 
            // configuration manager, otherwise config mgr and hierarchical config can't be overridden.
            IPropertyBag propertyBag = GetPropertyBag();

            if (propertyBag != null)
            {
                var configData = new ServiceLocationConfigData(typeMappings);
                configData.LastUpdate = DateTime.Now;
               
                    Manager.SetInPropertyBag(GetConfigKey(), configData, propertyBag);
            }
            else
            {
                throw new InvalidOperationException( Resources.ContextMissingSetTypeMappingList);
            }
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private ServiceLocationConfigData GetConfigData()
        {
            // We need to deal with low level seralization to break the dependency between service location and 
            // configuration manager, otherwise config mgr and hierarchical config can't be overridden.
            IPropertyBag propertyBag = GetPropertyBag();
            ServiceLocationConfigData configData = null;

            //In some cases this will be null, like when attempting to load farm service locator config from the
            //sandbox without the proxy installed.
            if (propertyBag != null)
            {
                configData = Manager.GetFromPropertyBag<ServiceLocationConfigData>(GetConfigKey(), propertyBag);
            }
            
            if(configData == null)
            {
                configData = new ServiceLocationConfigData() {LastUpdate = null};
            }

            return configData;
        }

        /// <summary>
        /// Gets the property bag to retrieve configuration data from.  If the site is set, it
        /// will retrieve the configuration data from the site, otherwise the configuration data
        /// will be retrieved from the farm.
        /// </summary>
        /// <returns>The property bag to use for the service locator config operations.</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected virtual IPropertyBag GetPropertyBag()
        {
            IPropertyBag bag = null;

            if (site == null)
            {
                if (SharePointEnvironment.CanAccessFarmConfig)
                {
                    bag = Manager.GetPropertyBag(ConfigLevel.CurrentSPFarm);
                }
            }
            else
            {
                Manager.SetWeb(site.RootWeb);
                bag = Manager.GetPropertyBag(ConfigLevel.CurrentSPSite);
            }

            return bag;
        }

        /// <summary>
        /// The last time configuration was updated, null if it has not been updated.
        /// </summary>
        public DateTime ?LastUpdate
        {
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                return GetConfigData().LastUpdate;
            }
        }

        /// <summary>
        /// The config key used to store the config values. 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected virtual string GetConfigKey()
        {
            return "Microsoft.Practices.SharePoint.Common.TypeMappings";
        }

        /// <summary>
        /// Gets the key for getting/setting the cache timeout for site service locators.
        /// </summary>
        /// <returns>The key to use to store the config data for the site cache interval</returns>
        protected virtual string GetSiteCacheIntervalConfigKey()
        {
            return "Microsoft.Practices.SharePoint.Common.SiteLocatorCacheInterval";
        }
    }
}
