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
using System.Reflection;

namespace Contoso.TrainingManagement
{
    /// <summary>
    /// The ServiceLocator class caches and centralizes the lookup of various services
    /// used throughout the Training Management application.
    /// </summary>
    public class ServiceLocator
    {
        #region Private Members

        private static readonly ServiceLocator instance = new ServiceLocator();
        private readonly Dictionary<Type, Type> typeMappings = new Dictionary<Type, Type>();
        private readonly Object lockObject = new Object();

        #endregion

        #region Constructor

        static ServiceLocator()
        {
            instance.Reset();
        }

        private ServiceLocator()
        {            
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns the count of type mappings in the service locator
        /// </summary>
        public int Count
        {
            get { return this.typeMappings.Count; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the service registered for a given type.
        /// </summary>
        /// <typeparam name="T">Type of the service to return.</typeparam>
        /// <returns>Service of a given type.</returns>
        public T Get<T>()
        {
            Type concreteType = this.typeMappings[typeof(T)];
            return (T)Activator.CreateInstance(concreteType);
        }

        /// <summary>
        /// Gets the current instance of the Service locator. If no Service Locator currently exists, one is created.
        /// </summary>
        /// <returns>The current Service Locator instance.</returns>
        public static ServiceLocator GetInstance()
        {
            return instance;
        }

        /// <summary>
        /// Resets the current Service Locator instance with default registrations.
        /// </summary>
        public void Reset()
        {
            instance.typeMappings.Clear();
            ICollection<TypeMetadata> typeMetaDataList = ConfigurationService.GetInstance().GetTypeMappings();

            foreach (TypeMetadata typeMetaData in typeMetaDataList)
            {
                Assembly contractAssembly = Assembly.Load(typeMetaData.ContractAssemblyName);
                Assembly concreteAssembly = Assembly.Load(typeMetaData.ConcreteAssemblyName);
                this.Register(contractAssembly.GetType(typeMetaData.ContractTypeName), concreteAssembly.GetType(typeMetaData.ConcreteTypeName));
            }
        }

        /// <summary>
        /// Clears the current Service Locator instance of a registrations.
        /// </summary>
        public static void Clear()
        {
            instance.typeMappings.Clear();
        }

        /// <summary>
        /// Registers a service
        /// </summary>
        /// <param name="contractType">type information for the service contract</param>
        /// <param name="concreteType">type information for the concrete service</param>
        public void Register(Type contractType, Type concreteType)
        {
            lock ( lockObject )
            {
                if ( instance.typeMappings.ContainsKey(contractType) )
                {
                    instance.typeMappings.Remove(contractType);
                }
                instance.typeMappings.Add(contractType, concreteType);
            }
        }

        /// <summary>
        /// Registers a service
        /// </summary>
        /// <typeparam name="T">the type of the serivce contract</typeparam>
        /// <param name="concreteType">type information for the concrete service</param>
        public void Register<T>(Type concreteType)
        {
            Register(typeof(T), concreteType);
        }

        #endregion
    }
}
