//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.UserCode;
using Microsoft.Practices.SharePoint.Common.ProxyArgs;
using Microsoft.Practices.SharePoint.Common.Logging;

namespace Microsoft.Practices.SharePoint.Common.LoggerProxy.Features.LoggerProxyFeature
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("e887ec7f-ace2-4129-bb2d-6164b309996e")]
    public class LoggerProxyFeatureEventReceiver : SPFeatureReceiver
    {
        /// <summary>
        /// Registers the proxy operation for logging and tracing with the farm.
        /// </summary>
        /// <param name="properties">The properties provided to the feature receiver</param>
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPUserCodeService userCodeService = SPUserCodeService.Local;

            SPProxyOperationType loggingOperation =
                new SPProxyOperationType(
                    ProxyOperationTypes.LoggingProxyAssemblyName, ProxyOperationTypes.LoggingOpTypeName);

            userCodeService.ProxyOperationTypes.Add(loggingOperation);

            SPProxyOperationType tracingOperation =
                new SPProxyOperationType(
                    ProxyOperationTypes.LoggingProxyAssemblyName, ProxyOperationTypes.TracingOpTypeName);

            userCodeService.ProxyOperationTypes.Add(tracingOperation);

            userCodeService.Update();
            DiagnosticsService.Register();
        }


        /// <summary>
        /// Removes the proxy operations for logging and tracing from the farm.
        /// </summary>
        /// <param name="properties">The properties provided</param>
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPUserCodeService userCodeService = SPUserCodeService.Local;

            SPProxyOperationType loggingOperation =
                new SPProxyOperationType(
                    ProxyOperationTypes.LoggingProxyAssemblyName, ProxyOperationTypes.LoggingOpTypeName);

            userCodeService.ProxyOperationTypes.Remove(loggingOperation);

            SPProxyOperationType tracingOperation =
                new SPProxyOperationType(
                    ProxyOperationTypes.LoggingProxyAssemblyName, ProxyOperationTypes.TracingOpTypeName);

            userCodeService.ProxyOperationTypes.Remove(tracingOperation);

            userCodeService.Update();
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
