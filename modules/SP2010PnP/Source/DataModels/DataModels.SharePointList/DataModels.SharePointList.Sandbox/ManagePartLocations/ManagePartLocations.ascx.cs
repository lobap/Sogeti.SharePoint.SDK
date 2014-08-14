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
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace DataModels.SharePointList.Sandbox.ManagePartLocations
{
    using System.Security.Permissions;
    using DTOs;
    using Microsoft.SharePoint.Security;

    [ToolboxItem(false)]
    public partial class ManagePartLocations : System.Web.UI.WebControls.WebParts.WebPart
    {
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            PartInventoryResultsGridView.RowDataBound += PartInventoryResultsGridView_RowDataBound;
            SaveButton.Click += SaveButton_Click;

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

        public void ShowParts(IEnumerable<PartInventoryDTO> partInventoryDtos)
        {
            PartInventoryResultsGridView.EmptyDataText = Constants.EmptyData.PartLocationResults;
            PartInventoryResultsGridView.DataSource = partInventoryDtos;
            PartInventoryResultsGridView.EmptyDataText = string.Empty;
        }

        public void ClearControls()
        {
            BinTextBox.Text = string.Empty;
            QuantityTextBox.Text = string.Empty;
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                if (this.Page.Request.QueryString[Constants.PartSku] != null)
                {
                    //Add the new record
                    partManagementRepository.AddNewInventoryLocationToPart(this.Page.Request.QueryString[Constants.PartSku], double.Parse(QuantityTextBox.Text), BinTextBox.Text);

                    //Clear Inputs
                    ClearControls();

                    //Rebind Grid
                    LoadPartLocations(this.Page.Request.QueryString[Constants.PartSku]);
                }
            }
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        protected void LoadPartLocations(string partSku)
        {
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                IEnumerable<PartInventoryDTO> partsData = partManagementRepository.GetPartsInventoryView(partSku);
                ShowParts(partsData);
            }
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.Request.QueryString[Constants.PartSku] != null)
            {
                string partSku = this.Page.Request.QueryString[Constants.PartSku].ToString();
                LoadPartLocations(partSku);
            }
        }
    }
}
