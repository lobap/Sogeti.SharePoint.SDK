//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using Microsoft.SharePoint.Utilities;
using Microsoft.Practices.SharePoint.Common.Properties;
using System.Globalization;
using Microsoft.Practices.SharePoint.Common.ProxyArgs;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;
namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// A property bag for reading farm configuration settings from
    /// the sandbox.  Write operation are not permitted.
    /// </summary>
    public class SandboxFarmPropertyBag : IPropertyBag
    {
        /// <summary>
        /// Checks if a key is in the property bag.
        /// </summary>
        /// <param name="key">The key to check for</param>
        /// <returns><c>true</c> if the key is found, <c>false</c> otherwise</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public bool Contains(string key)
        {
            Validation.ArgumentNotNullOrEmpty(key, "key");

            var args = new ContainsKeyDataArgs();
            args.Key = key;
            args.Level = (int)ConfigLevel.CurrentSPFarm;

            var result = SPUtility.ExecuteRegisteredProxyOperation(
                                    ContainsKeyDataArgs.OperationAssemblyName,
                                     ContainsKeyDataArgs.OperationTypeName,
                                     args);

            if (result != null && result.GetType().IsSubclassOf(typeof(System.Exception)))
            {
                var exception = (Exception) result;
                ExceptionHelper.ThrowSandboxConfigurationException(exception, ConfigLevel.CurrentSPFarm);
            }

            return (bool)result;
        }

        /// <summary>
        /// Indexer for getting and setting values for the key specified.  Setting values at the farm level will always
        /// result in a InvalidOperationException.
        /// </summary>
        /// <param name="key">The key to check for in the property bag</param>
        /// <returns>The value for the key, null if not found</returns>
     
        public string this[string key]
        {
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                Validation.ArgumentNotNullOrEmpty(key, "key");

                var args = new ReadConfigArgs();
                args.Key = key;
                args.Level = (int)ConfigLevel.CurrentSPFarm;
                object result = SPUtility.ExecuteRegisteredProxyOperation(
                                    ReadConfigArgs.OperationAssemblyName,
                                     ReadConfigArgs.OperationTypeName,
                                     args);

                if (result != null && result.GetType().IsSubclassOf(typeof(System.Exception)))
                {
                    var exception = (Exception)result;
                    ExceptionHelper.ThrowSandboxConfigurationException(exception, ConfigLevel.CurrentSPFarm);
                }

                return (string)result;
            }
            set
            {
                Validation.ArgumentNotNullOrEmpty(key, "key");
                throw new InvalidOperationException(Resources.WriteNotAllowedInSandboxToWebApplication);
            }
        }

        /// <summary>
        /// The configuration level for this property bag, always returns ConfigLevel.CurrentSPFarm.
        /// </summary>
        public ConfigLevel Level
        {
            get { return ConfigLevel.CurrentSPFarm; }
        }

        /// <summary>
        /// Removes a setting from the property bag.  Always throws an InvalidOperationException since
        /// writes are not permitted to the farm from the sandbox.
        /// </summary>
        /// <param name="key">the key for the setting to remove from the property bag</param>
        public void Remove(string key)
        {
            throw new InvalidOperationException(Resources.WriteNotAllowedInSandboxToWebApplication);
        }
    }
}
