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

namespace ExecutionModels.Sandboxed.ExternalList.VendorList
{
    public class VendorListViewPresenter
    {
        private IVendorListView view;
        private IVendorService VendorService;

        public IErrorVisualizer ErrorVisualizer { get; set; }
        public ViewExceptionHandler ExceptionHandler { get; set; }

        public VendorListViewPresenter(IVendorListView view)
            : this(view, new VendorService())
        {
        }


        protected VendorListViewPresenter(IVendorListView view, IVendorService VendorService)
        {
            this.view = view;
            this.VendorService = VendorService;
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void SetVendorDataWithTransactionCount()
        {
            try
            {
                view.VendorDataWithTransactionCount = VendorService.GetAllVendorsWithTransactionCount();
            }
            catch (Exception ex)
            {
                // If an unhandled exception occurs in the view, then instruct the ErrorVisualizer to replace
                // the view with an errormessage. 
                ViewExceptionHandler viewExceptionHandler = this.ExceptionHandler ?? new ViewExceptionHandler();
                viewExceptionHandler.HandleViewException(ex, this.ErrorVisualizer);
            }
        }


    }
}


