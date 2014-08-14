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
using Microsoft.SharePoint;

namespace DataModels.SharePointList.PartsMgmnt.ControlTemplates.PartsMgmntControls
{
    using System.Linq;
    using DTOs;
    using Microsoft.Practices.SharePoint.Common.ServiceLocation;
    using Model;
    using Constants = PartsMgmnt.Constants;

    public partial class ManagePartMachines : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString[Constants.Sku] != null)
            {
                string sku = Request.QueryString[Constants.Sku].ToString();
                LoadPartMachines(sku);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PartMachineResultsGridView.RowDeleting += PartMachineResultsGridView_RowDeleting;
            MachineResultsGridView.SelectedIndexChanged += MachineResultsGridView_SelectedIndexChanged;
            GoButton.Click += GoButton_Click;
            AddNewHyperlink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Machines,
                                                        "/NewForm.aspx?IsDlg=1');");
        }

        private void GoButton_Click(object sender, EventArgs eventArgs)
        {
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                IEnumerable<Machine> machines = partManagementRepository.GetMachinesByPartialModelNumber(MachineSearchTextBox.Text);
                ShowMachineResults(machines);
            }
        }

        protected void MachineResultsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedMachineId = int.Parse(MachineResultsGridView.DataKeys[MachineResultsGridView.SelectedIndex].Value.ToString());
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                if (Request.QueryString[Constants.Sku] != null)
                {
                    string sku = Request.QueryString[Constants.Sku].ToString();
                    partManagementRepository.AddNewMachinePart(selectedMachineId, sku);
                    LoadPartMachines(sku);
                }
            }
        }
        protected void PartMachineResultsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int partMachineId = int.Parse(MachineResultsGridView.DataKeys[e.RowIndex].Value.ToString());
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                partManagementRepository.DeleteMachinePart(partMachineId);
                if (Request.QueryString[Constants.Sku] != null)
                {
                    string sku = Request.QueryString[Constants.Sku].ToString();
                    LoadPartMachines(sku);
                }
            }
        }

        protected void LoadPartMachines(string sku)
        {
            using (var partManagementRepository = SharePointServiceLocator.GetCurrent().GetInstance<IPartManagementRepository>())
            {
                IEnumerable<MachinePart> machines = partManagementRepository.GetMachinePartsByPart(sku);
                var machineDtos = machines.Select(machinePart => new MachineDTO
                {
                    MachineName =
                        machinePart.Machine.Title,
                    Manufacturer =
                        machinePart.Machine.Manufacturer.
                        Title,
                    Id =
                        machinePart.Id.HasValue
                            ? machinePart.Id.Value
                            : 0,
                    Model =
                        machinePart.Machine.ModelNumber
                }).ToList();
                ShowMachineParts(machineDtos);
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
            MachineResultsGridView.DataSource = machineDtos;
            MachineResultsGridView.EmptyDataText = Constants.EmptyData.MachineResults;
            MachineResultsGridView.DataBind();
            MachineResultsUpdatePanel.Update();
        }

        public void ShowMachineParts(IEnumerable<MachineDTO> machineDtos)
        {
            PartMachineResultsGridView.EmptyDataText = Constants.EmptyData.MachinePartResults;
            PartMachineResultsGridView.DataSource = machineDtos;
            PartMachineResultsGridView.DataBind();
            PartMachineResultsUpdatePanel.Update();
            PartMachineResultsGridView.EmptyDataText = string.Empty;
        }
    }
}
