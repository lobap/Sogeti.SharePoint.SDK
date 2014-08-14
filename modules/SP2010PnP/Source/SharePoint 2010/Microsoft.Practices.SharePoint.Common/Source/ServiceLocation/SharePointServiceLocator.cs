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
using System.Security.Permissions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint;
using Microsoft.Practices.SharePoint.Common.Properties;

namespace Microsoft.Practices.SharePoint.Common.ServiceLocation
{
    /// <summary>
    /// Class that manages a single instance of of a service locator using farm or site collection
    /// configuration.
    /// </summary>
    public class SharePointServiceLocator
    {
        private const int defaultSiteCacheIntervalInSeconds = 60;

        private static IServiceLocator serviceLocatorInstance;
        private static Dictionary<Guid, SiteLocatorEntry> siteLocators = new Dictionary<Guid, SiteLocatorEntry>();
        private static object syncRoot = new object();
        static SharePointServiceLocator sharePointLocatorInstance = null;
        static IServiceLocatorConfig farmServiceLocatorConfig;

        private class SiteLocatorEntry
        {
            public List<TypeMapping> SiteMappings {get;set;}
            public DateTime LoadTime { get; set; }
            public IServiceLocator locator;
        }

        static int _siteCacheInterval = -1;

        /// <summary>
        /// The time in seconds between refreshing the configuration of a site level locator.
        /// </summary>
        public static int SiteCachingTimeoutInSeconds
        {
            get
            {
                if (_siteCacheInterval == -1)
                {
                    _siteCacheInterval = SharePointLocator.GetSiteCacheInterval();
                }
                return _siteCacheInterval;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ArgumentMustBeGreaterThanZero, "SiteCachingTimeoutInSeconds", value));
                }

