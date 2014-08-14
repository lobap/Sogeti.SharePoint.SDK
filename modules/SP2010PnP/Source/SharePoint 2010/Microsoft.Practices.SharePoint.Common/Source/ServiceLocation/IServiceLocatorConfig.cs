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
using System.Security.Permissions;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint;

namespace Microsoft.Practices.SharePoint.Common.ServiceLocation
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
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void RegisterTypeMapping<TFrom, TTo>() where TTo : TFrom, new();


        /// <summary>
        /// Register a type mapping between TFrom and TTo, with a specified key.
        /// </summary>
        /// <typeparam name="TFrom">The type that will be used to identify the type mapping.</typeparam>
        /// <typeparam name="TTo">The type that will be returned when using the type mapping.</typeparam>
        /// <param name="key">The key of the type mapping.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void RegisterTypeMapping<TFrom, TTo>(string key) where TTo : TFrom, new();

        /// <summary>
        /// Remove all type mappings for this type.
        /// </summary>
        /// <typeparam name="T">The type to remove type mappings for</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void RemoveTypeMappings<T>();

        /// <summary>
        /// Remove a type mapping with specified key. Use (null) to remove a type mapping that was registered without parameter. 
        /// </summary>
        /// <typeparam name="T">The type to remove type mappings for.</typeparam>
        /// <param name="key">The key to find type mappings for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void RemoveTypeMapping<T>(string key);

        /// <summary>
        /// Gets a list of all of the type mappings configured.
        /// </summary>
        /// <returns></returns>
        List<TypeMapping> GetTypeMappings();

        /// <summary>
        /// Gets the time that configuration was last updated.
        /// </summary>
        DateTime? LastUpdate { get; }

        /// <summary>
        /// Gets and sets the site for managing configuration at the site level.
        /// Set this property if you want to manage configuration for a site.
        /// </summary>
        SPSite Site {get; set;}

        /// <summary>
        /// Gets the cache interval in seconds for a site level service locator.  When the interval
        /// expires the configuration is read back for the locator and if changed then service locator
        /// is reloaded.
        /// </summary>
        /// <returns>the duration in seconds to cache a site service locator instance</returns>
        int GetSiteCacheInterval();

        /// <summary>
        /// Sets the site cache interval for a service locator
        /// </summary>
        /// <param name="interval">The interval in seconds to set</param>
        void SetSiteCacheInterval(int interval);
    }
}