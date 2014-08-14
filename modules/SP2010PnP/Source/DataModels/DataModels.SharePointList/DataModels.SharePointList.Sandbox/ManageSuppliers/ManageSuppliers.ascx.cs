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

namespace DataModels.SharePointList.Sandbox.ManageSuppliers
{
    using System.Security.Permissions;
    using DTOs;
    using Microsoft.SharePoint.Security;

    [ToolboxItem(false)]
    public partial class ManageSuppliers : System.Web.UI.WebControls.WebParts.WebPart
    {
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            GoButton.Click += GoButton_Click;
            SupplierResultsGridView.RowDataBound += SupplierResultsGridView_RowDataBound;
            SupplierResultsGridView.RowCommand += SupplierResultsGridView_RowCommand;
            AddNewHyperlink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Suppliers, "/NewForm.aspx?IsDlg=1');");

        }
        protected void Page_Load(object sender, EventArgs e)
        {
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
        public void ShowParts(IEnumerable<PartInventoryDTO> partInventoryDtos)
        {
            PartResultsGridView.EmptyDataText = Constants.EmptyData.PartSupplierResults;
            PartResultsGridView.DataSource = partInventoryDtos;
            PartResultsGridView.DataBind();
            PartResultsGridView.EmptyDataText = string.Empty;
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
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

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
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
            SupplierResultsGridView.EmptyDataText = string.Empty;
        }
    }
}
