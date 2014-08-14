//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.Properties;
using Microsoft.Practices.SharePoint.Common.ProxyArgs;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Security;
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SharePoint.Common
{
    /// <summary>
    /// Gets information about the current SharePoint context.
    /// </summary>
    public static class SharePointEnvironment
    {
        static int isInSandbox = -1;
        static object lockObj = new object();
        static IApplicationContextProvider applicationContextProvider;

        static SharePointEnvironment()
        {
            Reset();
        }

        /// <summary>
        /// Resets the SharePoint environment, clearing all cached information.
        /// </summary>
        public static void Reset()
        {
            isInSandbox = -1;
            proxyCheckerIsInstalled = -1;
            applicationContextProvider = new ApplicationContextProvider();
        }

        /// <summary>
        /// Returns true if running in the sandbox, false otherwise.
        /// </summary>
        public static bool InSandbox
        {
            get
            {
                if (isInSandbox == -1)
                {
                    lock (lockObj)
                    {
                        if (isInSandbox == -1)
                        {
                            if (applicationContextProvider.GetCurrentAppDomainFriendlyName().Contains("Sandbox"))
                                isInSandbox = 1;
                            else
                                isInSandbox = 0;
                        }
                    }
                }

                return isInSandbox == 1;
            }
        }

        /// <summary>
        /// Returns true if SharePoint can be accessed in any way, at the farm level or above.
        /// </summary>
        public static bool CanAccessSharePoint
        {
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                if (InSandbox)
                    return true;
                if (CanAccessFarm)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Returns true if the farm can be accessed.  Do not call from a sandbox application.
        /// </summary>
       
        public static bool CanAccessFarm
        {
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                return (applicationContextProvider.GetSPFarmLocal() != null);
            }
        }

        /// <summary>
        /// Returns true if farm level config can be accessed, false otherwise.  Safe to call from the sandbox.
        /// If in the sandbox, determines if the proxy is installed for reading configuration from web application or 
        /// the farm.
        /// </summary>
        public static bool CanAccessFarmConfig
        {
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                if (InSandbox)
                {
                    if (ProxyInstalled(ReadConfigArgs.OperationAssemblyName, ReadConfigArgs.OperationTypeName))
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    return CanAccessFarm;
                }
            }
        }

        /// <summary>
        /// Sets the application context provider to use for getting environment information.  Provided for testability.
        /// </summary>
        public static IApplicationContextProvider ApplicationContextProvider
        {
            set
            {
                applicationContextProvider = value;
            }
        }

        static int proxyCheckerIsInstalled = -1;

        /// <summary>
        /// Determines if a full trust sandbox proxy has been installed to the farm.
        /// </summary>
        /// <param name="assemblyName">The fully qualified assembly name containing the proxy</param>
        /// <param name="typeForProxy">The type for the proxy</param>
        /// <returns>True if the proxy is installed, false otherwise</returns>
        public static bool ProxyInstalled(string assemblyName, string typeForProxy)
        {
            if (proxyCheckerIsInstalled == -1)
            {
                if (applicationContextProvider.IsProxyCheckerInstalled())
                {
                    proxyCheckerIsInstalled = 1;
                }
                else
                {
                    proxyCheckerIsInstalled = 0;
                }
            }

            if (proxyCheckerIsInstalled == 1)
                return applicationContextProvider.IsProxyInstalled(assemblyName, typeForProxy);

            return false;
        }

    }
}
