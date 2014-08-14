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
using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using System.Collections;

namespace Microsoft.Practices.SharePoint.Common.ServiceLocation
{
    /// <summary>
    /// Class that provides basic service location functionality to an application. It uses System.Activator to create new instances.
    /// You can use this class to decouple classes from each other. A class only needs to know the interface of
    /// the services it needs to consume. This class will find and return the corresponding implementation for
    /// that interface.
    /// It implements the IServiceLocator interface as defined in the Common Service Locator project.
    /// </summary>
    public class ActivatingServiceLocator : ServiceLocatorImplBase
    {
        private readonly object syncRoot = new object();
        private Dictionary<string, Dictionary<string, TypeMapping>> typeMappingsDictionary = new Dictionary<string, Dictionary<string, TypeMapping>>();
        private Dictionary<string, Dictionary<string, object>> singletonsDictionary = new Dictionary<string, Dictionary<string, object>>();

        /// <summary>
        /// The event that is signalled when a new type mapping is registered.
        /// </summary>
        public event EventHandler<TypeMappingChangedArgs> MappingRegisteredEvent;

        /// <summary>
        /// This method will do the actual work of resolving the requested service instance.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <param name="key">Name of registered service you want. May be null.</param>
        /// <returns>The requested service instance.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        protected override object DoGetInstance(Type serviceType, string key)
        {
            Validation.ArgumentNotNull(serviceType, "serviceType");
            string nonNullKey = PreventNull(key);

            if (!typeMappingsDictionary.ContainsKey(serviceType.AssemblyQualifiedName))
                throw BuildNotRegisteredException(serviceType, key);

            Dictionary<string, TypeMapping> mappingsForType = typeMappingsDictionary[serviceType.AssemblyQualifiedName];

            if (!mappingsForType.ContainsKey(nonNullKey))
                throw BuildNotRegisteredException(serviceType, key);

            TypeMapping typeMapping = mappingsForType[nonNullKey];

            if (typeMapping.InstantiationType == InstantiationType.AsSingleton)
            {
                return GetSingleton(typeMapping);
            }

            return CreateInstanceFromTypeMapping(typeMapping);
        }

        private object GetSingleton(TypeMapping typeMapping)
        {
            Dictionary<string, object> singletonValueDictionary = GetSingletonValueDictionary(typeMapping);

            if (!singletonValueDictionary.ContainsKey(typeMapping.GetNonNullKey()))
            {
                lock (syncRoot)
                {
                    if (!singletonValueDictionary.ContainsKey(typeMapping.GetNonNullKey()))
                    {
                        singletonValueDictionary[typeMapping.GetNonNullKey()] = CreateInstanceFromTypeMapping(typeMapping);
                    }    
                }
            }

            return singletonValueDictionary[typeMapping.GetNonNullKey()];
        }

        private Dictionary<string, object> GetSingletonValueDictionary(TypeMapping typeMapping)
        {
            if (!singletonsDictionary.ContainsKey(typeMapping.FromType))
            {
                lock(syncRoot)
                {
                    if (!singletonsDictionary.ContainsKey(typeMapping.FromType))
                    {
                        singletonsDictionary[typeMapping.FromType] = new Dictionary<string, object>();
                    }
                }
            }

            return singletonsDictionary[typeMapping.FromType];
        }

        private static string PreventNull(string value)
        {
            if (value == null)
                return string.Empty;
             
            return value;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private ActivationException BuildNotRegisteredException(Type serviceType, string key)
        {
            return new ActivationException(
                String.Format(CultureInfo.CurrentCulture, 
                Properties.Resources.TypeMappingNotRegistered, serviceType.Name, key));
        }

        /// <summary>
        /// Create an instance of an object from a type mapping. An instance of type <see cref="TypeMapping.ToType"/> will
        /// be instantiated. 
        /// </summary>
        /// <param natme="typeMapping">The type mapping to use to create the instance. </param>
        /// <returns>The created instance. </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public static object CreateInstanceFromTypeMapping(TypeMapping typeMapping)
        {
            Validation.ArgumentNotNull(typeMapping, "typeMapping");

            Assembly.Load(typeMapping.ToAssembly);
            Type typeToCreate = Type.GetType(typeMapping.ToType);
            
            return Activator.CreateInstance(typeToCreate);
        }


        /// <summary>
        /// This method will do the actual work of resolving all the requested service instances for a particular type.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>Sequence of service instance objects.</returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            if (!typeMappingsDictionary.ContainsKey(serviceType.AssemblyQualifiedName))
                yield break;

            Dictionary<string, TypeMapping> mappingsForType = typeMappingsDictionary[serviceType.AssemblyQualifiedName];

            foreach (TypeMapping typeMapping in mappingsForType.Values)
            {
                if (typeMapping.InstantiationType == InstantiationType.AsSingleton)
                {
                    yield return GetSingleton(typeMapping);
                }
                else
                {
                    yield return CreateInstanceFromTypeMapping(typeMapping);
                }
            }
        }

