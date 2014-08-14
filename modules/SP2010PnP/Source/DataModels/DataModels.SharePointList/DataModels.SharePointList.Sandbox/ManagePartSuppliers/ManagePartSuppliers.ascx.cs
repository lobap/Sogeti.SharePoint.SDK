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
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using DataModels.SharePointList.Model;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace DataModels.SharePointList.Sandbox.ManagePartSuppliers
{
    using System.Security.Permissions;
    using DTOs;
    using Microsoft.SharePoint.Security;

    [ToolboxItem(false)]
    public partial class ManagePartSuppliers : System.Web.UI.WebControls.WebParts.WebPart
    {
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            PartSupplierResultsGridView.RowDeleting += SupplierResultsGridView_RowDeleting;
            SupplierResultsGridView.SelectedIndexChanged += SupplierResultsGridView_SelectedIndexChanged;
            GoButton.Click += GoButton_Click;
            AddNewHyperlink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Suppliers, "/NewForm.aspx?IsDlg=1');");
        }


        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.Request.QueryString[Constants.PartSku] != null)
            {
                string partSku = this.Page.Request.QueryString[Constants.PartSku].ToString();
                LoadPartSuppliers(partSku);
            }
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        protected void SupplierResultsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int partSupplierId = int.Parse(PartSupplierResultsGridView.DataKeys[e.RowIndex].Value.ToString());
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                partManagementRepository.DeletePartSupplier(partSupplierId);
                string partSku = this.Page.Request.QueryString[Constants.PartSku].ToString();
                LoadPartSuppliers(partSku);
            }
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
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

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        protected void SupplierResultsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedSupplierId = int.Parse(SupplierResultsGridView.DataKeys[SupplierResultsGridView.SelectedRow.RowIndex].Value.ToString());
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                if (this.Page.Request.QueryString[Constants.PartSku] != null)
                {
                    //Add Part Supplier
                    string partSku = this.Page.Request.QueryString[Constants.PartSku].ToString();
                    partManagementRepository.AddNewPartSupplier(selectedSupplierId, partSku);
                    LoadPartSuppliers(partSku);

                    //Clear Search Results
                    SupplierResultsGridView.DataSource = null;
                    SupplierResultsGridView.DataBind();
                }
            }
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        protected void LoadPartSuppliers(string partSku)
        {
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                IEnumerable<PartSupplier> partSuppliers = partManagementRepository.GetPartSuppliers(partSku);
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
            PartSupplierResultsGridView.EmptyDataText = string.Empty;
        }

        public void ShowSuppliers(IEnumerable<SupplierDTO> supplierDtos)
        {
            SupplierResultsGridView.DataSource = supplierDtos;
            SupplierResultsGridView.EmptyDataText = Constants.EmptyData.SupplierResults;
            SupplierResultsGridView.DataBind();
            SupplierResultsGridView.EmptyDataText = string.Empty;
        }
    }
}
