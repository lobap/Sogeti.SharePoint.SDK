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
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.SPG.Common.ServiceLocation
{
    /// <summary>
    /// Class that represents a type mapping for the <see cref="IServiceLocator"/>.
    /// 
    /// A type mapping links an interface to an implementation. 
    /// </summary>
    [Serializable]
    public class TypeMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeMapping"/> class.
        /// </summary>
        public TypeMapping()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeMapping"/> class and populate it's values. 
        /// </summary>
        /// <param name="typeFrom">The type that's used to index the type mapping</param>
        /// <param name="typeTo">The type that the typeFrom is mapped to. </param>
        /// <param name="key">The key used to index the type mapping.</param>
        public TypeMapping(Type typeFrom, Type typeTo, string key)
        {
            if (typeTo.IsAbstract)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "{0} must be a non-abstract type with a parameterless constructor", typeTo.Name));
            else if (!typeTo.IsSubclassOf(typeFrom) && typeTo.GetInterface(typeFrom.Name) == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "{0} does not have an implicit conversion defined for {1}", typeTo.Name, typeFrom.Name));
            }

            FromAssembly = typeFrom.Assembly.FullName;
            FromType = typeFrom.AssemblyQualifiedName;
            ToAssembly = typeTo.Assembly.FullName;
            ToType = typeTo.AssemblyQualifiedName;
            Key = key;
        }

        /// <summary>
        /// The name of the assembly that the 'from' type is located in. 
        /// </summary>
        public string FromAssembly { get; set; }

        /// <summary>
        /// The assembly qualified typename of the 'from' type. 
        /// </summary>
        public string FromType { get; set; }

        /// <summary>
        /// The assembly qualified typename of the 'to' type. 
        /// </summary>
        public string ToType { get; set; }

        /// <summary>
        /// The name of the Assembly that the 'to' type is located in. 
        /// </summary>
        public string ToAssembly { get; set; }

        /// <summary>
        /// A key that can differentiate several type mappings for the same fromtype. If you don't specify
        /// a key, null will be used. 
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Determines how the objects for this type mapping should be instantiated. As a singleton or a new
        /// instance each time. 
        /// </summary>
        public InstantiationType InstantiationType { get; set; }

        /// <summary>
        /// Helper method to create type mapping objects more easily. Creates a type mapping with
        /// key (null) and instantiation type NewInstanceForEachRequest. 
        /// </summary>
        /// <typeparam name="TFrom">the from type</typeparam>
        /// <typeparam name="TTo">the target type</typeparam>
        /// <returns>the created type mapping</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static TypeMapping Create<TFrom, TTo>()
            where TTo : TFrom, new()
        {
            return Create<TFrom, TTo>(null, InstantiationType.NewInstanceForEachRequest);
        }

        /// <summary>
        /// Helper methods to create type mapping objects more easily. Creates type mapping with
        /// specified key and instantiation type NewInstanceForEachRequest.
        /// </summary>
        /// <typeparam name="TFrom">the from type. </typeparam>
        /// <typeparam name="TTo">The target type</typeparam>
        /// <param name="key">The key to use. </param>
        /// <returns>the created type mapping</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static TypeMapping Create<TFrom, TTo>(string key)
            where TTo : TFrom, new()
        {
            return Create<TFrom, TTo>(key, InstantiationType.NewInstanceForEachRequest);
        }

        /// <summary>
        /// Helper method to create type mapping objects more easily. Creates a type mapping with
        /// key (null) and specified instantiation type. 
        /// </summary>
        /// <typeparam name="TFrom">the from type</typeparam>
        /// <typeparam name="TTo">the target type</typeparam>
        /// <param name="instantiate">How to instantiate the types from this type mapping.</param>
        /// <returns>the created type mapping</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static TypeMapping Create<TFrom, TTo>(InstantiationType instantiate)
            where TTo : TFrom, new()
        {
            return Create<TFrom, TTo>(null, instantiate);
        }

        /// <summary>
        /// Helper method to create type mapping objects more easily. Creates a type mapping with
        /// specified key and specified instantiation type. 
        /// </summary>
        /// <typeparam name="TFrom">the from type</typeparam>
        /// <typeparam name="TTo">the target type</typeparam>
        /// <param name="instantiate">How to instantiate the types from this type mapping.</param>
        /// <param name="key">The key to use. </param>
        /// <returns>the created type mapping</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static TypeMapping Create<TFrom, TTo>(string key, InstantiationType instantiate)
            where TTo : TFrom, new()
        {
            return new TypeMapping(typeof(TFrom), typeof(TTo), key) { InstantiationType = instantiate };
        }
    }
}
