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
using System.Collections.Generic;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SPG.Common.ServiceLocation
{

    /// <summary>
    /// Class that manages a single instance of of a service locator.
    /// </summary>
    [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    public static class SharePointServiceLocator
    {
        private static IServiceLocator serviceLocatorInstance;
        private static object syncRoot = new object();
        /// <summary>
        /// The current ambient container.
        /// </summary>
        public static IServiceLocator Current
        {
            get
            {
                EnsureCommonServiceLocatorCurrentFails();

                if (serviceLocatorInstance != null)
                    return serviceLocatorInstance;

                lock(syncRoot)
                {
                    if (serviceLocatorInstance != null)
                        return serviceLocatorInstance;

                    return serviceLocatorInstance = CreateServiceLocatorInstance();
                }
            }
        }

        private static volatile bool EnsureCommonServiceLocatorCurrentFailsSet = false;

        /// <summary>
        /// Make sure that the <see cref="ServiceLocator.Current"/> from the common service locator library doesn't work.
        /// It should fail with a <see cref="NotSupportedException"/>, because people should use <see cref="Current"/>.
        /// </summary>
        private static void EnsureCommonServiceLocatorCurrentFails()
        {
            if (!EnsureCommonServiceLocatorCurrentFailsSet)
            {
                ServiceLocator.SetLocatorProvider(throwNotSupportedException);
                EnsureCommonServiceLocatorCurrentFailsSet = true;
            }
        }

        private static IServiceLocator throwNotSupportedException()
        {
            throw new NotSupportedException("ServiceLocator.Current is not supported. Use SharePointServiceLocator.Current instead.");
        }

        /// <summary>
        /// Create a new instance of the service locator and possibly fill it with
        /// </summary>
        /// <returns></returns>
        private static IServiceLocator CreateServiceLocatorInstance()
        {
            // The SharePoint service locator has to have access to SPFarm.Local, because it uses the ServiceLocatorConfig 
            // to store it's configuration settings. 
            if (SPFarm.Local == null)
            {
                throw new NoSharePointContextException("The SharePointServiceLocator needs to run in a SharePoint context and have access to the SPFarm.");
            }

            // Retrieve the type mappings that are stored in config. 
            ServiceLocatorConfig serviceLocatorConfig = new ServiceLocatorConfig();
            IEnumerable<TypeMapping> configuredTypeMappings = serviceLocatorConfig.GetTypeMappings();

            // Create the factory that can configure and create the service locator
            // It's possible that the factory to be used has been changed in config. 
            IServiceLocatorFactory serviceLocatorFactory = GetServiceLocatorFactory(configuredTypeMappings);

            // Create the service locator and load it up with the default and configured type mappings
            IServiceLocator serviceLocator = serviceLocatorFactory.Create();
            serviceLocatorFactory.LoadTypeMappings(serviceLocator, GetDefaultTypeMappings());
            serviceLocatorFactory.LoadTypeMappings(serviceLocator, configuredTypeMappings);

            return serviceLocator;
        }

        private static IServiceLocatorFactory GetServiceLocatorFactory(IEnumerable<TypeMapping> configuredTypeMappings)
        {
            // Find configured factory. If it's there, creat it. 
            IServiceLocatorFactory factory = FindAndCreateConfiguredType<IServiceLocatorFactory>(configuredTypeMappings);

            // If there is no configured factory, then the ActivatingServiceLocatorFactory is the default one to use
            if (factory == null)
            {
                factory = new ActivatingServiceLocatorFactory();
            }

            return factory;
        }

        private static IEnumerable<TypeMapping> GetDefaultTypeMappings()
        {
            List<TypeMapping> defaultTypeMappings = new List<TypeMapping>();

            defaultTypeMappings.Add(TypeMapping.Create<ILogger, SharePointLogger>());
            defaultTypeMappings.Add(TypeMapping.Create<ITraceLogger, TraceLogger>());
            defaultTypeMappings.Add(TypeMapping.Create<IEventLogLogger, EventLogLogger>());
            defaultTypeMappings.Add(TypeMapping.Create<IHierarchicalConfig, HierarchicalConfig>());
            defaultTypeMappings.Add(TypeMapping.Create<IConfigManager, HierarchicalConfig>());
            defaultTypeMappings.Add(TypeMapping.Create<IServiceLocatorConfig, ServiceLocatorConfig>());

            return defaultTypeMappings;
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
        /// Replace the static instance of <see cref="Current"/> with a new service locator instance.
        /// </summary>
        /// <param name="newServiceLocator">The new service locator to use from now on. </param>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public static void ReplaceCurrentServiceLocator(IServiceLocator newServiceLocator)
        {
            EnsureCommonServiceLocatorCurrentFails();
            serviceLocatorInstance = newServiceLocator;
        }

        /// <summary>
        /// Reset the service locator back to the default service locator. 
        /// </summary>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public static void Reset()
        {
            ReplaceCurrentServiceLocator(null);
        }
    }
}
