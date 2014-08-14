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

namespace Microsoft.Practices.SPG.Common.Configuration
{
    /// <summary>
    /// Allows you to read config settings in a hierarchical way. You can look for a config setting in a property bag
    /// at a certain level, and it will look up the hierarchy for the first location where that config setting is located. 
    /// 
    /// Using this interface, you can only look in your current context. That means: in your Current SPWeb (default)
    /// , current SPSite, current SPWebApplication and current SPFarm. 
    /// 
    /// This interface is only for reading config settings and is optimized for consumption from within SharePoint pages,
    /// Web parts, list item event receivers, etc. It needs the SharePoint context to function. 
    /// 
    /// If you need to write settings into config, use the <see cref="IConfigManager"/> class. 
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    public interface IHierarchicalConfig
    {
        /// <summary>
        /// Read a config value based on the key, from the default PropertyBag: SPContext.Current.Web
        /// 
        /// If it can't find it in the default property bag it will look at it's parents.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to read. </typeparam>
        /// <param name="key">The key associated with the config value</param>
        /// <returns>The value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1004:GenericMethodsShouldProvideTypeParameter"),
         SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        TValue GetByKey<TValue>(string key);

        /// <summary>
        /// Read a config value based on the key, from the specified config level in the current context.
        /// 
        /// If the value cannot be found at the specified level, it will look recursively up at it's parent. 
        /// </summary>
        /// <typeparam name="TValue">The type of the value to read.</typeparam>
        /// <param name="key">The key associated with the config value</param>
        /// <param name="level">The config level to start looking in.
        /// For example, <see cref="ConfigLevel.CurrentSPWebApplication"/> means that it's looking at the current 'SPWebApplication'
        /// and above.
        ///  </param>
        /// <returns>The value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1004:GenericMethodsShouldProvideTypeParameter"),
         SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        TValue GetByKey<TValue>(string key, ConfigLevel level);

        /// <summary>
        /// Determines if a config value with the specified key can be found in config. 
        /// Will recursively look up to parent property bags to find the key. 
        /// </summary>
        /// <param name="key">the specified key</param>
        /// <returns>
        /// 	<c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        bool ContainsKey(string key);

        /// <summary>
        /// Determines if a config value with the specified key can be found in the hierarchical config at 
        /// the specified level in the current context or above.  Will recursively look up to parent property bags 
        /// to find the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="level">The level to start looking in.</param>
        /// <returns>
        /// 	<c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        bool ContainsKey(string key, ConfigLevel level);
    }
}