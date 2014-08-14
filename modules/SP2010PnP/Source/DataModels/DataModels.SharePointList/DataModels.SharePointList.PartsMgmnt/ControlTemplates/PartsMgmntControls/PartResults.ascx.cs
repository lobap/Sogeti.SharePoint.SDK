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
using Constants = DataModels.SharePointList.PartsMgmnt.Constants;

namespace DataModels.SharePointList.PartsMgmnt.PartsMgmntControls
{
    public partial class PartResults : UserControl
    {
        public event EventHandler<GenericEventArgs<int>> OnGetNextPage;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            PartResultsGridView.RowDataBound += PartResultsGridView_RowDataBound;
            PartResultsGridView.AllowPaging = true;
            PartResultsGridView.PageSize = Constants.DeafultPageSize;
            PartResultsGridView.PageIndexChanging += PartResultsGridView_PageIndexChanging;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public IEnumerable<Part> Data
        {
            set
            {
                var partResultsPresenter = new PartResultsPresenter();
                PartResultsGridView.DataSource = partResultsPresenter.GetViewModels(value);
                PartResultsGridView.DataBind();
            }
        }

        public void ClearControls()
        {
            PartResultsGridView.DataSource = null;
            PartResultsGridView.DataBind();
            Update();
        }

        public void Update()
        {
            PartResultUpdatePanel.Update();
        }

        protected void PartResultsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string partId = PartResultsGridView.DataKeys[e.Row.RowIndex].Values[0].ToString();
                string partSku = PartResultsGridView.DataKeys[e.Row.RowIndex].Values[1].ToString();

                var partNameLink = (HyperLink)e.Row.FindControl("PartNameHyperLink");
                partNameLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Parts, "/EditForm.aspx?ID=" + partId + "&IsDlg=1');");

                var machineLink = (HyperLink)e.Row.FindControl("MachineHyperLink");
                machineLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/",Constants.PartsMgmntLibrary,"/ManagePartMachines.aspx?" + Constants.PartSku + "=" + partSku + "&IsDlg=1');");

                var supplierLink = (HyperLink)e.Row.FindControl("SupplierHyperLink");
                supplierLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/",Constants.PartsMgmntLibrary,"/ManagePartSuppliers.aspx?" + Constants.PartSku + "=" + partSku + "&IsDlg=1');");

                var locationLink = (HyperLink)e.Row.FindControl("LocationHyperLink");
                locationLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/",Constants.PartsMgmntLibrary,"/ManagePartLocations.aspx?" + Constants.PartSku + "=" + partSku + "&IsDlg=1');");
            }
        }

        protected void PartResultsGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (OnGetNextPage != null)
            {
                var partsEventArgs = new GenericEventArgs<int>();
                partsEventArgs.PayLoad = e.NewPageIndex;
                GetNextPage(partsEventArgs);
            }
        }
        protected virtual void GetNextPage(GenericEventArgs<int> e)
        {
            OnGetNextPage(this, e);
        }
    }
}
