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
using DataModels.SharePointList.Model;
using Microsoft.SharePoint;

namespace DataModels.SharePointList.PartsMgmnt.PartsMgmntControls
{
    using System.Linq;
    using DTOs;
    using Microsoft.Practices.SharePoint.Common.ServiceLocation;

    public partial class ManageMachines : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            GoButton.Click += GoButton_Click;
            MachineResultsGridView.RowCommand += machineResultsGridView_RowCommand;
            MachineResultsGridView.RowDataBound += MachineResultsGridView_RowDataBound;
            PartResultsGridView.RowDataBound += PartResultsGridView_RowDataBound;
            AddNewHyperlink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Machines, "/NewForm.aspx?IsDlg=1');");
        }
        private void GoButton_Click(object sender, EventArgs eventArgs)
        {
            PartResultsGridView.DataSource = null;
            PartResultsGridView.DataBind();
            PartResultUpdatePanel.Update();

            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                IEnumerable<Machine> machines = partManagementRepository.GetMachinesByPartialModelNumber(MachineSearchTextBox.Text);
                ShowMachineResults(machines);
            }

        }

        protected void PartResultsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string partId = PartResultsGridView.DataKeys[e.Row.RowIndex].Values[0].ToString();
                string sku = PartResultsGridView.DataKeys[e.Row.RowIndex].Values[1].ToString();

                var partNameLink = (HyperLink)e.Row.FindControl("PartNameHyperLink");
                partNameLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Parts, "/EditForm.aspx?ID=" + partId + "&IsDlg=1');");

                var machineLink = (HyperLink)e.Row.FindControl("MachineHyperLink");
                machineLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", Constants.PartsMgmntLibrary, "/ManagePartMachines.aspx?" + Constants.Sku + "=" + sku + "&IsDlg=1');");

                var supplierLink = (HyperLink)e.Row.FindControl("SupplierHyperLink");
                supplierLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", Constants.PartsMgmntLibrary, "/ManagePartSuppliers.aspx?" + Constants.Sku + "=" + sku + "&IsDlg=1');");

                var locationLink = (HyperLink)e.Row.FindControl("LocationHyperLink");
                locationLink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", Constants.PartsMgmntLibrary, "/ManagePartLocations.aspx?" + Constants.Sku + "=" + sku + "&IsDlg=1');");
            }
        }
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

        public void ShowParts(IEnumerable<PartInventoryDTO> partInventoryDtos)
        {
            PartResultsGridView.EmptyDataText = Constants.EmptyData.MachinePartResults;
            PartResultsGridView.DataSource = partInventoryDtos;
            PartResultsGridView.DataBind();
            PartResultUpdatePanel.Update();
            PartResultsGridView.EmptyDataText = string.Empty;
        }

        private void machineResultsGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var selectedMachineId = int.Parse(MachineResultsGridView.DataKeys[int.Parse(e.CommandArgument.ToString())].Value.ToString());
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {

                switch (e.CommandName)
                {
                    case "ViewParts":
                         ShowParts(partManagementRepository.GetPartsByMachineId(selectedMachineId));
                        break;
                }
            }
        }

        private void ShowMachineResults(IEnumerable<Machine> machines)
        {
            var machineDtos = machines.Select(machine => new MachineDTO
            {
                MachineName = machine.Title,
                Manufacturer =
                    machine.Manufacturer != null ? machine.Manufacturer.Title : string.Empty,
                Id =
                    machine.Id.HasValue
                        ? machine.Id.Value
                        : 0,
                Model = machine.ModelNumber
            });
            ShowMachines(machineDtos);
        }

        public void ShowMachines(IEnumerable<MachineDTO> machineDtos)
        {
            MachineResultsGridView.DataSource = machineDtos;
            MachineResultsGridView.EmptyDataText = Constants.EmptyData.MachineResults;
            MachineResultsGridView.DataBind();
            MachineResultsUpdatePanel.Update();
            MachineResultsGridView.EmptyDataText = string.Empty;
        }
    }
}
