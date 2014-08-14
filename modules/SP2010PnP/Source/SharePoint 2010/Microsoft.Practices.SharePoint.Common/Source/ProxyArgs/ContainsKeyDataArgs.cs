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
    /// The arguments for calling the full trust proxy to determine if a web app
    /// or a farm config setting exists.
    /// </summary>
    [Serializable]
    public class ContainsKeyDataArgs: SPProxyOperationArgs
    {
        /// <summary>
        /// The key to check
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The level for the key to check.
        /// </summary>
        public int Level  { get; set; }

        /// <summary>
        /// The ID of the site associated with the web application or farm.
        /// </summary>
        public Guid SiteId { get; set; }

        /// <summary>
        /// The constructor for the arguments to pass to the full trust proxy for 
        /// configuration reading.
        /// </summary>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public ContainsKeyDataArgs()
        {
        }

        /// <summary>
        /// The name of the assembly for the proxy operation.
        /// </summary>
        public static string OperationAssemblyName
        {
            get { return ProxyOperationTypes.ConfigurationProxyAssemblyName; }
        }

        /// <summary>
        /// The name of the type for the proxy operation.
        /// </summary>
        public static string OperationTypeName
        {
            get { return ProxyOperationTypes.ContainsKeyConfigOpTypeName; }
        }
    }
}
