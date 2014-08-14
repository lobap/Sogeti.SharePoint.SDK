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
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataModels.SharePointList.PartsMgmnt.ViewModels;
using DataModels.SharePointList.Model;
using Microsoft.SharePoint;
 

namespace DataModels.SharePointList.PartsMgmnt.PartsMgmntControls
{

    public partial class SupplierResults : UserControl
    {
        public event EventHandler<GenericEventArgs<Supplier>> OnSupplierSelected;
        public event EventHandler<GenericEventArgs<IEnumerable<Part>>> OnRelatedPartsFound;

        public SupplierResults()
        {
            OnSupplierSelected += delegate { };
        }
        public bool AllowParts { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowSelect { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SupplierResultsGridView.RowDataBound += SupplierResultsGridView_RowDataBound;
            SupplierResultsGridView.RowCommand += SupplierResultsGridView_RowCommand;
            SupplierResultsGridView.SelectedIndexChanged += SupplierResultsGridView_SelectedIndexChanged;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void SupplierResultsGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            using (var partManagementRepository = new PartManagementRepository())
            {

                switch (e.CommandName)
                {
                    case "ViewParts":
                        var selectedSupplierId =
                            int.Parse(
                                SupplierResultsGridView.DataKeys[int.Parse(e.CommandArgument.ToString())].Value.ToString
                                    ());
                        if (OnRelatedPartsFound != null)
                        {
                            RelatedPartsFound(new GenericEventArgs<IEnumerable<Part>>
                                                  {
                                                      PayLoad = partManagementRepository.GetPartsBySupplierId(selectedSupplierId)
                                                  });
                        }

                        break;
                }
            }
        }


        protected void SupplierResultsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedSupplierId = int.Parse(SupplierResultsGridView.DataKeys[SupplierResultsGridView.SelectedRow.RowIndex].Value.ToString());
            using (var partManagementRepository = new PartManagementRepository())
            {

                if (OnSupplierSelected != null)
                {
                    SupplierSelected(new GenericEventArgs<Supplier>
                                         {
                                             PayLoad = partManagementRepository.GetSupplier(selectedSupplierId)
                                         });
                }
            }

        }


        protected virtual void SupplierSelected(GenericEventArgs<Supplier> e)
        {
            OnSupplierSelected(this, e);
        }
     

        public IEnumerable<Supplier> Data
        {
            set
            {
                var supplierResultsViewModels = value.Select(supplier => new SupplierResultsViewModel
                     {
                         Id = supplier.Id.HasValue ? supplier.Id.Value : 0, 
                         SupplierName = supplier.Title, 
                         DUNS = supplier.DUNS, 
                         Rating = supplier.Rating.HasValue ? supplier.Rating.Value : 0.00
                     }).ToList();

                FormatGrid(); 

                SupplierResultsGridView.DataSource = supplierResultsViewModels;
                SupplierResultsGridView.DataBind(); 
            }
        }

        public void ClearControls()
        {
            SupplierResultsGridView.DataSource = null;
            SupplierResultsGridView.DataBind();
            Update();
        }

        public void Update()
        {
            SupplierResultUpdatePanel.Update();
        }

        protected void SupplierResultsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink SupplierNameLink = (HyperLink)e.Row.FindControl("SupplierNameHyperLink");
                string Id = SupplierResultsGridView.DataKeys[e.Row.RowIndex].Value.ToString();
                SupplierNameLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/",SharePointList.Model.Constants.ListUrls.Suppliers,"/EditForm.aspx?ID=" + Id + "&IsDlg=1');");
            }
        }

        protected void FormatGrid()
        {
            if (!AllowParts)
            {
                SupplierResultsGridView.Columns[5].Visible = false;
            }
            if (!AllowEdit)
            {
                SupplierResultsGridView.Columns[1].Visible = false;
            }
            if (!AllowSelect)
            {
                SupplierResultsGridView.Columns[2].Visible = false;
            }
        }


        protected virtual void RelatedPartsFound(GenericEventArgs<IEnumerable<Part>> e)
        {
            OnRelatedPartsFound(this, e);
        }

    }
}
