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
using System.Web.UI;
using System.Web.UI.WebControls;
using DataModels.SharePointList.Model;
using Microsoft.SharePoint;


namespace DataModels.SharePointList.PartsMgmnt.ControlTemplates.PartsMgmntControls
{
    using System.Linq;
    using DTOs;
    using Microsoft.Practices.SharePoint.Common.ServiceLocation;

    public partial class ManagePartSuppliers : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            PartSupplierResultsGridView.RowDeleting += SupplierResultsGridView_RowDeleting;
            SupplierResultsGridView.SelectedIndexChanged += SupplierResultsGridView_SelectedIndexChanged;
            GoButton.Click += GoButton_Click;
            AddNewHyperlink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Suppliers, "/NewForm.aspx?IsDlg=1');");
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString[Constants.Sku] != null)
            {
                string sku = Request.QueryString[Constants.Sku].ToString();
                LoadPartSuppliers(sku);
            }
        }
        protected void SupplierResultsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int partSupplierId = int.Parse(PartSupplierResultsGridView.DataKeys[e.RowIndex].Value.ToString());
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                partManagementRepository.DeletePartSupplier(partSupplierId);
                string sku = Request.QueryString[Constants.Sku].ToString();
                LoadPartSuppliers(sku);
                PartSupplierResultUpdatePanel.Update();
            }
        }
        private void GoButton_Click(object sender, EventArgs eventArgs)
        {
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                IEnumerable<Supplier> suppliers = partManagementRepository.GetSuppliersByPartialName(SupplierSearchTextBox.Text);
                var supplierDtos = suppliers.Select(supplier => new SupplierDTO
                {
                    Id = supplier.Id.HasValue ? supplier.Id.Value : 0,
                    SupplierName = supplier.Title,
                    DUNS = supplier.DUNS,
                    Rating = supplier.Rating.HasValue ? supplier.Rating.Value : 0.00
                });

                ShowSuppliers(supplierDtos);
            }
        }

        protected void SupplierResultsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedSupplierId = int.Parse(SupplierResultsGridView.DataKeys[SupplierResultsGridView.SelectedRow.RowIndex].Value.ToString());
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                if (Request.QueryString[Constants.Sku] != null)
                {
                    //Add Part Supplier
                    string sku = Request.QueryString[Constants.Sku].ToString();
                    partManagementRepository.AddNewPartSupplier(selectedSupplierId, sku);
                    LoadPartSuppliers(sku);

                    //Clear Search Results
                    SupplierResultsGridView.DataSource = null;
                    SupplierResultsGridView.DataBind();
                    SupplierResultUpdatePanel.Update();
                }
            }
        }



        protected void LoadPartSuppliers(string sku)
        {
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                IEnumerable<PartSupplier> partSuppliers = partManagementRepository.GetPartSuppliers(sku);
                var partSupplierResults = partSuppliers.Select(partSupplier => new SupplierDTO
                {
                    Id = partSupplier.Id.HasValue ? partSupplier.Id.Value : 0,
                    SupplierName = partSupplier.Supplier.Title,
                    DUNS = partSupplier.Supplier.DUNS,
                    Rating = partSupplier.Supplier.Rating.HasValue ? partSupplier.Supplier.Rating.Value : 0.00
                });
                ShowPartSuppliers(partSupplierResults);
            }
        }

        public void ShowPartSuppliers(IEnumerable<SupplierDTO> partSupplierDtos)
        {
            PartSupplierResultsGridView.DataSource = partSupplierDtos;
            PartSupplierResultsGridView.EmptyDataText = Constants.EmptyData.PartSupplierResults;
            PartSupplierResultsGridView.DataBind();

            PartSupplierResultUpdatePanel.Update();

            PartSupplierResultsGridView.EmptyDataText = string.Empty;
        }

        public void ShowSuppliers(IEnumerable<SupplierDTO> supplierDtos)
        {
            SupplierResultsGridView.DataSource = supplierDtos;
            SupplierResultsGridView.EmptyDataText = Constants.EmptyData.SupplierResults;
            SupplierResultsGridView.DataBind();
            SupplierResultUpdatePanel.Update();
            SupplierResultsGridView.EmptyDataText = string.Empty;
        }
    }
}
