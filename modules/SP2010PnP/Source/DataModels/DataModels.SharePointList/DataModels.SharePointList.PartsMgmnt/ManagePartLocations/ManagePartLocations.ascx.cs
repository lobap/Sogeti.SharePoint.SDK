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


namespace DataModels.SharePointList.PartsMgmnt.ControlTemplates.PartsMgmntControls
{
    using DTOs;
    using Microsoft.Practices.SharePoint.Common.ServiceLocation;

    public partial class ManagePartLocations : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString[Constants.Sku] != null)
            {
                string sku = Request.QueryString[Constants.Sku].ToString();
                LoadPartLocations(sku);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PartInventoryResultsGridView.RowDataBound += PartInventoryResultsGridView_RowDataBound;
             SaveButton.Click += SaveButton_Click;

        }

        protected void PartInventoryResultsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink selectLink = (HyperLink)e.Row.FindControl("SelectHyperLink");
                string inventoryLocationId = PartInventoryResultsGridView.DataKeys[e.Row.RowIndex].Value.ToString();
                selectLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.InventoryLocations, "/EditForm.aspx?ID=" + inventoryLocationId + "&IsDlg=1');");
            }
        }

        public void ShowParts(IEnumerable<PartInventoryDTO> partInventoryDtos)
        {
            PartInventoryResultsGridView.EmptyDataText = Constants.EmptyData.PartLocationResults;
            PartInventoryResultsGridView.DataSource = partInventoryDtos;
            PartInventoryResultsGridView.DataBind();
            PartResultUpdatePanel.Update();
            PartInventoryResultsGridView.EmptyDataText = string.Empty;
        }

        public void ClearControls()
        {
            BinTextBox.Text = string.Empty;
            QuantityTextBox.Text = string.Empty;

            UpdatePanel1.Update();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                if (Request.QueryString[Constants.Sku] != null)
                {
                    //Add the new record
                    partManagementRepository.AddNewInventoryLocationToPart(Request.QueryString[Constants.Sku], double.Parse(QuantityTextBox.Text), BinTextBox.Text);
                    
                    //Clear Inputs
                    ClearControls();
                   
                    //Rebind Grid
                    LoadPartLocations(Request.QueryString[Constants.Sku]);
                }
            }
        }

        protected void LoadPartLocations(string sku)
        {
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                IEnumerable<PartInventoryDTO> partsData = partManagementRepository.GetPartsInventoryView(sku);
                ShowParts(partsData);
            }
        }
    }
}
