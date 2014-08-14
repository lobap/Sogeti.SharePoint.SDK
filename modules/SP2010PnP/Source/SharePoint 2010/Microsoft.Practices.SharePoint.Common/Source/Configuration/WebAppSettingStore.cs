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
using Microsoft.SharePoint.Administration;
using System.Runtime.InteropServices;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// Stores values for the web application level settings
    /// </summary>
    [GuidAttribute("50758A41-9AF1-4532-B933-3FE7B21C39CA")]
    public class WebAppSettingStore: SPPersistedObject
    {
         /// <summary>
        /// The settigns to store at the web application level.
        /// </summary>
        [Persisted]
        public Dictionary<string, string> Settings = new Dictionary<string, string>();

        /// <summary>
        /// The name for this persisted object.
        /// </summary>
        public static string StoreName
        {
            get
            {
                return "_pnpWebAppConfig_";
            }
        }

        static readonly string displayName = "patterns & practices Web App setting store";

        /// <summary>
        /// Required for serialization
        /// </summary>
        public WebAppSettingStore()
        {
        }

        /// <summary>
        /// Required constructor by the SPPersistedObject base class.
        /// </summary>
        /// <param name="name">The name for the persisted object</param>
        /// <param name="parent">The parent of this persisted object</param>
        public WebAppSettingStore(string name, SPPersistedObject parent)
            : base(name, parent)
        {

        }

        /// <summary>
        /// The display name shown in admin for this persisted object.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return displayName;
            }
        }

        /// <summary>
        /// Loads the setting store for the farm provided, and creates the setting store if not found.
        /// </summary>
        /// <param name="webApp">The farm to use</param>
        /// <returns>The setting store for farm level settings</returns>
        public static WebAppSettingStore Load(SPWebApplication webApp)
        {
            var settingStore = webApp.GetChild<WebAppSettingStore>(WebAppSettingStore.StoreName);

            return settingStore;
        }

        static object createlock = new object();

        /// <summary>
        /// Creates the setting store for a web application
        /// </summary>
        /// <param name="webApp">The parent web app to store the settings</param>
        /// <returns>The web app setting store instance</returns>
        public static WebAppSettingStore Create(SPWebApplication webApp)
        {
            lock (createlock)
            {
                //load to make sure it wasn't already created in a race...
                var settingStore = webApp.GetChild<WebAppSettingStore>(WebAppSettingStore.StoreName);
                if (settingStore == null)
                {
                    settingStore = new WebAppSettingStore(WebAppSettingStore.StoreName, webApp);
                    settingStore.Update();
                }
                return settingStore;
            }
        }

        /// <summary>
        /// Removes the setting store.  Warning, all settings will be lost when removed.
        /// </summary>
        /// <param name="webApp">The web app to clear the settings for</param>
        public static void DeleteStore(SPWebApplication webApp)
        {
            var settingStore = webApp.GetChild<WebAppSettingStore>(WebAppSettingStore.StoreName);

            if (settingStore != null)  
            {
                settingStore.Delete();
            }
        }
    }
}
