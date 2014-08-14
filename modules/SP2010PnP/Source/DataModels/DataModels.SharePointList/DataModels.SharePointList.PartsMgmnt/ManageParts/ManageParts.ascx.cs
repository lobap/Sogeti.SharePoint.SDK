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
using Microsoft.SharePoint;

namespace DataModels.SharePointList.PartsMgmnt.PartsMgmntControls
{
    using DTOs;
    using Microsoft.Practices.SharePoint.Common.ServiceLocation;


    public partial class ManageParts : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PartResultsGridView.RowDataBound += PartResultsGridView_RowDataBound;
            GoButton.Click += GoButton_Click;
            AddNewHyperlink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Parts, "/NewForm.aspx?IsDlg=1');");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void GoButton_Click(object sender, EventArgs eventArgs)
        {
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                IEnumerable<PartInventoryDTO> partsData = partManagementRepository.GetPartsInventoryView(PartSearchTextBox.Text);
                ShowParts(partsData);
            }

        }

        public void ShowParts(IEnumerable<PartInventoryDTO> partInventoryDtos)
        {
            PartResultsGridView.EmptyDataText = Constants.EmptyData.PartResults;
            PartResultsGridView.DataSource = partInventoryDtos;
            PartResultsGridView.DataBind();
            PartResultUpdatePanel.Update();
            PartResultsGridView.EmptyDataText = string.Empty;
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
