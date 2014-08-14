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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using DataModels.SharePointList.PartsMgmnt.ViewModels;
using DataModels.SharePointList.Model;
using Microsoft.SharePoint;

namespace DataModels.SharePointList.PartsMgmnt.PartsMgmntControls
{
    public partial class PartSupplierResults : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SupplierResultsGridView.RowDeleting += SupplierResultsGridView_RowDeleting;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public IEnumerable<PartSupplier> Data
        {
            set
            {
                var supplierResultsViewModels = value.Select(partSupplier => new SupplierResultsViewModel
                     {
                         Id = partSupplier.Id.HasValue ? partSupplier.Id.Value : 0,
                         SupplierName = partSupplier.Supplier.Title,
                         DUNS = partSupplier.Supplier.DUNS,
                         Rating = partSupplier.Supplier.Rating.HasValue ? partSupplier.Supplier.Rating.Value : 0.00
                     }).ToList();

                SupplierResultsGridView.DataSource = supplierResultsViewModels;
                SupplierResultsGridView.DataBind(); 
            }
        }

        public void ClearControls()
        {
            SupplierResultsGridView.DataSource = null;
            SupplierResultsGridView.DataBind();
            Update();
        }

        public void Update()
        {
            SupplierResultUpdatePanel.Update();
        }


        protected void SupplierResultsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int partSupplierId = int.Parse(SupplierResultsGridView.DataKeys[e.RowIndex].Value.ToString());
            using (var partManagementRepository = new PartManagementRepository())
            {
                partManagementRepository.DeletePartSupplier(partSupplierId);
                Update();
            }
        }

    }
}
