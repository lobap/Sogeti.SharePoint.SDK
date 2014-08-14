//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.UserCode;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;

namespace Microsoft.Practices.SharePoint.Common.ProxyArgs
{
    /// <summary>
    /// The proxy args for determining if a proxy is installed.
    /// </summary>
    [Serializable]
    public class ProxyInstalledArgs : SPProxyOperationArgs
    {
        /// <summary>
        /// The name of the assemlby to check if the proxy operation is installed.
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// The type name of the proxy operation to check if the proxy operation is installed.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Constructs a proxy installed argument instance.
        /// </summary>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)] 
        public ProxyInstalledArgs()
        {
        }

        /// <summary>
        /// The assembly containing the operation to determine if the proxy operation is installed.
        /// </summary>
        public static string OperationAssemblyName
        {
            get { return ProxyOperationTypes.ConfigurationProxyAssemblyName; }
        }


        /// <summary>
        /// The type name for the operation to use to determine if a proxy is installed.
        /// </summary>
        public static string OperationTypeName
        {
            get { return ProxyOperationTypes.ProxyInstalledOpTypeName; }
        }
    }
}
