//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Security.Permissions;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using VendorSystemProxy;

namespace ExecutionModels.Sandboxed.Proxy.VendorDetails
{
    public class VendorService: IVendorService
    {
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public double GetVendorAccountsPayable(AccountsPayableProxyArgs proxyArgs, string assemblyName)
        {
            var result = SPUtility.ExecuteRegisteredProxyOperation(
                     assemblyName,
                     proxyArgs.ProxyOpsTypeName,
                     proxyArgs);

            if (result.GetType().IsSubclassOf(typeof(Exception))) { throw result as Exception; }

            double cred = (double) result;
            return (cred);
        }
    }
}
