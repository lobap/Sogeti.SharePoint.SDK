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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace DataModels.SharePointList.Sandbox.ManageInventory
{
    using System.Security.Permissions;
    using DTOs;
    using Microsoft.SharePoint.Security;

    [ToolboxItem(false)]
    public partial class ManageInventory : System.Web.UI.WebControls.WebParts.WebPart
    {
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            GoButton.Click += GoButton_Click;
            PartInventoryResultsGridView.RowDataBound += PartInventoryResultsGridView_RowDataBound;
            AddNewHyperlink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Parts, "/NewForm.aspx?IsDlg=1');");
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
                IEnumerable<PartInventoryDTO> partsData = partManagementRepository.GetPartsInventoryView(PartSearchTextBox.Text);
                ShowParts(partsData);
            }
        }

        public void ShowParts(IEnumerable<PartInventoryDTO> partInventoryDtos)
        {

            PartInventoryResultsGridView.DataSource = partInventoryDtos;
            PartInventoryResultsGridView.EmptyDataText = Constants.EmptyData.PartResults;
            PartInventoryResultsGridView.DataBind();
            PartInventoryResultsGridView.EmptyDataText = string.Empty;
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
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
