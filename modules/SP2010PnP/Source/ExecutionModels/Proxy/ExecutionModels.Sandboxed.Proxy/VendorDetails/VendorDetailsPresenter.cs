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
using ExecutionModels.Common.ExceptionHandling;
using Microsoft.SharePoint.Security;
using VendorSystemProxy;

namespace ExecutionModels.Sandboxed.Proxy.VendorDetails
{
    class VendorDetailsPresenter
    {
        private IVendorDetails view;
        private VendorService vendorService;

        private string vendorName;
        public string VendorName 
        {
            get { return (vendorName); }
            set { vendorName = value; }
        }

        public VendorDetailsPresenter(IVendorDetails view, VendorService vendorService)
        {
            this.view = view;
            this.vendorService = vendorService;
        }

        private IErrorVisualizer errorVisualizer;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualizer")]
        public IErrorVisualizer ErrorVisualizer 
        {
            get { return (errorVisualizer); }
            set { errorVisualizer = value; }
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void SetVendorDetails()
        {
            try
            {
                AccountsPayableProxyArgs proxyArgs = new AccountsPayableProxyArgs();
                proxyArgs.VendorName  = vendorName;

                string assemblyName = proxyArgs.ProxyOpsAssemblyName;

                 view.AccountsPayable = vendorService.GetVendorAccountsPayable(proxyArgs, assemblyName);

            }
            catch (Exception ex)
            {
                // If an unhandled exception occurs in the view, then instruct the ErrorVisualizer to replace
                // the view with an errormessage. 
                ViewExceptionHandler viewExceptionHandler = new ViewExceptionHandler();
                viewExceptionHandler.HandleViewException(ex, this.ErrorVisualizer);
            }
        }
    }

}
