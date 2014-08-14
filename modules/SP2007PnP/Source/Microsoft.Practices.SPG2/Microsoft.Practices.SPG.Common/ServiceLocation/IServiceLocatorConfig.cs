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
using System.Security.Permissions;
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SPG.Common.ServiceLocation
{
    /// <summary>
    /// Interface for a class that can read and write type mappings into config. 
    /// </summary>
    public interface IServiceLocatorConfig
    {
        /// <summary>
        /// Register a type mapping between TFrom and TTo, with (null) as a key.
        /// </summary>
        /// <typeparam name="TFrom">The type that will be used to identify the type mapping.</typeparam>
        /// <typeparam name="TTo">The type that will be returned when using the type mapping.</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void RegisterTypeMapping<TFrom, TTo>() where TTo : TFrom, new();


        /// <summary>
        /// Register a type mapping between TFrom and TTo, with a specified key.
        /// </summary>
        /// <typeparam name="TFrom">The type that will be used to identify the type mapping.</typeparam>
        /// <typeparam name="TTo">The type that will be returned when using the type mapping.</typeparam>
        /// <param name="key">The key of the type mapping.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        void RegisterTypeMapping<TFrom, TTo>(string key) where TTo : TFrom, new();

        /// <summary>
        /// Remove all type mappings for this type.
        /// </summary>
        /// <typeparam name="T">The type to remove type mappings for</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        void RemoveTypeMappings<T>();

        /// <summary>
        /// Remove a type mapping with specified key. Use (null) to remove a type mapping that was registered without parameter. 
        /// </summary>
        /// <typeparam name="T">The type to remove type mappings for.</typeparam>
        /// <param name="key">The key to find type mappings for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        void RemoveTypeMapping<T>(string key);
    }
}