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
using DataModels.SharePointList.Model;
using Microsoft.SharePoint;

namespace DataModels.SharePointList.PartsMgmnt.PartsMgmntControls
{
    public partial class SupplierByName : UserControl
    {
        public event EventHandler<GenericEventArgs<IEnumerable<Supplier>>> OnSuppliersFound;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            GoButton.Click += GoButton_Click;
            AddNewHyperlink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Suppliers, "/NewForm.aspx?IsDlg=1');");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void GoButton_Click(object sender, EventArgs eventArgs)
        {
            using (var partManagementRepository = new PartManagementRepository())
            {

                if (OnSuppliersFound != null)
                {
                    SuppliersFound(new GenericEventArgs<IEnumerable<Supplier>>
                                       {
                                           PayLoad = partManagementRepository.GetSuppliersByPartialName(SupplierSearchTextBox.Text)
                                       });
                }
            }

        }

        protected virtual void SuppliersFound(GenericEventArgs<IEnumerable<Supplier>> e)
        {
            OnSuppliersFound(this, e);
        }
    }
}
