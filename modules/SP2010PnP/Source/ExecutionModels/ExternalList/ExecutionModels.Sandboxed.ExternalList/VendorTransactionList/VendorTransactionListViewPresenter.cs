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
using ExecutionModels.Sandboxed.ExternalList.VendorList;
using Microsoft.SharePoint.Security;

namespace ExecutionModels.Sandboxed.ExternalList.VendorTransactionList
{
    public class VendorTransactionListViewPresenter
    {
        private IVendorTransactionListView view;
        private IVendorService VendorService;

        private string vendorId;
        public string VendorId 
        {
            get { return (vendorId); }
            set { vendorId = value; }
        }

        public VendorTransactionListViewPresenter(IVendorTransactionListView view)  : this (view,new VendorService())
        {
            
        }

        protected VendorTransactionListViewPresenter(IVendorTransactionListView view, IVendorService creditService)
        {
            this.view = view;
            this.VendorService = creditService;
        }

        private IErrorVisualizer errorVisualizer;
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
                if (!string.IsNullOrEmpty(vendorId) && int.Parse(vendorId) > 0)
                {
                    view.VendorTransaction = VendorService.GetTransactionByVendor(int.Parse(vendorId));
                }
                else throw new Exception("Vendor ID Must be Set before access Vendor Details");
                
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


