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
using System.Collections.Generic;
using System.Security.Permissions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SPG.Common.ServiceLocation
{
    /// <summary>
    /// Class that reads and writes the <see cref="SharePointServiceLocator"/>'s configration in hierarchical config. It uses the 
    /// <see cref="HierarchicalConfig"/> to read and write values into the Farm level of hierarchical config. 
    /// </summary>
    [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
    public class ServiceLocatorConfig : IServiceLocatorConfig
    {
        private readonly IConfigManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocatorConfig"/> class that reads and writes values into 
        /// a config manager that reads and writes to the SPFarm.
        /// </summary>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public ServiceLocatorConfig()
        {
            this.manager = new HierarchicalConfig();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocatorConfig"/> class that stores it's config in the specified
        /// IHierarchicalConfig. 
        /// </summary>
        /// <param name="manager">The config manager to be used with this instance.</param>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public ServiceLocatorConfig(IConfigManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Register a type mapping between two types. When asking for a TFrom, an instance of TTo is returned.
        /// </summary>
        /// <typeparam name="TFrom">The type that can be requested.</typeparam>
        /// <typeparam name="TTo">The type of object that should be returned when asking for a type.</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
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
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter"), SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public virtual void RegisterTypeMapping<TFrom, TTo>(string key, InstantiationType instantiationType)
            where TTo : TFrom, new()
        {
            List<TypeMapping> typeMappings = GetTypeMappingsList();

            TypeMapping newTypeMapping = TypeMapping.Create<TFrom, TTo>(key, instantiationType);

            RemovePreviousMappingsForFromType(typeMappings, newTypeMapping);
            typeMappings.Add(newTypeMapping);

            SetTypeMappingsList(typeMappings);
        }

        private void SetTypeMappingsList(List<TypeMapping> typeMappings)
        {
            manager.SetInPropertyBag(GetConfigKey(), typeMappings, SPFarm.Local);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void RemovePreviousMappingsForFromType(List<TypeMapping> mappings, TypeMapping newTypeMapping)
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

        /// <summary>
        /// Returns the list of type mappings that's stored in the config. 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public IEnumerable<TypeMapping> GetTypeMappings()
        {
            return GetTypeMappingsList();
        }

        private List<TypeMapping> GetTypeMappingsList()
        {
            if (!manager.ContainsKeyInPropertyBag(GetConfigKey(), SPFarm.Local))
                return new List<TypeMapping>();

            List<TypeMapping> typeMappings = manager.GetFromPropertyBag<List<TypeMapping>>(GetConfigKey(), SPFarm.Local);

            if (typeMappings == null)
            {
                return new List<TypeMapping>();
            }

            return typeMappings;
        }

        /// <summary>
        /// The config key used to store the config values. 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        protected virtual string GetConfigKey()
        {
            return "Microsoft.Practices.SPG.Common.TypeMappings";
        }


        /// <summary>
        /// Remove all type mappings for a specified type. 
        /// </summary>
        /// <typeparam name="T">The type to remove type mappings for. </typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter"), SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public virtual void RemoveTypeMappings<T>()
        {
            List<TypeMapping> typeMappings = GetTypeMappingsList();

            foreach(TypeMapping mapping in typeMappings.ToArray())
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter"), SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public virtual void RemoveTypeMapping<T>(string key)
        {
            List<TypeMapping> typeMappings = GetTypeMappingsList();

            foreach(TypeMapping mapping in typeMappings.ToArray())
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
        /// <param name="mappingToRemove"></param>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public virtual void RemoveTypeMapping(TypeMapping mappingToRemove)
        {
            List<TypeMapping> typeMappings = GetTypeMappingsList();
            foreach (TypeMapping mapping in typeMappings.ToArray())
            {
                if (mapping == mappingToRemove)
                {
                    typeMappings.Remove(mapping);
                }
            }

            SetTypeMappingsList(typeMappings);
        }


    }
}