        /// <summary>
        /// Register a type mapping between two types. When asking for a TFrom, an instance of TTo is returned.
        /// </summary>
        /// <typeparam name="TFrom">The type that can be requested.</typeparam>
        /// <typeparam name="TTo">The type of object that should be returned when asking for a type.</typeparam>
        /// <returns>The service locator to make it easier to add multiple type mappings</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Normal service locator method")]
        public ActivatingServiceLocator RegisterTypeMapping<TFrom, TTo>()
            where TTo : TFrom, new()
        {
            return this.RegisterTypeMapping(typeof(TFrom), typeof(TTo), null, InstantiationType.NewInstanceForEachRequest);
        }

        /// <summary>
        /// Register a type mapping between TFrom and TTo with key (null). When <see cref="IServiceLocator.GetInstance(System.Type)"/> with
        /// parameter TFrom is called, an instance of type TTO is returned. 
        /// </summary>
        /// <typeparam name="TFrom">The type to register type mappings for. </typeparam>
        /// <typeparam name="TTo">The type to create if <see cref="IServiceLocator.GetInstance(System.Type)"/> is called with TFrom. </typeparam>
        /// <param name="instantiationType">Determines how the type should be created. </param>
        /// <returns>The service locator to make it easier to add multiple type mappings</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public ActivatingServiceLocator RegisterTypeMapping<TFrom, TTo>(InstantiationType instantiationType)
            where TTo: TFrom, new()
        {
            return RegisterTypeMapping(typeof (TFrom), typeof (TTo), null, instantiationType);
        }

        /// <summary>
        /// Register a type mapping between TFrom and TTo with specified key. When <see cref="IServiceLocator.GetInstance(System.Type)"/> with
        /// parameter TFrom is called, an instance of type TTO is returned. 
        /// </summary>
        /// <typeparam name="TFrom">The type to register type mappings for. </typeparam>
        /// <typeparam name="TTo">The type to create if <see cref="IServiceLocator.GetInstance(System.Type)"/> is called with TFrom. </typeparam>
        /// <param name="instantiationType">Determines how the type should be created. </param>
        /// <param name="key">The key that's used to store the type mapping.</param>
        /// <returns>The service locator to make it easier to add multiple type mappings</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public ActivatingServiceLocator RegisterTypeMapping<TFrom, TTo>(string key, InstantiationType instantiationType)
            where TTo : TFrom, new()
        {
            return this.RegisterTypeMapping(typeof(TFrom), typeof(TTo), key, instantiationType);
        }

        /// <summary>
        /// Register a type mapping between TFrom and TTo with specified key. When <see cref="IServiceLocator.GetInstance(System.Type)"/> with
        /// parameter TFrom is called, an instance of type TTO is returned. 
        /// </summary>
        /// <typeparam name="TFrom">The type to register type mappings for. </typeparam>
        /// <typeparam name="TTo">The type to create if <see cref="IServiceLocator.GetInstance(System.Type)"/> is called with TFrom. </typeparam>
        /// <param name="key">The key that's used to store the type mapping.</param>
        /// <returns>The service locator to make it easier to add multiple type mappings</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public ActivatingServiceLocator RegisterTypeMapping<TFrom, TTo>(string key)
            where TTo : TFrom, new()
        {
            return this.RegisterTypeMapping(typeof(TFrom), typeof(TTo), key, InstantiationType.NewInstanceForEachRequest);
        }

        /// <summary>
        /// Register a type mapping between fromType and toType with specified key. When <see cref="IServiceLocator.GetInstance(System.Type)"/> with
        /// parameter TFrom is called, an instance of type TTO is returned.
        /// </summary>
        /// <param name="fromType">The type to register type mappings for. </param>
        /// <param name="toType">The type to create if <see cref="IServiceLocator.GetInstance(System.Type)"/> is called with fromType. </param>
        /// <returns>The service locator to make it easier to add multiple type mappings</returns>
        public ActivatingServiceLocator RegisterTypeMapping(Type fromType, Type toType)
        {
            return this.RegisterTypeMapping(fromType, toType, null, InstantiationType.NewInstanceForEachRequest);
        }

        /// <summary>
        /// Register a type mapping between fromType and toType with specified key. When <see cref="IServiceLocator.GetInstance(System.Type)"/> with
        /// parameter TFrom is called, an instance of type TTO is returned.
        /// </summary>
        /// <param name="fromType">The type to register type mappings for. </param>
        /// <param name="toType">The type to create if <see cref="IServiceLocator.GetInstance(System.Type)"/> is called with fromType. </param>
        /// <param name="key">The key that's used to store the type mapping.</param>
        /// <param name="instantiationType">Determines how the type should be created. </param>
        /// <returns>The service locator to make it easier to add multiple type mappings</returns>
        public ActivatingServiceLocator RegisterTypeMapping(Type fromType, Type toType, string key, InstantiationType instantiationType)
        {
            Validation.ArgumentNotNull(fromType, "fromType");
            Validation.ArgumentNotNull(toType, "toType");
            return RegisterTypeMapping(new TypeMapping(fromType, toType, key) {InstantiationType = instantiationType});
        }


