//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System.ComponentModel;
using System.Security.Permissions;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using ExecutionModels.Common.ExceptionHandling;
using System.Web.UI.WebControls;
using ExecutionModels.ExceptionHandling;
using Microsoft.SharePoint.Security;

namespace ExecutionModels.Sandboxed.Proxy.VendorDetails
{
    [ToolboxItemAttribute(false)]
    public class VendorDetails : WebPart, IVendorDetails
    {
        private Label PayablesLabel = new Label();
        private Label PayablesValueLabel = new Label();
        private Label VendorNameLabel = new Label();
        private Label VendorNameValueLabel = new Label();
        private Label VendorIdLabel = new Label();
        private Label VendorIdValueLabel = new Label();
        private VendorDetailsPresenter presenter;

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            string vendorName = Page.Request.QueryString["VendorName"];

            VendorNameLabel.Visible = true;
            VendorNameLabel.Text = "Vendor Name: ";
            Controls.Add(VendorNameLabel);

            if (! string.IsNullOrEmpty(vendorName))
            {
                VendorNameValueLabel.Visible = true;
                VendorNameValueLabel.Text = vendorName;
                Controls.Add(VendorNameValueLabel);
            }

            LiteralControl br2 = new LiteralControl("<BR/>");
            Controls.Add(br2);

            PayablesLabel.Visible = true;
            PayablesLabel.Text = "Accounts Payable: ";
            Controls.Add(PayablesLabel);

            PayablesValueLabel.Visible = true;
            Controls.Add(PayablesValueLabel);

            IErrorVisualizer errorVisualizer = new ErrorVisualizer(this);

            presenter = new VendorDetailsPresenter(this, new VendorService());
            presenter.VendorName = vendorName;
            presenter.ErrorVisualizer = errorVisualizer;
            presenter.SetVendorDetails();
        }

        public double AccountsPayable
        {
            set 
            {
                PayablesValueLabel.Text = string.Format("{0:C}", value);
            }
        }
    }
}


