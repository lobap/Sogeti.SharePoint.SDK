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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using ExecutionModels.Common;
using ExecutionModels.Common.ExceptionHandling;
using ExecutionModels.ExceptionHandling;
using ExecutionModels.Sandboxed.ExternalList.VendorList;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.WebControls;

namespace ExecutionModels.Sandboxed.ExternalList.VendorTransactionList
{
    [ToolboxItemAttribute(false)]
    public class VendorTransactionList : WebPart, IVendorTransactionListView
    {
        const string Gridid = "grid";
        private GridView gridView;
        private VendorTransactionListViewPresenter presenter;
        private IErrorVisualizer errorVisualizer;

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            string VendorID = Page.Request.QueryString["VendorID"];

            gridView = new GridView();
            gridView.ID = Gridid;
            gridView.AutoGenerateColumns = false;
            gridView.Width = 550;
            gridView.HeaderStyle.BackColor = System.Drawing.Color.LightGray;
            gridView.HeaderStyle.Font.Bold = true;
            gridView.Columns.Clear();

            errorVisualizer = new ErrorVisualizer(this);
            presenter = new VendorTransactionListViewPresenter(this);
            presenter.VendorId = VendorID;
            presenter.ErrorVisualizer = errorVisualizer;

            //Load the data
            presenter.SetVendorDetails();
            

            Controls.Add(gridView);
        }


        //Databind in a setter?
        public DataTable VendorTransaction
        {
            set
            {
                PresenterUtilities.FormatGridDisplay(gridView, value);
                gridView.DataSource = value;
                gridView.DataBind();
            }
            get
            {
                return VendorTransaction;
            }
        }
    }
}
