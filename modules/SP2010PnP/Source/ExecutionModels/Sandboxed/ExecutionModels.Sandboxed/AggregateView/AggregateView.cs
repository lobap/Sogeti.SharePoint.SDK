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
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using ExecutionModels.Common;
using ExecutionModels.ExceptionHandling;
using Microsoft.Practices.SharePoint.Common;
using ExecutionModels.Common.ExceptionHandling;
using Microsoft.SharePoint.Security;

namespace ExecutionModels.Sandboxed.AggregateView
{
    [ToolboxItemAttribute(false)]
    public class AggregateView : WebPart, IAggregateView
    {
        const string Gridid = "grid";
        private GridView gridView;
        private AggregateViewPresenter presenter;

        protected override void OnInit(EventArgs e)
        {
            gridView = new GridView();
            gridView.RowDataBound += new GridViewRowEventHandler(GridView_RowDataBound);

            base.OnInit(e);
        }

        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            const int estimateValueCellIndex = 2;

            DataControlRowType rowType = e.Row.RowType;

            if (rowType == DataControlRowType.DataRow)
            {
                TableCell cell = e.Row.Cells[estimateValueCellIndex];

                double currencyValue;
                if (double.TryParse(cell.Text, out currencyValue))
                {
                    cell.Text = String.Format("{0:C}", currencyValue);
                }
            }
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            gridView.ID = Gridid;
            gridView.AllowPaging = false;
            gridView.AutoGenerateColumns = false;
            gridView.AllowSorting = true;
            gridView.Width = 550;
            gridView.HeaderStyle.BackColor = System.Drawing.Color.LightGray;
            gridView.HeaderStyle.Font.Bold = true;
            gridView.Columns.Clear();

            presenter = new AggregateViewPresenter(this, new EstimatesService());           
            presenter.SetSiteData();

            Controls.Add(gridView);

            IErrorVisualizer errorVisualizer = new ErrorVisualizer(this);
            presenter.ErrorVisualizer = errorVisualizer;
        }

        public DataTable SetSiteData
        {
            set
            {
                PresenterUtilities.FormatGridDisplay(gridView, value);

                gridView.DataSource = value;
                gridView.DataBind();
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
                if (gridView != null)
                {
                    DataTable data = (gridView.DataSource as DataTable);
                    if (data != null) data.Dispose();
                    gridView.Dispose();
                }
                base.Dispose();
            }
            

        }
    }
}


