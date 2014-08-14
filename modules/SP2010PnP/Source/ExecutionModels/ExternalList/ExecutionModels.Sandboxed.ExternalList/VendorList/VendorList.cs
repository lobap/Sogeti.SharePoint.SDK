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
using System.Data;
using System.Security.Permissions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using ExecutionModels.Common;
using ExecutionModels.ExceptionHandling;
using ExecutionModels.Common.ExceptionHandling;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace ExecutionModels.Sandboxed.ExternalList.VendorList
{
    [ToolboxItemAttribute(false)]
    public class VendorList : WebPart, IVendorListView
    {
        const string Gridid = "grid";
        private GridView gridView = new GridView();
        private VendorListViewPresenter presenter;

        public VendorList()
        {

        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            gridView.ID = Gridid;
            gridView.AutoGenerateColumns = false;
            gridView.Width = 550;
            gridView.HeaderStyle.BackColor = System.Drawing.Color.LightGray;
            gridView.HeaderStyle.Font.Bold = true;
            gridView.Columns.Clear();

            presenter = new VendorListViewPresenter(this);
            presenter.SetVendorDataWithTransactionCount();

            Controls.Add(gridView);

            IErrorVisualizer errorVisualizer = new ErrorVisualizer(this);
            presenter.ErrorVisualizer = errorVisualizer;
        }


        //Data binding in a setter?
        public DataTable VendorDataWithTransactionCount
        {
            set
            {
                PresenterUtilities.FormatGridDisplay(gridView, value);

                gridView.DataSource = value;
                gridView.DataBind();
            }
            get
            {
                return gridView.DataSource as DataTable;
            }
        }




        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            gridView.RowDataBound += new GridViewRowEventHandler(GridView_RowDataBound);
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            const int VendorIdCellIndex = 0;
            const int TransactionCountCellIndex = 9;
            DataControlRowType rowType = e.Row.RowType;

            switch (rowType)
            {
                case DataControlRowType.DataRow:
                    {
                        TableCell vendorIdCell = e.Row.Cells[VendorIdCellIndex];
                        TableCell transactionCountCell = e.Row.Cells[TransactionCountCellIndex];

                        //Build Hyperlink
                        HyperLink transactionCountHyperLink = new HyperLink();
                        transactionCountHyperLink.Font.Size = FontUnit.Small;
                        transactionCountHyperLink.Text = transactionCountCell.Text;
                        transactionCountHyperLink.NavigateUrl = string.Concat("javascript: ShowVendorDetailsDialog('",
                                                                           SPContext.Current.Site.RootWeb.Url,
                                                                           "/Lists/ModalPages/VendorTransactionDetail.aspx?VendorID=" +
                                                                           vendorIdCell.Text + "');"
                            );

                        //Add New Cell with Transaction Count
                        transactionCountCell.HorizontalAlign = HorizontalAlign.Center;
                        transactionCountCell.Controls.Add(transactionCountHyperLink);
                        e.Row.Cells.Add(transactionCountCell);

                    }
                    break;
            }
        }


        public int TransactionCount { get; set; }

        public sealed override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DataTable data = (gridView.DataSource as DataTable);
                if (data != null) data.Dispose();

                gridView.Dispose();

            }
            base.Dispose();
        }


    }
}


