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


namespace DataModels.SharePointList.PartsMgmnt.PartsMgmntControls
{
    using System.Linq;
    using DTOs;
    using Microsoft.Practices.SharePoint.Common.ServiceLocation;

    public partial class ManageSuppliers : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            GoButton.Click += GoButton_Click;
            PartResultsGridView.RowDataBound += PartResultsGridView_RowDataBound;
            SupplierResultsGridView.RowDataBound += SupplierResultsGridView_RowDataBound;
            SupplierResultsGridView.RowCommand += SupplierResultsGridView_RowCommand;
            AddNewHyperlink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Suppliers, "/NewForm.aspx?IsDlg=1');");

        }
        protected void Page_Load(object sender, EventArgs e)
        {
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
        public void ShowParts(IEnumerable<PartInventoryDTO> partInventoryDtos)
        {
            PartResultsGridView.EmptyDataText = Constants.EmptyData.PartSupplierResults;
            PartResultsGridView.DataSource = partInventoryDtos;
            PartResultsGridView.DataBind();
            PartResultUpdatePanel.Update();
            PartResultsGridView.EmptyDataText = string.Empty;
        }

        private void SupplierResultsGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                switch (e.CommandName)
                {
                    case "ViewParts":
                        var selectedSupplierId = int.Parse(SupplierResultsGridView.DataKeys[int.Parse(e.CommandArgument.ToString())].Value.ToString());
                        ShowParts(partManagementRepository.GetPartsBySupplierId(selectedSupplierId));
                        break;
                }
            }
        }


        protected void SupplierResultsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink SupplierNameLink = (HyperLink)e.Row.FindControl("SupplierNameHyperLink");
                string Id = SupplierResultsGridView.DataKeys[e.Row.RowIndex].Value.ToString();
                SupplierNameLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Suppliers, "/EditForm.aspx?ID=" + Id + "&IsDlg=1');");
            }
        }

        public void ShowSuppliers(IEnumerable<SupplierDTO> supplierDtos)
        {
            SupplierResultsGridView.DataSource = supplierDtos;
            SupplierResultsGridView.EmptyDataText = Constants.EmptyData.SupplierResults;
            SupplierResultsGridView.DataBind();
            SupplierResultUpdatePanel.Update();
            SupplierResultsGridView.EmptyDataText = string.Empty;
        }

        protected void PartResultsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string partId = PartResultsGridView.DataKeys[e.Row.RowIndex].Values[0].ToString();
                string sku = PartResultsGridView.DataKeys[e.Row.RowIndex].Values[1].ToString();

                var partNameLink = (HyperLink)e.Row.FindControl("PartNameHyperLink");
                partNameLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Parts, "/EditForm.aspx?ID=" + partId + "&IsDlg=1');");

                var machineLink = (HyperLink)e.Row.FindControl("MachineHyperLink");
                machineLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", Constants.PartsMgmntLibrary, "/ManagePartMachines.aspx?" + Constants.Sku + "=" + sku + "&IsDlg=1');");

                var supplierLink = (HyperLink)e.Row.FindControl("SupplierHyperLink");
                supplierLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", Constants.PartsMgmntLibrary, "/ManagePartSuppliers.aspx?" + Constants.Sku + "=" + sku + "&IsDlg=1');");

                var locationLink = (HyperLink)e.Row.FindControl("LocationHyperLink");
                locationLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", Constants.PartsMgmntLibrary, "/ManagePartLocations.aspx?" + Constants.Sku + "=" + sku + "&IsDlg=1');");
            }
        }
    }
}
