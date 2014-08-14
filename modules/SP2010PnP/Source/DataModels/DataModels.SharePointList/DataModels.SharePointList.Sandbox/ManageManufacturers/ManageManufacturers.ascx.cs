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
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using DataModels.SharePointList.Model;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace DataModels.SharePointList.Sandbox.ManageManufacturers
{
    using System.Security.Permissions;
    using DTOs;
    using Microsoft.SharePoint.Security;

    [ToolboxItem(false)]
    public partial class ManageManufacturers : System.Web.UI.WebControls.WebParts.WebPart
    {
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
 GoButton.Click += GoButton_Click;
            ManufacturerResultsGridView.RowDataBound += ManufacturerResultsGridView_RowDataBound;
            ManufacturerResultsGridView.SelectedIndexChanged += ManufacturerResultsGridView_SelectedIndexChanged;
            MachineResultsGridView.RowDataBound += MachineResultsGridView_RowDataBound;
            AddNewHyperlink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Manufacturers,
                                                        "/NewForm.aspx?IsDlg=1');");

        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        private void GoButton_Click(object sender, EventArgs eventArgs)
        {
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                IEnumerable<Manufacturer> manufacturers =
                    partManagementRepository.GetManufacturersByPartialName(ManufacturerSearchTextBox.Text);
                var manufacturerDtos = manufacturers.Select(manufacturer => new ManufacturerDTO
                {
                    ManufacturerId =
                        manufacturer.Id.HasValue
                            ? manufacturer.Id.
                                  Value
                            : 0,
                    ManufacturerName =
                        manufacturer.Title,
                });
                ShowManufacturers(manufacturerDtos);
            }
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        protected void ManufacturerResultsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink manufacturerNameLink = (HyperLink)e.Row.FindControl("ManufacturerNameHyperLink");
                string manufacturerId = ManufacturerResultsGridView.DataKeys[e.Row.RowIndex].Value.ToString();
                manufacturerNameLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                                 SPContext.Current.Site.RootWeb.Url,
                                                                 "/",
                                                                 SharePointList.Model.Constants.ListUrls.Manufacturers,
                                                                 "/EditForm.aspx?ID=" + manufacturerId + "&IsDlg=1');");
            }
        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        protected void ManufacturerResultsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedManufacturerId = int.Parse(ManufacturerResultsGridView.DataKeys[ManufacturerResultsGridView.SelectedRow.RowIndex].Value.ToString());
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                IEnumerable<MachineDTO> machines = partManagementRepository.GetMachinesByManufacturer(selectedManufacturerId);
                ShowMachines(machines);
            }

        }

        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        protected void MachineResultsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink machineNameLink = (HyperLink)e.Row.FindControl("MachineNameHyperLink");
                string machineId = MachineResultsGridView.DataKeys[e.Row.RowIndex].Value.ToString();
                machineNameLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                       "/", SharePointList.Model.Constants.ListUrls.Machines, "/EditForm.aspx?ID=" + machineId + "&IsDlg=1');");
            }
        }

        public void ShowMachines(IEnumerable<MachineDTO> machineDtos)
        {
            MachineResultsGridView.DataSource = machineDtos;
            MachineResultsGridView.EmptyDataText = Constants.EmptyData.MachineManufacturerResults;
            MachineResultsGridView.DataBind();
            MachineResultsGridView.EmptyDataText = string.Empty;
        }

        public void ShowManufacturers(IEnumerable<ManufacturerDTO> manufacturerDtos)
        {
            ManufacturerResultsGridView.DataSource = manufacturerDtos;
            ManufacturerResultsGridView.EmptyDataText = Constants.EmptyData.ManufacturerResults;
            ManufacturerResultsGridView.DataBind();
            ManufacturerResultsGridView.EmptyDataText = string.Empty;
        }
    }
}
