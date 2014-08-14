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
using Microsoft.SharePoint;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// Allows you to read config settings in a hierarchical way. You can look for a config setting in a property bag
    /// at a certain level, and it will look up the hierarchy for the first location where that config setting is located. 
    /// 
    /// Using this interface, you can look up values in your current context. That means: in your Current SPWeb (default)
    /// , current SPSite, current SPWebApplication and current SPFarm.  If the SPContext.Current is not available, you can
    /// set the web to use as a basis for the web, site (web.Site), web application (web.Site.WebApplication), and farm
    /// (web.Site.WebApplication.Farm) to use.  Use this approach for example in a feature reciever, event reciever, or a 
    /// console application.
    /// 
    /// This interface is only for reading config settings using the hierarchy.  Use <see cref="IConfigManager"/> for getting
    /// values from a specific level in the hierarchy, or to manage settings stored within property bags. 
    /// </summary>
    public interface IHierarchicalConfig
    {
        /// <summary>
        /// Read a config value based on the key, walking the property bag hierarchy to find the value.  Throws 
        /// a <see cref="ConfigurationException"/> if the key is not found.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to read. </typeparam>
        /// <param name="key">The key associated with the config value</param>
        /// <returns>The value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design","CA1004:GenericMethodsShouldProvideTypeParameter")]
        TValue GetByKey<TValue>(string key);

        /// <summary>
        /// Read a config value based on the key, walking the property bag hierarchy starting at the specified config level.  Throws 
        /// a <see cref="ConfigurationException"/> if the key is not found.
        /// 
        /// </summary>
        /// <typeparam name="TValue">The type of the value to read.</typeparam>
        /// <param name="key">The key associated with the config value</param>
        /// <param name="level">The config level to start looking in.
        /// For example, <see cref="ConfigLevel.CurrentSPWebApplication"/> means that it's looking at the current 'SPWebApplication'
        /// and above.
        ///  </param>
        /// <returns>The value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design","CA1004:GenericMethodsShouldProvideTypeParameter")]
        TValue GetByKey<TValue>(string key, ConfigLevel level);

        /// <summary>
        /// Determines if a config value with the specified key can be found in config. Will walk the 
        /// hierarchy of property bags to find the key.
        /// </summary>
        /// <param name="key">the specified key</param>
        /// <returns>
        /// 	<c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        bool ContainsKey(string key);

        /// <summary>
        /// Determines if a config value with the specified key can be found walking the property bag hierarchy, starting at 
        /// the specified level. 
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="level">The level to start looking in.</param>
        /// <returns>
        /// 	<c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        bool ContainsKey(string key, ConfigLevel level);

        /// <summary>
        /// Sets the web to use as a basis for creating the property bag hierarchy.  Set this value if you are accessing
        /// configuration from a context where SPContext.Current is null.
        /// </summary>
        /// <param name="web">The web to use for creating the property bags for the hierarchy</param>
        void SetWeb(SPWeb web);

    }
}