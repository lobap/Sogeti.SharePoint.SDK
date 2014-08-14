//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System.Security.Permissions;
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// The Interface that represents a property bag (stores name-value pairs) for holding
    /// configuration information for a particular level like a web, site, web application, and farm. 
    /// </summary>
    public interface IPropertyBag
    {
        /// <summary>
        /// Checks if a specific key exist in the PropertyBag. 
        /// </summary>
        /// <param name="key">the key to check.</param>
        /// <returns><c>true</c> if the key exists, else <c>false</c>.</returns>
        bool Contains(string key);

        /// <summary>
        /// Gets or sets a value based on the key. If the value is not defined in this PropertyBag.
        /// </summary>
        /// <param name="key">The key to find the config value in the config. </param>
        /// <returns>The config value defined in the property bag, null if not found </returns>
        string this[string key] { get; set; }

        /// <summary>
        /// The config level this PropertyBag represents. 
        /// </summary>
        ConfigLevel Level { get; }

        /// <summary>
        /// Remove a particular config setting from this property bag.
        /// </summary>
        /// <param name="key">The key to remove</param>
        void Remove(string key);
    }
}