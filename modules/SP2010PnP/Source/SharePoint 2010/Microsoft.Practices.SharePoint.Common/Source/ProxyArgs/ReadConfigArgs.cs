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
    /// The arguments for reading configuration arguments.
    /// </summary>
    [Serializable]
    public class ReadConfigArgs: SPProxyOperationArgs
    {
        /// <summary>
        /// The key for the setting to read.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The level for reading the config key.
        /// </summary>
        public int Level  { get; set; }

        /// <summary>
        /// The ID of the site is the child of the web app or farm to read config for.
        /// </summary>
        public Guid SiteId { get; set; }

        /// <summary>
        /// The constructor for read config args.
        /// </summary>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public ReadConfigArgs()
        {
        }

        /// <summary>
        /// The name of the assembly containg the read config operation.
        /// </summary>
        public static string OperationAssemblyName
        {
            get { return ProxyOperationTypes.ConfigurationProxyAssemblyName; }
        }

        /// <summary>
        /// The type of the operation for reading config.
        /// </summary>
        public static string OperationTypeName
        {
            get { return ProxyOperationTypes.ReadConfigDataOpTypeName; }
        }
    }
}
