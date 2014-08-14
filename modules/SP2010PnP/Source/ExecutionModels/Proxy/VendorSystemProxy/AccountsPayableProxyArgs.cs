//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Security;
using Microsoft.SharePoint.UserCode;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;

namespace VendorSystemProxy
{
    [Serializable]
    public class AccountsPayableProxyArgs : SPProxyOperationArgs
    {
        public static readonly string AssemblyName = "VendorSystemProxy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7d7cc5a694ad3e5d";
        public static readonly string TypeName = "VendorSystemProxy.AccountsPayableProxyOps";

        private string vendorName;
        public string VendorName 
        {
            get { return (this.vendorName); }
            set { this.vendorName = value; } 
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public AccountsPayableProxyArgs(string vendorName)
        {
            this.vendorName = vendorName;
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public AccountsPayableProxyArgs()
        {
        }

        public string ProxyOpsAssemblyName
        {
            get { return AssemblyName; }
        }
        public string ProxyOpsTypeName
        {
            get { return TypeName; }
        }
    }
}
