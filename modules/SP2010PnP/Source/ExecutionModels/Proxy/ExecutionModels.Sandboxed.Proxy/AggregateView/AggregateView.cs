//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using ExecutionModels.Common;
using ExecutionModels.Common.ExceptionHandling;
using ExecutionModels.ExceptionHandling;
using Microsoft.SharePoint;
using System.Data;
using Microsoft.SharePoint.Security;

namespace ExecutionModels.Sandboxed.Proxy.AggregateView
{
    [ToolboxItemAttribute(false)]
    public class AggregateView : WebPart, IAggregateView
    {
        const string Gridid = "grid";

        private GridView gvResults = new GridView();
        private AggregateViewPresenter presenter;
        private Label lblMsg = new Label();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            gvResults.RowDataBound += new GridViewRowEventHandler(GridView_RowDataBound);
        }


        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            const int VendorNameCellIndex = 3;
            const int EstimateValueCellIndex = 2;
       
            DataControlRowType rowType = e.Row.RowType;

            if (rowType == DataControlRowType.DataRow)
            {

                TableCell vendorNameCell = e.Row.Cells[VendorNameCellIndex];

                HyperLink hlControl = new HyperLink();
                hlControl.Text = vendorNameCell.Text;
                hlControl.NavigateUrl = string.Concat("javascript: ShowVendorDetailsDialog('", 
                                                      SPContext.Current.Site.RootWeb.Url,
                                                      "/Lists/Pages/VendorDetail.aspx?VendorName=" + vendorNameCell.Text +
                                                      "');"
                    );

                vendorNameCell.Controls.Add(hlControl);

                TableCell estimateValueCell = e.Row.Cells[EstimateValueCellIndex];

                double currencyValue;

                if(double.TryParse(estimateValueCell.Text, out currencyValue))
                {
                    estimateValueCell.Text = String.Format("{0:C}", currencyValue);
                }
                
            }
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            gvResults.ID = Gridid;
            gvResults.AllowPaging = false;
            gvResults.AutoGenerateColumns = false;
            gvResults.AllowSorting = true;
            gvResults.Width = 550;
            gvResults.HeaderStyle.BackColor = System.Drawing.Color.LightGray;
            gvResults.HeaderStyle.Font.Bold = true;
            gvResults.Columns.Clear();

            IErrorVisualizer errorVisualizer = new ErrorVisualizer(this);

            presenter = new AggregateViewPresenter(this, new EstimatesService());
            presenter.ErrorVisualizer = errorVisualizer;
            presenter.SetSiteData();

            Controls.Add(gvResults);
            Controls.Add(lblMsg);
        }

        public DataTable SetSiteData
        {
            set
            {
                if (value.Rows.Count > 0)
                {
                    PresenterUtilities.FormatGridDisplay(gvResults, value);

                    gvResults.DataSource = value;
                    gvResults.DataBind();
                    gvResults.Visible = true;

                    lblMsg.Visible = false;
                }
                else
                {
                    lblMsg.Text = "There were no rows returned";
                    lblMsg.Visible = true;
                    gvResults.Visible = false;
                }
            }
        }

        public sealed override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(gvResults != null)
                {
                    DataTable data = (gvResults.DataSource as DataTable);
                    if (data != null) data.Dispose();

                    gvResults.Dispose();
                }
            }
        }
    }
}


