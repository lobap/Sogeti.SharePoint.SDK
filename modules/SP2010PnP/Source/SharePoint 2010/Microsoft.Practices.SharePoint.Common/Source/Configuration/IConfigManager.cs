//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using Microsoft.SharePoint;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// The IConfigManager interface allows you to read and write config settings in property bags for different levels of web, 
    /// site, web application, and farm.
    /// 
    /// This interface is closely related to the  <see cref="IHierarchicalConfig"/>  class. This class allows you
    /// to read configuration at specific levels, and to manage configuration settings (create/update/delete) at specific levels 
    /// that are read by the hierarchical configuration.  Using this interface is a two step process.  You first get a property bag to
    /// operate upon by calling GetPropertyBag to retrieve the property bag, and then calling the desired operation for setting, getting, 
    /// or removing a value passing in the property bag. If a property bag is not available for the level, then
    /// GetPropertyBag will return null.
    /// 
    /// To use the IConfigManager from an environment without a SPContext.Current, like a feature receiver, list recieve, or console
    /// application, then set the web to use as a starting point for the hierarchy using <see cref="IConfigManager.SetWeb"/>.  If you 
    /// call into the configuration manager without a SPContext.Current, and without setting the web, you can only operate upon 
    /// a property bag at the <see cref="ConfigLevel.CurrentSPFarm"/> level. 
    /// </summary>
    public interface IConfigManager
    {
        /// <summary>
        /// Remove a config value from a specified location. This method will not look up in parent property bags. 
        /// </summary>
        /// <param name="key">The configsetting to remove</param>
        /// <param name="propertyBag">The property bag to remove it from</param>
        void RemoveKeyFromPropertyBag(string key, IPropertyBag propertyBag);

        /// <summary>
        /// See if a particular property bag contains a config setting with that key. 
        /// This method will check the specific property bag provided for the value. 
        /// </summary>
        /// <param name="key">The key to use to find the config setting</param>
        /// <param name="propertyBag">The property bag to look in</param>
        /// <returns>True if the PropertyBag is found, else false</returns>
        bool ContainsKeyInPropertyBag(string key, IPropertyBag propertyBag);

        /// <summary>
        /// Sets a config value for a specific key in the specified property bag
        /// </summary>
        /// <param name="key">The key for this config setting</param>
        /// <param name="value">The value to give this config setting</param>
        /// <param name="propertyBag">The property bag to store the setting in</param>
        void SetInPropertyBag(string key, object value, IPropertyBag propertyBag);
        
        /// <summary>
        /// Reads a config value from the specified property bag, but will not look up the hierarchy. 
        /// </summary>
        /// <typeparam name="TValue">The type of the config value.</typeparam>
        /// <param name="key">The key the config value was stored under. </param>
        /// <param name="propertyBag">The property bag to read the config value from.</param>
        /// <returns>The config value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        TValue GetFromPropertyBag<TValue>(string key, IPropertyBag propertyBag);
        
        /// <summary>
        /// Gets the property bag for the level specified.  If the property bag for the level is not available,
        /// then null will be returned.
        /// </summary>
        /// <param name="level">The level to retrieve</param>
        /// <returns>The property bag for the level, or null if no property bag for the level is available.</returns>
        IPropertyBag GetPropertyBag(ConfigLevel level);

        /// <summary>
        /// Returns true if the farm can be accessed in the current context, false otherwise.  The farm can not be accessed
        /// from a sandbox application unless a full-trust proxy is installed that enables reading from the farm (and web application) 
        /// configuration elvels.
        /// </summary>
        bool CanAccessFarmConfig { get; }

        /// <summary>
        /// Sets the web to use as a basis for building the property bags to use for configuration (this technique is called method injection).
        /// Use this method to set the web to use when SPContext.Current is not available.
        /// </summary>
        /// <param name="web">The web to use as a basis for building the property bags available</param>
        void SetWeb(SPWeb web);

     }
}