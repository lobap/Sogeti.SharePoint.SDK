//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using Microsoft.SharePoint.UserCode;
using Microsoft.Practices.SharePoint.Common.ProxyArgs;
using System.Globalization;
using Microsoft.Practices.SharePoint.Common.Configuration.ConfigurationProxy.Properties;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace Microsoft.Practices.SharePoint.Common.Configuration.ConfigurationProxy
{
    /// <summary>
    /// The proxy operation to read configuration items from the web application and farm levels.
    /// </summary>
    public class ReadConfigurationOperation : SPProxyOperation
    {
        /// <summary>
        /// Implements looking up a value for the key provided at the farm or web application level.  The key must be preceded
        /// by the p and p namespace value, otherwise a <see cref="ConfigurationException"/> will be returned.
        /// </summary>
        /// <param name="args">The arguments for the operation, must be of type <see cref="ReadConfigArgs"/></param>
        /// <returns>The value for the key, null if no value found, or an exception representing an error if an error occurred</returns>
        public override object Execute(SPProxyOperationArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            var readConfigArgs = args as ReadConfigArgs;

            if (readConfigArgs == null)
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resources.InvalidProxyArgumentType,
                    typeof(ReadConfigArgs).FullName, args.GetType().FullName);
                var ex = new ConfigurationException(message);
                return ex;
            }
            else if (readConfigArgs.Key == null)
            {
                return new ArgumentNullException("ReadConfigArgs.Key");
            }

            try
            {
                ConfigLevel level = (ConfigLevel)readConfigArgs.Level;

                if (readConfigArgs.Key.StartsWith(ConfigManager.PnPKeyNamespace) == false)
                {
                    string message = string.Format(CultureInfo.CurrentCulture, Resources.InvalidKeyName, readConfigArgs.Key);
                    ConfigurationException ex = new ConfigurationException(message);
                    return ex;
                }
                else
                {
                    if (level == ConfigLevel.CurrentSPWebApplication)
                    {
                        if (readConfigArgs.SiteId == Guid.Empty)
                        {
                            return new ConfigurationException(Resources.EmptySiteGuid);
                        }

                        using (var site = new SPSite(readConfigArgs.SiteId))
                        {
                            var bag = new SPWebAppPropertyBag(site.WebApplication);
                            return bag[readConfigArgs.Key];
                        }
                    }
                    else if (level == ConfigLevel.CurrentSPFarm)
                    {
                        var bag = new SPFarmPropertyBag(SPFarm.Local);
                        return bag[readConfigArgs.Key];
                    }
                    else
                    {
                        string message = string.Format(CultureInfo.CurrentCulture, Resources.InvalidConfigLevel, level.ToString());
                        ConfigurationException ex = new ConfigurationException(message);
                        return ex;
                    }
                }




            }
            catch (Exception excpt)
            {
                return excpt;
            }
        }
    }
}