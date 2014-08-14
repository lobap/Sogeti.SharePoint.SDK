//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using Microsoft.Practices.SharePoint.Common.Configuration.ConfigurationProxy.Properties;
using Microsoft.SharePoint.UserCode;
using Microsoft.Practices.SharePoint.Common.ProxyArgs;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.Globalization;

namespace Microsoft.Practices.SharePoint.Common.Configuration.ConfigurationProxy
{
    /// <summary>
    /// The full trust proxy operation for determining if a key exists in a web application or farm property bag.
    /// </summary>
    public class ContainsKeyOperation : SPProxyOperation
    {
        /// <summary>
        /// Implements the operation for determining if a key exists in a web application
        /// </summary>
        /// <param name="args">The arguments for the contains key operation.  This must be an instance of ContainsKeyDataArgs.</param>
        /// <returns>true if the key found, false if the key is not found, or an exception representing an error if an error occurred</returns>
        public override object Execute(SPProxyOperationArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            var containsKeyArgs = args as ContainsKeyDataArgs;

            if (containsKeyArgs == null)
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resources.InvalidProxyArgumentType,
                    typeof(ContainsKeyDataArgs).FullName, args.GetType().FullName);
                var ex = new ConfigurationException(message);
                return ex;
            }
            else if (containsKeyArgs.Key == null)
            {
                return new ArgumentNullException("ContainsKeyDataArgs.Key");
            }

            try
            {
                ConfigLevel level = (ConfigLevel)containsKeyArgs.Level;

                if (containsKeyArgs.Key.StartsWith(ConfigManager.PnPKeyNamespace) == false)
                {
                    string message = string.Format(CultureInfo.CurrentCulture, Resources.InvalidKeyName, containsKeyArgs.Key);
                    ConfigurationException ex = new ConfigurationException(message);
                    return ex;
                }
                else
                {
                    bool contains = false;

                    if (level == ConfigLevel.CurrentSPWebApplication)
                    {
                        if (containsKeyArgs.SiteId == Guid.Empty)
                        {
                            return new ConfigurationException(Resources.EmptySiteGuid);
                        }
                        using (SPSite site = new SPSite(containsKeyArgs.SiteId))
                        {
                            SPWebAppPropertyBag bag = new SPWebAppPropertyBag(site.WebApplication);
                            contains = bag.Contains(containsKeyArgs.Key);
                        }
                    }
                    else if (level == ConfigLevel.CurrentSPFarm)
                    {
                        var bag = new SPFarmPropertyBag(SPFarm.Local);
                        contains = bag.Contains(containsKeyArgs.Key);
                    }
                    else
                    {
                        string message = string.Format(CultureInfo.CurrentCulture, Resources.InvalidConfigLevel, level.ToString());
                        ConfigurationException ex = new ConfigurationException(message);
                        return ex;
                    }
                    return contains;
                }
            }
            catch (Exception excpt)
            {
                return excpt;
            }
        }
    }
}