                _siteCacheInterval = value;
            }
        }

        private IServiceLocatorConfig FarmLocatorConfig
        {
            get
            {
                if (farmServiceLocatorConfig == null)
                {
                    farmServiceLocatorConfig = this.GetServiceLocatorConfig();
                }

                return farmServiceLocatorConfig;
            }
        }


        private static SharePointServiceLocator SharePointLocator
        {
            get
            {
                if (sharePointLocatorInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (sharePointLocatorInstance == null)
                        {
                            sharePointLocatorInstance = new SharePointServiceLocator();
                        }
                    }
                }
                return sharePointLocatorInstance;
            }
        }


        /// <summary>
        /// The current container for services.  This will look up type mappings at the site level and 
        /// the farm level if SPContext exists, otherwise it will only look at the farm level.
        /// Settings at the SPSite will be override settings at SPFarm.  In a sandbox solution, it
        /// will always only look at the site level unless the farm proxy is installed.
        /// </summary>
         /// <returns>The farm/site locator if a context exists, otherwise the farm only locator</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public static IServiceLocator GetCurrent()
        {
            return SharePointLocator.DoGetCurrent();
        }

        /// <summary>
        /// The current container for services for the site specified.  This will look up type mappings 
        /// at the site level and the farm level.Settings at the SPSite will be override settings at SPFarm.
        /// Use when SPContext is not available and but you want to lookup values from the site level.
        /// </summary>
        /// <param name="site">The site that holds config. Pass in null if site not avaiable</param>
        /// <returns>the farm/site service locator for the site specified</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public static IServiceLocator GetCurrent(SPSite site)
        {
            return SharePointLocator.DoGetCurrent(site);
        }

        /// <summary>
        /// This will return a locator loading only farm level settings.  Use this if you want to only use
        /// type mappings defined at the farm level, or if you want to add a type mapping at runtime rather
        /// than through configuration that applies across the entire farm.
        /// </summary>
        /// <returns>the farm level service locator</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public static IServiceLocator GetCurrentFarm()
        {
            return SharePointLocator.GetCurrentFarmLocator();
        }


        /// <summary>
        /// This will lookup a setting at the site level. This method is virtual to improve
        /// testability.
        /// </summary>
        /// <param name="site">The site to get the currently locator for</param>
        /// <returns>the site locator</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected IServiceLocator DoGetCurrent(SPSite site)
        {
            Validation.ArgumentNotNull(site, "site");

            IServiceLocator locator = null;

            if (siteLocators.ContainsKey(site.ID))
            {
                SiteLocatorEntry entry = siteLocators[site.ID];

                if (DateTime.Now.Subtract(entry.LoadTime).TotalSeconds < SiteCachingTimeoutInSeconds)
                {
                    locator = siteLocators[site.ID].locator;
                }
                else
                {
                    locator = RefreshServiceLocatorInstance(site);
                }
            }
            else
            {
                locator = CreateServiceLocatorInstance(site);
            }

            return locator;
        }

        /// <summary>
        /// Gets the current service locator for the farm
        /// </summary>
        /// <returns></returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected IServiceLocator DoGetCurrent()
        {
            if (SPContext.Current != null)
            {
                return this.DoGetCurrent(SPContext.Current.Site);
            }
            else
            {
                return this.GetCurrentFarmLocator();
            }
        }
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private IServiceLocator GetCurrentFarmLocator()
        {
            if (serviceLocatorInstance == null)
            {
                lock (syncRoot)
                {
                    if (serviceLocatorInstance == null)
                    {
                        // The SharePoint service locator has to have access to SPFarm.Local, because it uses the ServiceLocatorConfig 
                        // to store it's configuration settings. 
                        if (SharePointEnvironment.CanAccessSharePoint == false)
                        {
                            throw new NoSharePointContextException(Properties.Resources.InvalidRunContext);
                        }

                        EnsureCommonServiceLocatorCurrentFails();
                        serviceLocatorInstance = CreateServiceLocatorInstance();
                        if (serviceLocatorInstance.GetType() == typeof(ActivatingServiceLocator))
                        {
                            var activatingLocator = (ActivatingServiceLocator)serviceLocatorInstance;
                            activatingLocator.MappingRegisteredEvent += SharePointLocator.OnFarmTypeMappingChanged;
                        }
                    }
                }
            }
            return serviceLocatorInstance;
        }

        /// <summary>
        /// Make sure that the <see cref="ServiceLocator.Current"/> from the common service locator library doesn't work.
        /// It should fail with a <see cref="NotSupportedException"/>, because people should use <see cref="GetCurrent()"/>.
        /// </summary>
        private void EnsureCommonServiceLocatorCurrentFails()
        {
            ServiceLocator.SetLocatorProvider(throwNotSupportedException);
        }

        private IServiceLocator throwNotSupportedException()
        {
            throw new NotSupportedException(Properties.Resources.ServiceLocatorNotSupported);
        }

        /// <summary>
        /// Create a new instance of the service locator and possibly fill it with
        /// </summary>
        /// <returns></returns>
        private IServiceLocator CreateServiceLocatorInstance()
        {
            IEnumerable<TypeMapping> configuredTypeMappings = this.FarmLocatorConfig.GetTypeMappings();

            // Create the factory that can configure and create the service locator
            // It's possible that the factory to be used has been changed in config. 
            IServiceLocatorFactory serviceLocatorFactory = GetServiceLocatorFactory(configuredTypeMappings);

            // Create the service locator and load it up with the default and configured type mappings
            IServiceLocator serviceLocator = serviceLocatorFactory.Create();
            serviceLocatorFactory.LoadTypeMappings(serviceLocator, GetDefaultTypeMappings());
            serviceLocatorFactory.LoadTypeMappings(serviceLocator, configuredTypeMappings);

            return serviceLocator;
        }

        /// <summary>
        /// Gets the service locator config data to use for the farm config settings.  This is made virtual
        /// for testability.
        /// </summary>
        /// <returns></returns>
        protected virtual IServiceLocatorConfig GetServiceLocatorConfig()
        {
            return  new ServiceLocatorConfig();
        }

        /// <summary>
        /// Gets the service locator config for site config settings.  Made virtual for testability.  
        /// </summary>
        /// <param name="site">The site to load config settings for</param>
        /// <returns>The config settings for a site</returns>
        protected virtual IServiceLocatorConfig GetServiceLocatorConfig(SPSite site)
        {
            var config = new ServiceLocatorConfig();
            config.Site = site;
            return config;
        }

        /// <summary>
        /// Create a new instance of the service locator and possibly fill it with farm and 
        /// site level mappings.
        /// </summary>
        /// <returns>The service locator instance for the site.  Combines site and farm level mappings</returns>
        private IServiceLocator CreateServiceLocatorInstance(SPSite site)
        {

            IEnumerable<TypeMapping> configuredTypeMappings =  this.FarmLocatorConfig.GetTypeMappings();
            IServiceLocatorFactory factory = GetServiceLocatorFactory(configuredTypeMappings);
            var entry = new SiteLocatorEntry();
            entry.LoadTime = DateTime.Now;
            IServiceLocatorConfig siteserviceLocatorConfig = GetServiceLocatorConfig(site);

            entry.SiteMappings = siteserviceLocatorConfig.GetTypeMappings();
            entry.locator = factory.Create();

            lock (syncRoot)
            {
                var farmLocator = this.GetCurrentFarmLocator();

                if (farmLocator.GetType() == typeof(ActivatingServiceLocator))
                {
                    // call order is important, the mappings at the site will override the mappings at the service.
                    factory.LoadTypeMappings(entry.locator, ((ActivatingServiceLocator)farmLocator).GetTypeMappings());
                    factory.LoadTypeMappings(entry.locator, entry.SiteMappings);
                }
                else
                {
                    //since registering runtime registeration is a feature of activating service locator, we will
                    //assume that if it is not an activating locator, it is not supporting runtime registration.
                    SetupCustomLocator(factory, entry.locator, entry.SiteMappings);

                }

                siteLocators[site.ID] = entry;
            }

            return entry.locator;
        }

        private void SetupCustomLocator(IServiceLocatorFactory factory, IServiceLocator locator, IEnumerable<TypeMapping> siteMappings)
        {
            factory.LoadTypeMappings(locator, GetDefaultTypeMappings());
            factory.LoadTypeMappings(locator, FarmLocatorConfig.GetTypeMappings());
            factory.LoadTypeMappings(locator, siteMappings);
        }
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private IServiceLocator RefreshServiceLocatorInstance(SPSite site)
        {
            var siteserviceLocatorConfig = GetServiceLocatorConfig(site);
            SiteLocatorEntry entry = siteLocators[site.ID];

            //only update if changed since last time we loaded...
            if (siteserviceLocatorConfig.LastUpdate > entry.LoadTime)
            {
                if(entry.locator.GetType() == typeof(ActivatingServiceLocator))
                {
                    // get any mappings added programmatically.  Assume this behavior is unique to 
                    //Activating service locator, so ignore if not activating.
                    var activatingLocator = (ActivatingServiceLocator)entry.locator;
                    activatingLocator.Refresh(GetDefaultTypeMappings(),
                        FarmLocatorConfig.GetTypeMappings(),
                        entry.SiteMappings, 
                        siteserviceLocatorConfig.GetTypeMappings());
                }
                else
                {
                    IServiceLocatorFactory factory = GetServiceLocatorFactory(this.FarmLocatorConfig.GetTypeMappings());
                    SetupCustomLocator(factory, entry.locator, siteserviceLocatorConfig.GetTypeMappings());
                }
                entry.SiteMappings = siteserviceLocatorConfig.GetTypeMappings();
            }

            entry.LoadTime = DateTime.Now;
            return entry.locator;
        }


        private void OnFarmTypeMappingChanged(object sender, TypeMappingChangedArgs args)
        {
            lock (syncRoot)
            {
                foreach (SiteLocatorEntry entry in siteLocators.Values)
                {
                    //only override if not defined at the site level.
                    if (entry.SiteMappings == null ||
                        entry.SiteMappings.Exists((t) => t.Key == args.Mapping.Key &&
                                                    t.FromAssembly == args.Mapping.FromAssembly &&
                                                    t.FromType == args.Mapping.FromType) == false)
                    {
                        var activatingLocator = entry.locator as ActivatingServiceLocator;
                        if (activatingLocator != null)
                        {
                            activatingLocator.RegisterTypeMapping(args.Mapping);
                        }
                    }
                }
            }
        }

        private static IServiceLocatorFactory GetServiceLocatorFactory(IEnumerable<TypeMapping> configuredTypeMappings)
        {
            // Find configured factory. If it's there, creat it. 
            var factory = FindAndCreateConfiguredType<IServiceLocatorFactory>(configuredTypeMappings);

            // If there is no configured factory, then the ActivatingServiceLocatorFactory is the default one to use
            if (factory == null)
            {
                factory = new ActivatingServiceLocatorFactory();
            }

            return factory;
        }

        private static readonly List<TypeMapping> defaultMappings = new List<TypeMapping>()
                                                               {
                                                                    TypeMapping.Create<ILogger, SharePointLogger>(),
                                                                    TypeMapping.Create<ITraceLogger, TraceLogger>(),
                                                                    TypeMapping.Create<IEventLogLogger, EventLogLogger>(),
                                                                    TypeMapping.Create<IHierarchicalConfig, HierarchicalConfig>(),
                                                                    TypeMapping.Create<IConfigManager, ConfigManager>(),
                                                                    TypeMapping.Create<IServiceLocatorConfig, ServiceLocatorConfig>(),
                                                                    TypeMapping.Create<IApplicationContextProvider, ApplicationContextProvider>(),
                                                               };

        private static IEnumerable<TypeMapping> GetDefaultTypeMappings()
        {
            return defaultMappings;
        }

        private static TService FindAndCreateConfiguredType<TService>(IEnumerable<TypeMapping> configuredTypeMappings)
            where TService : class
        {
            TypeMapping mapping = FindMappingForType<TService>(configuredTypeMappings);
            if (mapping == null)
                return null;

            return (TService) ActivatingServiceLocator.CreateInstanceFromTypeMapping(mapping);
        }

        private static TypeMapping FindMappingForType<TService>(IEnumerable<TypeMapping> configuredTypeMappings)
        {
            if (configuredTypeMappings == null)
                return null;

            foreach (TypeMapping configuredMapping in configuredTypeMappings)
            {
                if(configuredMapping.FromType == typeof(TService).AssemblyQualifiedName)
                {
                    return configuredMapping;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the caching interval for the site collection locator.
        /// </summary>
        /// <returns>The interval for caching a site service locator</returns>
        public int GetSiteCacheInterval()
        {
            IServiceLocatorConfig config = GetServiceLocatorConfig();
            int cacheInterval = config.GetSiteCacheInterval();

            if (cacheInterval == -1)
                cacheInterval = defaultSiteCacheIntervalInSeconds;

            return cacheInterval;
        }

        /// <summary>
        /// Replace the static instance of service locator with a new service locator instance.
        /// </summary>
        /// <param name="newServiceLocator">The new service locator to use from now on. </param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public static void ReplaceCurrentServiceLocator(IServiceLocator newServiceLocator)
        {
            SharePointLocator.EnsureCommonServiceLocatorCurrentFails();
            serviceLocatorInstance = newServiceLocator;
        }

        /// <summary>
        /// Reset the service locator back to the default service locator. 
        /// </summary>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public static void Reset()
        {
            ReplaceCurrentServiceLocator(null);
            farmServiceLocatorConfig = null;
            siteLocators.Clear();
        }
    }
}
