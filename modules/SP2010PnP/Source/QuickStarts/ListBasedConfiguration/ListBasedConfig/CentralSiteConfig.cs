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
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.SharePoint.Administration;
using Microsoft.Practices.SharePoint.Common;

namespace ListBasedConfig
{
    /// <summary>
    /// This class stores configuration information for use in creating the property bags
    /// for farm and web app.  The location of this list must be centralized for the farm and web app.
    /// </summary>
    class CentralSiteConfig
    {
        private const string defaultCentralUrlKey = "demo.centralurl";

        /// <summary>
        /// Reads the configuration settings from the farm.  This is stored in the 
        /// default property bag for farm, which uses a persisted object in the farm.  Avoid storing this
        /// in SPFarm.Properties, since updating SPFarm will result in the entire farm cache being flushed.
        /// </summary>
        /// <returns>the setting if it exists, otherwise throws a configuration exception</returns>
        private static string GetConfig()
        {
            var bag = new SPFarmPropertyBag(SPFarm.Local);

            if (bag.Contains(defaultCentralUrlKey))
                return bag[defaultCentralUrlKey] as string;
            else return null;
        }

        /// <summary>
        /// Sets the central configuration key to the value specified.
        /// </summary>
        /// <param name="url">The value of the url to set</param>
        public static void SetCentralConfigUrl(string url)
        {
            var bag = new SPFarmPropertyBag(SPFarm.Local);

            bag[defaultCentralUrlKey] = url;
        }

        /// <summary>
        /// Gets the setting for the central configuration site url.  If in the sandbox, null is returned
        /// since we can access a list in another site collection from the sandbox.
        /// </summary>
        /// <returns>The location of the centralized list for web app and farm setting storage</returns>
        public static string GetCentralConfigSiteUrl()
        {
            return SharePointEnvironment.InSandbox == false ? GetConfig() : null;
        }
    }
}
