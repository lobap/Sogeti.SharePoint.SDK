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
using System.Reflection;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.SPG.Common.ServiceLocation
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
        /// This method will do the actual work of resolving the requested service instance.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <param name="key">Name of registered service you want. May be null.</param>
        /// <returns>The requested service instance.</returns>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (!typeMappingsDictionary.ContainsKey(serviceType.AssemblyQualifiedName))
                throw BuildNotRegisteredException(serviceType, key);

            Dictionary<string, TypeMapping> mappingsForType = typeMappingsDictionary[serviceType.AssemblyQualifiedName];

            if (!mappingsForType.ContainsKey(PreventNull(key)))
                throw BuildNotRegisteredException(serviceType, key);

            TypeMapping typeMapping = mappingsForType[PreventNull(key)];

            if (typeMapping.InstantiationType == InstantiationType.AsSingleton)
            {
                return GetSingleton(typeMapping);
            }

            return CreateInstanceFromTypeMapping(typeMapping);
        }

        private object GetSingleton(TypeMapping typeMapping)
        {
            Dictionary<string, object> singletonValueDictionary = GetSingletonValueDictionary(typeMapping);

            if (!singletonValueDictionary.ContainsKey(PreventNull(typeMapping.Key)))
            {
                lock (syncRoot)
                {
                    if (!singletonValueDictionary.ContainsKey(PreventNull(typeMapping.Key)))
                    {
                        singletonValueDictionary[PreventNull(typeMapping.Key)] = CreateInstanceFromTypeMapping(typeMapping);
                    }    
                }
            }

            return singletonValueDictionary[PreventNull(typeMapping.Key)];
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
            return new ActivationException(String.Format(CultureInfo.CurrentCulture, "No type mapping was registered for type '{0}' and key '{1}'.", serviceType.Name, key));
        }

        /// <summary>
        /// Create an instance of an object from a type mapping. An instance of type <see cref="TypeMapping.ToType"/> will
        /// be instantiated. 
        /// </summary>
        /// <param natme="typeMapping">The type mapping to use to create the instance. </param>
        /// <returns>The created instance. </returns>
        public static object CreateInstanceFromTypeMapping(TypeMapping typeMapping)
        {
            Assembly.Load(typeMapping.ToAssembly);
            Type typeToCreate = Type.GetType(typeMapping.ToType);
            
            return Activator.CreateInstance(typeToCreate);
        }


        /// <summary>
        /// This method will do the actual work of resolving all the requested service instances.
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
            return RegisterTypeMapping(new TypeMapping(fromType, toType, key) {InstantiationType = instantiationType});
        }


        /// <summary>
        /// Registers a type mapping.
        /// </summary>
        /// <param name="mapping">The mapping to register.</param>
        /// <returns>The service locator to make it easier to add multiple type mappings</returns>
        public ActivatingServiceLocator RegisterTypeMapping(TypeMapping mapping)
        {
            if (!this.typeMappingsDictionary.ContainsKey(mapping.FromType))
            {
                this.typeMappingsDictionary[mapping.FromType] = new Dictionary<string, TypeMapping>();
            }

            Dictionary<string, TypeMapping> mappingsForType = this.typeMappingsDictionary[mapping.FromType];

            mappingsForType[PreventNull(mapping.Key)] = mapping;
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
            return this.typeMappingsDictionary.ContainsKey(typeof(TService).AssemblyQualifiedName);
        }
    }
}
