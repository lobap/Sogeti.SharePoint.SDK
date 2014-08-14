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
    /// The args for tracing a message from the sandbox.
    /// </summary>
    [Serializable]
    public class TracingOperationArgs : SPProxyOperationArgs
    {
        /// <summary>
        /// The message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The event ID to use.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// The serverity of the message.
        /// </summary>
        public int? Severity { get; set; }

        /// <summary>
        /// The cateogory of the message.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The ID of the site associated with the message.
        /// </summary>
        public Guid? SiteID { get; set; }

        /// <summary>
        /// The constructor for the arguments for the tracing operation.
        /// </summary>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public TracingOperationArgs()
        {
            Severity = null;
        }

        /// <summary>
        /// The name of the assembly.
        /// </summary>
        public static string OperationAssemblyName
        {
            get { return ProxyOperationTypes.LoggingProxyAssemblyName; }
        }

        /// <summary>
        /// The name of the type to use.
        /// </summary>
        public static string OperationTypeName
        {
            get { return ProxyOperationTypes.TracingOpTypeName; }
        }
    }
}
