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
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SPG.Common.Configuration
{
    /// <summary>
    /// Interface to give a generic way to access property bags on SPFarm, SPWebApplication, SPSite, SPWeb. 
    /// 
    /// The implementations of this interface are hierarchical. This means that if a value cannot be found in the SPWeb,
    /// it will look in it's parents (SPSite through SPFarm). 
    /// </summary>
    [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)] 
    public interface IPropertyBag
    {
        /// <summary>
        /// Does a specific key exist in the PropertyBag. 
        /// </summary>
        /// <param name="key">the key to check.</param>
        /// <returns>true if the key exists, else false.</returns>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        bool Contains(string key);

        /// <summary>
        /// Gets or sets a value based on the key. If the value is not defined in this PropertyBag, it will look in it's 
        /// parent property bag. 
        /// </summary>
        /// <param name="key">The key to find the config value in the config. </param>
        /// <returns>The config value defined in the property bag. </returns>
        string this[string key] { get; set; }

        /// <summary>
        /// Save any changes made to this PropertyBag.
        /// </summary>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        void Update();

        /// <summary>
        /// The config level this PropertyBag represents. 
        /// </summary>
        ConfigLevel Level { get; }

        /// <summary>
        /// Remove a particular config setting from this property bag and all of it's child property bags, using the key. 
        /// </summary>
        /// <param name="key">The key to remove</param>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        void Remove(string key);

        /// <summary>
        /// Returns the parent IPropertyBag. If this is the root node, this should return null. 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification="Should be a method, because it can fail.")]
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        IPropertyBag GetParent();
    }
}