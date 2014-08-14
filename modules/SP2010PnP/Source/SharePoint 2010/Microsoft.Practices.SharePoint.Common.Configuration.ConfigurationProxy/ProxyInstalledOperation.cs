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
using Microsoft.SharePoint.UserCode;
using Microsoft.Practices.SharePoint.Common.ProxyArgs;
using Microsoft.SharePoint.Administration;
using System.Globalization;
using Microsoft.Practices.SharePoint.Common.Configuration.ConfigurationProxy.Properties;

namespace Microsoft.Practices.SharePoint.Common.Configuration.ConfigurationProxy
{         
    /// <summary>
    /// The proxy operation used to determine if a full-trust proxy is installed in the farm.
    /// </summary>
    public class ProxyInstalledOperation : SPProxyOperation
    {
        /// <summary>
        /// Determines if a full trust proxy is installed to the farm.
        /// </summary>
        /// <param name="args">Provides the arguments defining the full trust proxy to check for installation, must be of type <see cref="ProxyInstalledArgs"/></param>
        /// <returns>true if the proxy is installed in the farm, false if not installed, or an exception representing an error if an error occurred</returns>
        public override object Execute(SPProxyOperationArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            var proxyInstalledArgs = args as ProxyInstalledArgs;

            if (proxyInstalledArgs == null)
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resources.InvalidProxyArgumentType,
                    typeof(ProxyInstalledArgs).FullName, args.GetType().FullName);
                var ex = new ConfigurationException(message);
                return ex;
            }
            else if (proxyInstalledArgs.TypeName == null)
            {
                return new ArgumentNullException("ProxyInstalledArgs.TypeName");
            }
            else if (proxyInstalledArgs.AssemblyName == null)
            {
                return new ArgumentNullException("ProxyInstalledArgs.AssemblyName");
            }


            try
            {
                 return GetProxyInstalled(proxyInstalledArgs.AssemblyName, proxyInstalledArgs.TypeName);
            }
            catch (Exception excpt)
            {
                return excpt;
            }
        }



        private static bool GetProxyInstalled(string assemblyName, string typeForProxy)
        {
            var proxy = new SPProxyOperationType(assemblyName, typeForProxy);
            bool proxyFound = SPUserCodeService.Local.ProxyOperationTypes.Contains(proxy, new ProxyComparer());
            return proxyFound;
        }


        /// <summary>
        /// Does an equality comparison of two proxy operation types.
        /// </summary>
        public class ProxyComparer : IEqualityComparer<SPProxyOperationType>
        {
            /// <summary>
            /// Returns true if the two items are equal, false otherwise.
            /// </summary>
            /// <param name="x">the first operation to compare</param>
            /// <param name="y">the second operation to compare</param>
            /// <returns></returns>
            public bool Equals(SPProxyOperationType x, SPProxyOperationType y)
            {
                if (x == null && y == null)
                    return true;

                if (x == null || y == null)
                    return false;

                return (x.AssemblyName.Equals(y.AssemblyName, StringComparison.OrdinalIgnoreCase) &&
                        x.TypeName.Equals(y.TypeName, StringComparison.OrdinalIgnoreCase));
            }

            /// <summary>
            /// Returns hash code for the proxy operation.
            /// </summary>
            /// <param name="obj">The object to generate the hash code for</param>
            /// <returns></returns>
            public int GetHashCode(SPProxyOperationType obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
