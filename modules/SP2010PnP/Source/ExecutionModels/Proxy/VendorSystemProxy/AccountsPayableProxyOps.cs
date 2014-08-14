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
using System.Security.Permissions;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.UserCode;
using VendorSystemProxy;
using System.Diagnostics;
using System.ServiceModel;
using Vendor.Services.Contract;


namespace VendorSystemProxy
{
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    public class AccountsPayableProxyOps : SPProxyOperation
    {
        private const string address = "http://localhost:81/Vendor/service.svc";

        public override object Execute(SPProxyOperationArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            try
            {
                AccountsPayableProxyArgs proxyArgs = args as AccountsPayableProxyArgs;

                string vendorName = proxyArgs.VendorName;
                double accountsPayable = 0.0;

                BasicHttpBinding binding = new BasicHttpBinding();
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
                binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                EndpointAddress endpointAddress = new EndpointAddress(address);
                using (ChannelFactory<IVendorServices> factory = new ChannelFactory<IVendorServices>(binding, endpointAddress))
                {
                    IVendorServices proxy = factory.CreateChannel();
                    accountsPayable = proxy.GetAccountsPayable(vendorName);
                    factory.Close();
                }

                return accountsPayable;
            }
            catch (Exception excpt)
            {
                return excpt;
            }
        }
    }
}
