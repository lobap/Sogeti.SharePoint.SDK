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
    public partial class ManufacturerResults : UserControl
    {
        public event EventHandler<GenericEventArgs<IEnumerable<Machine>>> OnRelatedMachinesFound;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ManufacturerResultsGridView.RowDataBound += ManufacturerResultsGridView_RowDataBound;
            ManufacturerResultsGridView.SelectedIndexChanged += ManufacturerResultsGridView_SelectedIndexChanged;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public IEnumerable<Manufacturer> Data
        {
            set
            {
                var manufacturerResultsViewModels = value.Select(manufacturer => new ManufacturerResultsViewModel
                {
                    ManufacturerId = manufacturer.Id.HasValue ? manufacturer.Id.Value : 0,
                    ManufacturerName = manufacturer.Title,
                }).ToList();

                ManufacturerResultsGridView.DataSource = manufacturerResultsViewModels;
                ManufacturerResultsGridView.DataBind(); ;
            }
        }

        public void ClearControls()
        {
            ManufacturerResultsGridView.DataSource = null;
            ManufacturerResultsGridView.DataBind();
            Update();
        }

        public void Update()
        {
            ManufacturerResultUpdatePanel.Update();
        }

        protected void ManufacturerResultsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink manufacturerNameLink = (HyperLink)e.Row.FindControl("ManufacturerNameHyperLink");
                string manufacturerId = ManufacturerResultsGridView.DataKeys[e.Row.RowIndex].Value.ToString();
                manufacturerNameLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Manufacturers, "/EditForm.aspx?ID=" + manufacturerId + "&IsDlg=1');");
            }
        }

        protected void ManufacturerResultsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedManufacturerId = int.Parse(ManufacturerResultsGridView.DataKeys[ManufacturerResultsGridView.SelectedRow.RowIndex].Value.ToString());
            using (var partManagementRepository = new PartManagementRepository())
            {

                if (OnRelatedMachinesFound != null)
                {
                    MachineResultsReturned(new GenericEventArgs<IEnumerable<Machine>>
                                               {
                                                   PayLoad =
                                                       partManagementRepository.GetMachinesByManufacturer(selectedManufacturerId)
                                               });
                }
            }

        }

        protected virtual void MachineResultsReturned(GenericEventArgs<IEnumerable<Machine>> e)
        {
            OnRelatedMachinesFound(this, e);
        }

    }
}
