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
    /// The proxy arguments for logging through the full trust proxy.
    /// </summary>
    [Serializable]
    public class LoggingOperationArgs: SPProxyOperationArgs
    {
        /// <summary>
        /// The message to log
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The event id for the message being logged.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// The severity of the message to log.
        /// </summary>
        public int? Severity { get; set; }

        /// <summary>
        /// The name of the cateory to log.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The ID of the site whose parent web application or farm is used in the logging operation.
        /// </summary>
        public Guid? SiteID { get; set; }

        /// <summary>
        /// The constructor for the logging operation.
        /// </summary>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public LoggingOperationArgs()
        {
            Severity = null;
        }

        /// <summary>
        /// The assembly name containing the operation.
        /// </summary>
        public static string OperationAssemblyName
        {
            get { return ProxyOperationTypes.LoggingProxyAssemblyName; }
        }

        /// <summary>
        /// The type name for the operation.
        /// </summary>
        public static string OperationTypeName
        {
            get { return ProxyOperationTypes.LoggingOpTypeName; }
        }
    }
}
