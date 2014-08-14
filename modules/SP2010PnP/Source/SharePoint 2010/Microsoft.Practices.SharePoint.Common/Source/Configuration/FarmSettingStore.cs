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
using System.Collections;
using System.Runtime.InteropServices;

namespace Microsoft.Practices.SharePoint.Common.Configuration
{
    /// <summary>
    /// Class for persisting settings for the farm level.
    /// </summary>
    [GuidAttribute("0FA75489-47B2-492C-8878-4FED235255BB")]
    public class FarmSettingStore: SPPersistedObject
    {
         /// <summary>
        /// Settings to be stored at the farm level.  Must be a field instead of a property in order
        /// to be serialized by SharePoint.
        /// </summary>
        [Persisted]
        public Dictionary<string, string> Settings = new Dictionary<string, string>();

        /// <summary>
        /// THe name of the farm configuration store persisted object.
        /// </summary>
        public static string StoreName
        {
            get
            {
                return "_pnpFarmConfig_";
            }
        }

        static readonly string displayName = "patterns & practices Farm setting store";

        /// <summary>
        /// Required for serialization
        /// </summary>
        public FarmSettingStore()
        {
        }

        /// <summary>
        /// Required constructor by the SPPersistedObject base class.
        /// </summary>
        /// <param name="name">The name for the persisted object</param>
        /// <param name="parent">The parent of this persisted object</param>
        public FarmSettingStore(string name, SPPersistedObject parent)
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
        /// <param name="farm">The farm to use</param>
        /// <returns>The setting store for farm level settings</returns>
        public static FarmSettingStore Load(SPFarm farm)
        {
            var settingStore = farm.GetChild<FarmSettingStore>(FarmSettingStore.StoreName);
            return settingStore;
        }

        static object createlock = new object();

        /// <summary>
        /// Creates a new FarmSettingStore.  This operation cannot be done from a content web application.
        /// </summary>
        /// <param name="farm">The Farm to create the setting store in</param>
        /// <returns></returns>
        public static FarmSettingStore Create(SPFarm farm)
        {
            lock (createlock)
            {
                //lock to prevent duplicate create attempts on a WFE (still a possible race with other WFE's).
                var settingStore = farm.GetChild<FarmSettingStore>(FarmSettingStore.StoreName);

                if (settingStore == null)  // has not been previously saved
                {
                    settingStore = new FarmSettingStore(FarmSettingStore.StoreName, farm);
                    settingStore.Update();
                }
                return settingStore;
            }
        }

        /// <summary>
        /// Removes the setting store.  Warning, all settings will be lost when removed.
        /// </summary>
        /// <param name="farm">The farm to clear the settings for</param>
        public static void DeleteStore(SPFarm farm)
        {
             var settingStore = farm.GetChild<FarmSettingStore>(FarmSettingStore.StoreName);

             if (settingStore != null)  
             {
                 settingStore.Delete();
             }
        }
    }
}