        /// <summary>
        /// Registers a type mapping.
        /// </summary>
        /// <param name="mapping">The mapping to register.</param>
        /// <returns>The service locator to make it easier to add multiple type mappings</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public ActivatingServiceLocator RegisterTypeMapping(TypeMapping mapping)
        {
            TypeMapping.ValidateMapping(mapping);

            if (!this.typeMappingsDictionary.ContainsKey(mapping.FromType))
            {
                this.typeMappingsDictionary[mapping.FromType] = new Dictionary<string, TypeMapping>();
            }

            Dictionary<string, TypeMapping> mappingsForType = this.typeMappingsDictionary[mapping.FromType];

            mappingsForType[mapping.GetNonNullKey()] = mapping;

            EventHandler<TypeMappingChangedArgs> handler = this.MappingRegisteredEvent;

            if (handler != null)
            {
                TypeMappingChangedArgs args = new TypeMappingChangedArgs();
                args.Mapping = mapping;
                handler(this, args);
            }

            return this;
        }

        /// <summary>
        /// Determines if a type mapping is registered. 
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>True if the type mapping is registered. Else false. </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public bool IsTypeRegistered<TService>()
        {
            return IsTypeRegistered(typeof(TService));
        }

        /// <summary>
        /// Determines if a type mapping is registered. 
        /// </summary>
        /// <param name="t">The type of the service.</param>
        /// <returns>True if the type mapping is registered. Else false. </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public bool IsTypeRegistered(Type t)
        {
            return this.typeMappingsDictionary.ContainsKey(t.AssemblyQualifiedName);
        }

        /// <summary>
        /// Determines if a type mapping is registered. 
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="key">The key for the type mapping</param>
        /// <returns>True if the type mapping is registered. Else false. </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public bool IsTypeRegistered<TService>(string key)
        {
            return IsTypeRegistered(typeof(TService), key);
        }

        /// <summary>
        /// Determines if a type mapping is registered. 
        /// </summary>
        /// <param name="t">The type of the service.</param>
        /// <param name="key">The key for the type mapping</param>
        /// <returns>True if the type mapping is registered. Else false. </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public bool IsTypeRegistered(Type t, string key)
        {
            if (typeMappingsDictionary.ContainsKey(t.AssemblyQualifiedName))
            {
                Dictionary<string, TypeMapping> mappingsForType = typeMappingsDictionary[t.AssemblyQualifiedName];

                if (mappingsForType.ContainsKey(PreventNull(key)))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the list of type mappings the service locator instance manages.
        /// </summary>
        /// <returns>An enumeration of the type mappings the service locator posesses</returns>
        internal IEnumerable<TypeMapping> GetTypeMappings()
        {
            return this.typeMappingsDictionary.Values.SelectMany(tdict => tdict.Values).ToList();
        }


        private static void RemoveMappings(List<TypeMapping> list, IEnumerable<TypeMapping> toRemove)
        {
            //select all of the mappings in teh collection from the list that are in the toRemove list, and remove
            //them from the list.
            foreach (int index in
               toRemove.Select(mapping => list.FindIndex((t) => t.Equals(mapping))).Where(num => num >= 0)
               )
            {
                list.RemoveAt(index);
            }           
        }

        private void RegisterMappings(IEnumerable<TypeMapping> mappings)
        {
            foreach (var mapping in mappings)
            {
                this.RegisterTypeMapping(mapping);
            }
        }

        /// <summary>
        /// Refreshes the services for a service locator.  The default configuration and farm configuration
        /// settings are merged with the site config settings.  Programmatically added mappings are preserved.
        /// </summary>
        /// <param name="defaultConfig">The default configuration type mappings</param>
        /// <param name="farmConfig">The farm level configured type mappings</param>
        /// <param name="siteConfig">The previous site level configured type mappings</param>
        /// <param name="newSiteMappings">The new site level type mappings</param>
        internal void Refresh(IEnumerable<TypeMapping> defaultConfig, IEnumerable<TypeMapping> farmConfig, 
            IEnumerable<TypeMapping> siteConfig, IEnumerable<TypeMapping> newSiteMappings)
        {
            Validation.ArgumentNotNull(siteConfig, "siteConfig");
            Validation.ArgumentNotNull(newSiteMappings, "newSiteMappings");

            //Start with all type mappings, and remove configured type mappings.  The remaining type mappings
            //were added programmatically.
            var programmaticMappings = GetTypeMappings() as List<TypeMapping> ?? new List<TypeMapping>();

            //Remove all of the configured type mappings.
            RemoveMappings(programmaticMappings, defaultConfig);
            RemoveMappings(programmaticMappings, farmConfig);
            RemoveMappings(programmaticMappings, siteConfig);

            //Clear the dictionary, and re-add the mappings using the new site type mappings.  Programatically added
            //type mappings take highest precendence.
            this.typeMappingsDictionary.Clear();

            RegisterMappings(defaultConfig);
            RegisterMappings(farmConfig);
            RegisterMappings(newSiteMappings);
            RegisterMappings(programmaticMappings);
        }
    }
}
