//===============================================================================
// Microsoft patterns & practices
// SharePoint Guidance version 2.0
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
using System;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.Practices.SPG.Common;
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint.Administration;
using Microsoft.Practices.SPG.Common.Logging;

namespace Contoso.PartnerPortal
{
    [CLSCompliant(false)]
    [Guid("306e3ea3-ecaf-4542-9282-58a4a8134b3e")]
    public class ContosoPartnerPortalEventSourceReceiver : SPFeatureReceiver
    {
        /// <summary>
        /// Initializes a new instance of the Microsoft.SharePoint.SPItemEventReceiver class.
        /// </summary>
        public ContosoPartnerPortalEventSourceReceiver()
        {
        }
		
        /// <summary>
        /// Occurs after a Feature is activated. 
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPFeatureReceiverProperties object that represents properties of the event handler.
        /// </param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
		public override void FeatureActivated(SPFeatureReceiverProperties properties)
		{
            IConfigManager config = SharePointServiceLocator.Current.GetInstance<IConfigManager>();
            config.SetInPropertyBag(Constants.EventSourceNameConfigKey, "Contoso Partner Portal", SPFarm.Local);

            ILogger logger = SharePointServiceLocator.Current.GetInstance<ILogger>();
            logger.LogToOperations("Registered event source in configuration.", 1, System.Diagnostics.EventLogEntryType.Information);
		}
		
		/// <summary>
        /// Occurs when a Feature is deactivating.
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPFeatureReceiverProperties object that represents properties of the event handler.
        /// </param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
		{

		}
		
		/// <summary>
        /// Occurs after a Feature is installed.
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPFeatureReceiverProperties object that represents properties of the event handler.
        /// </param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
		{

		}
		
		/// <summary>
        /// Occurs when a Feature is uninstalling.
        /// </summary>
        /// <param name="properties">
        /// A Microsoft.SharePoint.SPFeatureReceiverProperties object that represents properties of the event handler.
        /// </param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
		{

		}
    }
}
