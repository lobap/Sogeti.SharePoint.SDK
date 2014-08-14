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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.Practices.SPG.AJAXSupport.Properties;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;

namespace Microsoft.Practices.SPG.AJAXSupport
{
    /// <summary>
    /// Feature receiver that will add Ajax support to a Web Application in SharePoint. 
    /// </summary>
    [CLSCompliant(false)]
    [Guid("e3446eb2-a4e1-48ed-927e-c0f167d57a7c")]
    public class WebApplicationFeatureReceiver : SPFeatureReceiver
    {
        private List<SPWebConfigModification> modifications;
        private string owner;

        /// <summary>
        /// Initializes a new instance of the WebApplicationFeatureReceiver class.
        /// </summary>
        public WebApplicationFeatureReceiver()
        {
            modifications = new List<SPWebConfigModification>();

            owner = GetType().Assembly.GetName().ToString();

            SPWebConfigModificationHelper.CreateAndAddWebConfigModification(modifications,
                "sectionGroup[@name=\"system.web.extensions\"]",
                "configuration/configSections", 
                SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode, 
                Resources.WebConfigModSectionGroup,
                owner);

            SPWebConfigModificationHelper.CreateAndAddWebConfigModification(modifications,
                "controls",
                "configuration/system.web/pages", 
                SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode, 
                Resources.WebConfigModControls,
                owner);

            SPWebConfigModificationHelper.CreateAndAddWebConfigModification(modifications,
                "add[@assembly=\"System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\"]",
                "configuration/system.web/compilation/assemblies", 
                SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode, 
                Resources.WebConfigModAssemblies,
                owner);

            SPWebConfigModificationHelper.CreateAndAddWebConfigModification(modifications,
                "add[@verb=\"*\"][@path=\"*.asmx\"]",
                "configuration/system.web/httpHandlers", 
                SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode,
                Resources.WebConfigModHttpHandlersASMX,
                owner);

            SPWebConfigModificationHelper.CreateAndAddWebConfigModification(modifications,
                "add[@verb=\"*\"][@path=\"*_AppService.axd\"]",
                "configuration/system.web/httpHandlers", 
                SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode,
                Resources.WebConfigModHttpHandlersAppService,
                owner);

            SPWebConfigModificationHelper.CreateAndAddWebConfigModification(modifications,
                "add[@verb=\"GET,HEAD\"][@path=\"ScriptResource.axd\"]",
                "configuration/system.web/httpHandlers", 
                SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode,
                Resources.WebConfigModHttpHandlerScriptResource,
                owner);

            SPWebConfigModificationHelper.CreateAndAddWebConfigModification(modifications,
                "add[@name=\"ScriptModule\"]",
                "configuration/system.web/httpModules", 
                SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode,
                Resources.WebConfigModHttpHandlerScriptModule,
                owner);

            SPWebConfigModificationHelper.CreateAndAddWebConfigModification(modifications,
                "SafeControl[@Assembly=\"System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\"]",
                "configuration/SharePoint/SafeControls", 
                SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode,
                Resources.WebConfigModSafeControl,
                owner);

            SPWebConfigModificationHelper.CreateAndAddWebConfigModification(modifications,
                "system.web.extensions",
                "configuration", 
                SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode,
                Resources.WebConfigModSystemWebExtensions,
                owner);

            SPWebConfigModificationHelper.CreateAndAddWebConfigModification(modifications,
                "system.webServer",
                "configuration", 
                SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode,
                Resources.WebConfigModSystemWebServer,
                owner);
        }

        /// <summary>
        /// Occurs after a Feature is activated. 
        /// </summary>
        /// <param name="properties">
        /// A <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"></see> object that represents properties of the event handler.
        /// </param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel=true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = (SPWebApplication)properties.Feature.Parent;

            foreach (SPWebConfigModification modification in modifications)
            {
                webApp.WebConfigModifications.Add(modification);
            }

            webApp.Update();
            webApp.WebService.ApplyWebConfigModifications();
        }



        /// <summary>
        /// Occurs when a Feature is deactivating.
        /// </summary>
        /// <param name="properties">
        /// A <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"></see> object that represents properties of the event handler.
        /// </param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = (SPWebApplication)properties.Feature.Parent;

            SPWebConfigModificationHelper.CleanUpWebConfigModifications(webApp, owner);

            webApp.Update();
            webApp.WebService.ApplyWebConfigModifications();
        }

        
        /// <summary>
        /// Occurs after a Feature is installed.
        /// </summary>
        /// <param name="properties">
        /// A <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"></see> object that represents properties of the event handler.
        /// </param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {

        }

        /// <summary>
        /// Occurs when a Feature is uninstalling.
        /// </summary>
        /// <param name="properties">
        /// A <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"></see> object that represents properties of the event handler.
        /// </param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {

        }
    }
}