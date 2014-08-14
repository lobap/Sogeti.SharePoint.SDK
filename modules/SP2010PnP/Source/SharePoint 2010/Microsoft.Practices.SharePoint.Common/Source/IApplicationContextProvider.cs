//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.UserCode;

namespace Microsoft.Practices.SharePoint.Common
{
    /// <summary>
    /// The interface for providing context to the SharePointEnvironment.
    /// </summary>
    public interface IApplicationContextProvider
    {
        /// <summary>
        /// Gets the current app domain friendly name.  Used to check if running in the sandbox.
        /// </summary>
        /// <returns>The friendly name of the current app domain</returns>
        string GetCurrentAppDomainFriendlyName();

        /// <summary>
        /// Gets the web in SPContext.Current.
        /// </summary>
        /// <returns>The web in the current context</returns>
        SPWeb GetSPContextCurrentWeb();

        /// <summary>
        /// Gets the local farm instance to use.
        /// </summary>
        /// <returns>The value of SPFarm.Local</returns>
        SPFarm GetSPFarmLocal();

        /// <summary>
        /// Executes the proxy operation with the args provided.
        /// </summary>
        /// <param name="assemblyName">The assembly for the proxy (fully qualified)</param>
        /// <param name="typeName">The type name of the proxy</param>
        /// <param name="args">The args to pass to the proxy</param>
        /// <returns></returns>
        object ExecuteRegisteredProxyOperation(string assemblyName, string typeName, SPProxyOperationArgs args);

        /// <summary>
        /// Checks if the proxy is installed that checks for the presence of other proxies.
        /// </summary>
        /// <returns>true if the proxy is installed, false otherwise</returns>
        bool IsProxyCheckerInstalled();

        /// <summary>
        /// Checks if the proxy specified is installed. If the proxy checker is not installed, then false will always
        /// be returned.
        /// </summary>
        /// <param name="assemblyName">The fully qualified assembly name containing the proxy</param>
        /// <param name="typeForProxy">The type name of the proxy</param>
        /// <returns>true if the proxy is installed, false otherwise.</returns>
        bool IsProxyInstalled(string assemblyName, string typeForProxy);
    }
}
