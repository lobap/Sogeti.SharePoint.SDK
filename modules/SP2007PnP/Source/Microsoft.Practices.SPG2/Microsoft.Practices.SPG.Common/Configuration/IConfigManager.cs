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
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SPG.Common.Configuration
{
    /// <summary>
    /// The IHierarchicalConfigManager interface allows you to read and write config settings in several SharePoint 
    /// property bags: the SPFarm, the SPWebApplication, the SPSite and the SPWeb. 
    /// 
    /// This interface is closely related to the IHierarchicalConfig class. This class allows you
    /// to read and write config settings at specific levels. The HierarchicalConfig allows you to read config settings
    /// from a particular level and above. 
    /// 
    /// The IConfigManager is designed to be used from a Feature Receiver. To write in a Farm or WebApplication, you need
    /// the correct privileges.  
    /// </summary>
    public interface IConfigManager
    {
        /// <summary>
        /// Remove a config value from a specified location. This method will not look up in parent property bags. 
        /// </summary>
        /// <param name="key">The configsetting to remove</param>
        /// <param name="propertyBag">The property bag to remove it from</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void RemoveKeyFromPropertyBag(string key, SPFarm propertyBag);

        /// <summary>
        /// Remove a config value from a specified location. This method will not look up in parent property bags. 
        /// </summary>
        /// <param name="key">The configsetting to remove</param>
        /// <param name="propertyBag">The property bag to remove it from</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void RemoveKeyFromPropertyBag(string key, SPSite propertyBag);

        /// <summary>
        /// Remove a config value from a specified location. This method will not look up in parent property bags. 
        /// </summary>
        /// <param name="key">The configsetting to remove</param>
        /// <param name="propertyBag">The property bag to remove it from</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void RemoveKeyFromPropertyBag(string key, SPWebApplication propertyBag);

        /// <summary>
        /// Remove a config value from a specified location. This method will not look up in parent property bags. 
        /// </summary>
        /// <param name="key">The configsetting to remove</param>
        /// <param name="propertyBag">The property bag to remove it from</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void RemoveKeyFromPropertyBag(string key, SPWeb propertyBag);

        /// <summary>
        /// See if a particular property bag contains a config setting with that key. 
        /// This method will not look up in parent property bags. 
        /// </summary>
        /// <param name="key">The key to use to find the config setting</param>
        /// <param name="propertyBag">The property bag to look in</param>
        /// <returns>True if the PropertyBag is found, else false</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        bool ContainsKeyInPropertyBag(string key, SPFarm propertyBag);

        /// <summary>
        /// See if a particular property bag contains a config setting with that key. 
        /// This method will not look up in parent property bags. 
        /// </summary>
        /// <param name="key">The key to use to find the config setting</param>
        /// <param name="propertyBag">The property bag to look in</param>
        /// <returns>True if the PropertyBag is found, else false</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        bool ContainsKeyInPropertyBag(string key, SPSite propertyBag);

        /// <summary>
        /// See if a particular property bag contains a config setting with that key. 
        /// This method will not look up in parent property bags. 
        /// </summary>
        /// <param name="key">The key to use to find the config setting</param>
        /// <param name="propertyBag">The property bag to look in</param>
        /// <returns>True if the PropertyBag is found, else false</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        bool ContainsKeyInPropertyBag(string key, SPWebApplication propertyBag);

        /// <summary>
        /// See if a particular property bag contains a config setting with that key. 
        /// This method will not look up in parent property bags. 
        /// </summary>
        /// <param name="key">The key to use to find the config setting</param>
        /// <param name="propertyBag">The property bag to look in</param>
        /// <returns>True if the PropertyBag is found, else false</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        bool ContainsKeyInPropertyBag(string key, SPWeb propertyBag);

        /// <summary>
        /// Sets a config value for a specific key in the specified property bag
        /// </summary>
        /// <param name="key">The key for this config setting</param>
        /// <param name="value">The value to give this config setting</param>
        /// <param name="propertyBag">The property bag to store the setting in</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void SetInPropertyBag(string key, object value, SPWeb propertyBag);

        /// <summary>
        /// Sets a config value for a specific key in the specified property bag
        /// </summary>
        /// <param name="key">The key for this config setting</param>
        /// <param name="value">The value to give this config setting</param>
        /// <param name="propertyBag">The property bag to store the setting in</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void SetInPropertyBag(string key, object value, SPSite propertyBag);

        /// <summary>
        /// Sets a config value for a specific key in the specified property bag
        /// </summary>
        /// <param name="key">The key for this config setting</param>
        /// <param name="value">The value to give this config setting</param>
        /// <param name="propertyBag">The property bag to store the setting in</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void SetInPropertyBag(string key, object value, SPWebApplication propertyBag);

        /// <summary>
        /// Sets a config value for a specific key in the specified property bag
        /// </summary>
        /// <param name="key">The key for this config setting</param>
        /// <param name="value">The value to give this config setting</param>
        /// <param name="propertyBag">The property bag to store the setting in</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        void SetInPropertyBag(string key, object value, SPFarm propertyBag);

        /// <summary>
        /// Reads a config value from the specified property bag, but will not look up the hierarchy. 
        /// </summary>
        /// <typeparam name="TValue">The type of the config value.</typeparam>
        /// <param name="key">The key the config value was stored under. </param>
        /// <param name="propertyBag">The property bag to read the config value from.</param>
        /// <returns>The config value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter"), SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        TValue GetFromPropertyBag<TValue>(string key, SPWeb propertyBag);

        /// <summary>
        /// Reads a config value from the specified property bag, but will not look up the hierarchy. 
        /// </summary>
        /// <typeparam name="TValue">The type of the config value.</typeparam>
        /// <param name="key">The key the config value was stored under. </param>
        /// <param name="propertyBag">The property bag to read the config value from.</param>
        /// <returns>The config value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter"), SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        TValue GetFromPropertyBag<TValue>(string key, SPSite propertyBag);

        /// <summary>
        /// Reads a config value from the specified property bag, but will not look up the hierarchy. 
        /// </summary>
        /// <typeparam name="TValue">The type of the config value.</typeparam>
        /// <param name="key">The key the config value was stored under. </param>
        /// <param name="propertyBag">The property bag to read the config value from.</param>
        /// <returns>The config value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter"), SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        TValue GetFromPropertyBag<TValue>(string key, SPWebApplication propertyBag);

        /// <summary>
        /// Reads a config value from the specified property bag, but will not look up the hierarchy. 
        /// </summary>
        /// <typeparam name="TValue">The type of the config value.</typeparam>
        /// <param name="key">The key the config value was stored under. </param>
        /// <param name="propertyBag">The property bag to read the config value from.</param>
        /// <returns>The config value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter"), SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        TValue GetFromPropertyBag<TValue>(string key, SPFarm propertyBag);
    }
}