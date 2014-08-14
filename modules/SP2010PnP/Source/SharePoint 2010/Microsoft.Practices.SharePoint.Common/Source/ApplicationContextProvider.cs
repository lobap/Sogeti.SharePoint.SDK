//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Security.Permissions;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.Properties;
using Microsoft.Practices.SharePoint.Common.ProxyArgs;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.UserCode;
using Microsoft.SharePoint.Utilities;
using System.Reflection;
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SharePoint.Common
{
    /// <summary>
    /// Provides the application context information for the sharepoint environment.  The logic separated into 
    /// the ApplicationContextProvider for testability.
    /// </summary>
    public class ApplicationContextProvider : IApplicationContextProvider
    {
        /// <summary>
        /// Gets the friendly application domain name for the current application domain.
        /// </summary>
        /// <returns>The friendly name of the current application domain</returns>
        public string GetCurrentAppDomainFriendlyName()
        {
            return System.AppDomain.CurrentDomain.FriendlyName;
        }

        /// <summary>
        /// Gets the local farm
        /// </summary>
        /// <returns>The local farm instance</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]

        public SPFarm GetSPFarmLocal()
        {
            return SPFarm.Local;
        }

        /// <summary>
        /// Provides the current SPContext.
        /// </summary>
        /// <returns>Returns the current web</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]

        public SPWeb GetSPContextCurrentWeb()
        {
            return SPContext.Current.Web;
        }

        /// <summary>
        /// Executes a registered proxy operation 
        /// </summary>
        /// <param name="assemblyName">The assembly name for the proxy to execute</param>
        /// <param name="typeName">The name of the type of the proxy operation</param>
        /// <param name="args">The arguments to pass to the proxy operation</param>
        /// <returns></returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]

        public object ExecuteRegisteredProxyOperation(string assemblyName, string typeName, SPProxyOperationArgs args)
        {
            return SPUtility.ExecuteRegisteredProxyOperation(assemblyName, typeName, args);
        }

        /// <summary>
        /// Returns true if the proxy checker is installed (which is a full trust proxy that can check if other proxies are
        /// available).
        /// </summary>
        /// <returns>returns true if the proxy checker is installed, false otherwise</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]

        public bool IsProxyCheckerInstalled()
        {
            if (CheckInGAC() == false)
            {
                return false;
            }

            var args = new ProxyInstalledArgs() {AssemblyName = ProxyInstalledArgs.OperationAssemblyName, TypeName = ProxyInstalledArgs.OperationTypeName};

            try
            {
                var result = ExecuteRegisteredProxyOperation(
                                    ProxyInstalledArgs.OperationAssemblyName,
                                     ProxyInstalledArgs.OperationTypeName,
                                     args);
            }
            catch (ArgumentException)
            {
                // this will occur if the proxy args are not in a gac'd assembly (i.e., if the Microsoft.Practices.SharePoint.Common
                // assembly is not gac'd).
                return false;
            }
            catch (TypeLoadException)
            {
                //this will occur if the operation is not deployed to the gac
                return false;
            }
            catch (InvalidOperationException)
            {
                // this will occur if the operation isn't available.
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true if a proxy is installed identified by the assembly name and type provided.
        /// </summary>
        /// <param name="assemblyName">The assembly name to check for the proxy installed (fully qualified)</param>
        /// <param name="typeForProxy">The type of the proxy to check for</param>
        /// <returns></returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public bool IsProxyInstalled(string assemblyName, string typeForProxy)
        {
            var args = new ProxyInstalledArgs();
            args.AssemblyName = assemblyName;
            args.TypeName = typeForProxy;

            var result = ExecuteRegisteredProxyOperation(
                                    ProxyInstalledArgs.OperationAssemblyName,
                                     ProxyInstalledArgs.OperationTypeName,
                                     args);

            if (result != null && result.GetType().IsSubclassOf(typeof(System.Exception)))
            {
                var configException = new ConfigurationException(Resources.FailureReadingProxyInstalled, (Exception)result);
                throw configException;
            }

            return (bool)result;
        }

        /// <summary>
        /// Checks if the currently running assembly is in the GAC
        /// </summary>
        /// <returns>True if the assembly is in the GAC</returns>
        protected virtual bool CheckInGAC()
        {
            return Assembly.GetCallingAssembly().GlobalAssemblyCache;
        }
    }
}
