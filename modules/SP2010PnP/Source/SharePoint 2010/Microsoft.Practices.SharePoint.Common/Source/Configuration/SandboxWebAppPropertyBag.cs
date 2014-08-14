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
    /// A read only property bag for use in the sandbox to access the web application settings.
    /// </summary>
    public class SandboxWebAppPropertyBag: IPropertyBag 
    {
        Guid siteId;

        /// <summary>
        /// Constructs a web application property bag for the sandbox.
        /// </summary>
        /// <param name="siteId">The ID of the site that is associated with the web application</param>
        public SandboxWebAppPropertyBag(Guid siteId)
        {
            this.siteId = siteId;
        }

        /// <summary>
        /// Checks if the web application property bag contains the key specified.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns><c>true</c> if the property is in the bag, <c>false</c> otherwise</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public bool Contains(string key)
        {
            Validation.ArgumentNotNullOrEmpty(key, "key");

            var args = new ContainsKeyDataArgs();
            args.Key = key;
            args.SiteId = this.siteId;

            args.Level = (int) ConfigLevel.CurrentSPWebApplication;

            var result = SPUtility.ExecuteRegisteredProxyOperation(
                                    ContainsKeyDataArgs.OperationAssemblyName,
                                     ContainsKeyDataArgs.OperationTypeName,
                                     args);


            if (result != null && result.GetType().IsSubclassOf(typeof(System.Exception)))
            {
                var ex = new ConfigurationException(string.Format(CultureInfo.CurrentCulture,
                    Resources.UnexpectedExceptionFromSandbox, ConfigLevel.CurrentSPWebApplication.ToString()), (Exception)result);
                throw ex;
            }

            return (bool)result;
        }

        /// <summary>
        /// Indexer for getting and setting values for the key specified.  Setting values at the web application level will always
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
                args.SiteId = siteId;
                args.Level = (int) ConfigLevel.CurrentSPWebApplication;

                object result = SPUtility.ExecuteRegisteredProxyOperation(
                                    ReadConfigArgs.OperationAssemblyName,
                                     ReadConfigArgs.OperationTypeName,
                                     args);

                if (result != null && result.GetType().IsSubclassOf(typeof(System.Exception)))
                {
                    ExceptionHelper.ThrowSandboxConfigurationException((Exception)result, ConfigLevel.CurrentSPWebApplication);
                }

                return (string)result;
            }
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            set
            {
                Validation.ArgumentNotNullOrEmpty(key, "key");
                throw new InvalidOperationException(Resources.WriteNotAllowedInSandboxToWebApplication);
            }
        }

        /// <summary>
        /// The configuration level for this property bag, always returns ConfigLevel.CurrentSPWebApplication.
        /// </summary>
        public ConfigLevel Level
        {
            get { return ConfigLevel.CurrentSPWebApplication; }
        }


        /// <summary>
        /// Removes a setting from the property bag.  Always throws an InvalidOperationException since
        /// writes are not permitted to the web application from the sandbox.
        /// </summary>
        /// <param name="key">the key for the setting to remove from the property bag</param>
        public void Remove(string key)
        {
            throw new InvalidOperationException(Resources.WriteNotAllowedInSandboxToWebApplication);
        }
    }
}
