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
using DataModels.SharePointList.PartsMgmnt.Presenters;
using DataModels.SharePointList.Model;
using Microsoft.SharePoint;

namespace DataModels.SharePointList.PartsMgmnt.PartsMgmntControls
{
    public partial class PartInventoryResults : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            PartInventoryResultsGridView.RowDataBound += PartInventoryResultsGridView_RowDataBound;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public IEnumerable<Part> Data
        {
            set
            {
                PartResultsPresenter partResultsPresenter = new PartResultsPresenter();
                PartInventoryResultsGridView.DataSource = partResultsPresenter.GetViewModels(value);
                PartInventoryResultsGridView.DataBind(); ;
            }
        }

        public void ClearControls()
        {
            PartInventoryResultsGridView.DataSource = null;
            PartInventoryResultsGridView.DataBind();
            Update();
        }

        public void Update()
        {
            PartResultUpdatePanel.Update();
        }

        protected void PartInventoryResultsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink selectLink = (HyperLink)e.Row.FindControl("SelectHyperLink");
                string inventoryLocationId = PartInventoryResultsGridView.DataKeys[e.Row.RowIndex].Value.ToString();
                selectLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.InventoryLocations,"/EditForm.aspx?ID=" + inventoryLocationId + "&IsDlg=1');");
            }
        }

    }
}
