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
using DataModels.SharePointList.Model;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;

namespace DataModels.SharePointList.PartsMgmnt.PartsMgmntControls
{
    using DTOs;

    public enum AreaDisplayTypes
    {
        Category,
        Manufacturer,
        Department
    }


    public partial class SearchPage : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            GoButton.Click += GoButton_Click;
            PartGoButton.Click += PartGoButton_Click;
            AreaResultsGridView.RowCommand += areaResultsGridView_RowCommand;
            AreaRadioButton.SelectedIndexChanged += AreaRadioButton_SelectedIndexChanged;
            MachineResultsGridView.RowCommand += machineResultsGridView_RowCommand;
            MachineResultsGridView.RowDataBound += MachineResultsGridView_RowDataBound;
            PartResultsGridView.RowDataBound += PartResultsGridView_RowDataBound;

            AddNewHyperlink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Machines,
                                                        "/NewForm.aspx?IsDlg=1');");

            PartAddNewHyperlink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Parts, "/NewForm.aspx?IsDlg=1');");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void PartGoButton_Click(object sender, EventArgs eventArgs)
        {
            //Clear the Machine Results Grid
            MachineResultsGridView.DataSource = null;
            MachineResultsGridView.DataBind();
            MachineResultsUpdatePanel.Update();
            
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                IEnumerable<PartInventoryDTO> partsData = partManagementRepository.GetPartsInventoryView(PartSearchTextBox.Text);
                ShowParts(partsData);
            }
        }

        public void ShowParts(IEnumerable<PartInventoryDTO> partInventoryDtos)
        {
            PartResultsGridView.EmptyDataText = Constants.EmptyData.PartResults;
            PartResultsGridView.DataSource = partInventoryDtos;
            PartResultsGridView.DataBind();
            PartResultUpdatePanel.Update();
            PartResultsGridView.EmptyDataText = string.Empty;
        }

        private void GoButton_Click(object sender, EventArgs eventArgs)
        {
            //Clear the Part Results Grid
            PartResultsGridView.DataSource = null;
            PartResultsGridView.DataBind();
            PartResultUpdatePanel.Update();

            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                IEnumerable<Machine> machines = partManagementRepository.GetMachinesByPartialModelNumber(MachineSearchTextBox.Text);
                ShowMachineResults(machines);
            }
        }

        protected void areaResultsGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            AreaDisplayTypes displayType =
                (AreaDisplayTypes)System.Enum.Parse(typeof(AreaDisplayTypes), AreaRadioButton.SelectedValue);
            int selectedAreaId =
                int.Parse(AreaResultsGridView.DataKeys[int.Parse(e.CommandArgument.ToString())].Value.ToString());

            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                IEnumerable<MachineDTO> machines = null;

                switch (displayType)
                {
                    case AreaDisplayTypes.Category:
                        machines = partManagementRepository.GetMachinesByCategory(selectedAreaId);
                        break;
                    case AreaDisplayTypes.Manufacturer:
                        machines = partManagementRepository.GetMachinesByManufacturer(selectedAreaId);
                        break;
                    case AreaDisplayTypes.Department:
                        machines = partManagementRepository.GetMachinesByDepartment(selectedAreaId);
                        break;
                }
                ShowMachines(machines);
            }
        }

        protected void AreaRadioButton_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Clear the Part Results Grid
            PartResultsGridView.DataSource = null;
            PartResultsGridView.DataBind();
            PartResultUpdatePanel.Update();

            //Clear the Machine Results Grid
            MachineResultsGridView.DataSource = null;
            MachineResultsGridView.DataBind();
            MachineResultsUpdatePanel.Update();

            AreaDisplayTypes displayType = (AreaDisplayTypes)System.Enum.Parse(typeof(AreaDisplayTypes), AreaRadioButton.SelectedValue);
            //  AreaResultsGridView.Columns[1].HeaderText = "All " + MakePlural(displayType);
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {

                if (displayType == AreaDisplayTypes.Category)
                {
                    var categories = partManagementRepository.GetCategories();
                    var areaDtos = categories.Select(category => new AreaDTO
                                                                                  {
                                                                                      Title = category.Title,
                                                                                      Id =
                                                                                          category.Id.HasValue
                                                                                              ? category.Id.Value
                                                                                              : 0
                                                                                  });
                    ShowAreaResults(areaDtos);

                }
                if (displayType == AreaDisplayTypes.Department)
                {
                    var departments = partManagementRepository.GetDepartments();
                    var areaDtos = departments.Select(department => new AreaDTO
                                                                                     {
                                                                                         Title = department.Title,
                                                                                         Id =
                                                                                             department.Id.HasValue
                                                                                                 ? department.Id.Value
                                                                                                 : 0
                                                                                     });
                    ShowAreaResults(areaDtos);
                }
                if (displayType == AreaDisplayTypes.Manufacturer)
                {
                    var manufacturers = partManagementRepository.GetManufacturers();
                    var areaDtos = manufacturers.Select(manufacturer => new AreaDTO
                                                                                         {
                                                                                             Title = manufacturer.Title,
                                                                                             Id =
                                                                                                 manufacturer.Id.
                                                                                                     HasValue
                                                                                                     ? manufacturer.Id.
                                                                                                           Value
                                                                                                     : 0
                                                                                         });
                    ShowAreaResults(areaDtos);
                }
            }
        }

        private string MakePlural(AreaDisplayTypes areaDisplayType)
        {
            switch (areaDisplayType)
            {
                case AreaDisplayTypes.Category:
                    return "Categories";
                case AreaDisplayTypes.Manufacturer:
                    return "Manufacturers";
                case AreaDisplayTypes.Department:
                    return "Departments";
                default:
                    throw new ArgumentOutOfRangeException("areaDisplayType");
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

        private void ShowMachineResults(IEnumerable<Machine> machines)
        {
            var machineDtos = machines.Select(machine => new MachineDTO
            {
                MachineName = machine.Title,
                Manufacturer =
                    machine.Manufacturer.Title,
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
            MachineResultsGridView.EmptyDataText = Constants.EmptyData.MachineResults;
            MachineResultsGridView.DataSource = machineDtos;
            MachineResultsGridView.DataBind();
            MachineResultsUpdatePanel.Update();
            MachineResultsGridView.EmptyDataText = string.Empty;
        }

        public void ShowAreaResults(IEnumerable<AreaDTO> areaDtos)
        {
            AreaResultsGridView.DataSource = areaDtos;
            AreaResultsGridView.DataBind();
        }
    }
}
