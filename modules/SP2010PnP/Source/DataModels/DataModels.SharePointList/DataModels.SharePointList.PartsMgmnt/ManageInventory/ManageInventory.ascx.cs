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

namespace DataModels.SharePointList.PartsMgmnt.ManageInventory
{
    using DTOs;
    using Microsoft.Practices.SharePoint.Common.ServiceLocation;

    public partial class ManageInventoryUserControl : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            GoButton.Click += GoButton_Click;
            PartInventoryResultsGridView.RowDataBound += PartInventoryResultsGridView_RowDataBound;
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
            
            PartInventoryResultsGridView.DataSource = partInventoryDtos;
            PartInventoryResultsGridView.EmptyDataText = Constants.EmptyData.PartResults;
            PartInventoryResultsGridView.DataBind();
            PartResultUpdatePanel.Update();
            PartInventoryResultsGridView.EmptyDataText = string.Empty;
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
    }
}
