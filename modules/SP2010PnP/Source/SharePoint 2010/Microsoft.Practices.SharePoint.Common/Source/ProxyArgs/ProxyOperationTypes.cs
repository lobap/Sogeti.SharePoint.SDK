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

namespace Microsoft.Practices.SharePoint.Common.ProxyArgs
{
    /// <summary>
    /// The list of the assemlys and types for the sharepoint library.
    /// </summary>
    public class ProxyOperationTypes
    {
        /// <summary>
        /// The assembly for the config proxy operation
        /// </summary>
        public static readonly string ConfigurationProxyAssemblyName = "Microsoft.Practices.SharePoint.Common.Configuration.ConfigurationProxy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ef4330804b3c4129";

        /// <summary>
        /// The assembly for logging proxy.
        /// </summary>
        public static readonly string LoggingProxyAssemblyName = "Microsoft.Practices.SharePoint.Common.LoggerProxy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ef4330804b3c4129";
        
        /// <summary>
        /// The operation for reading configuration data.
        /// </summary>
         public static readonly string ReadConfigDataOpTypeName = "Microsoft.Practices.SharePoint.Common.Configuration.ConfigurationProxy.ReadConfigurationOperation";

        /// <summary>
        /// The operation name for determining if a key is contained in config settings.
        /// </summary>
        public static readonly string ContainsKeyConfigOpTypeName = "Microsoft.Practices.SharePoint.Common.Configuration.ConfigurationProxy.ContainsKeyOperation";

        /// <summary>
        /// The proxy type for determining if a proxy operation is installed.
        /// </summary>
        public static readonly string ProxyInstalledOpTypeName = "Microsoft.Practices.SharePoint.Common.Configuration.ConfigurationProxy.ProxyInstalledOperation";

        /// <summary>
        /// The type for logging to the event log.
        /// </summary>
        public static readonly string LoggingOpTypeName = "Microsoft.Practices.SharePoint.Common.Logging.LoggerProxy.LoggingOperation";

        /// <summary>
        /// The type for tracing to the ULS log.
        /// </summary>
        public static readonly string TracingOpTypeName = "Microsoft.Practices.SharePoint.Common.Logging.LoggerProxy.TracingOperation";
    }
}
