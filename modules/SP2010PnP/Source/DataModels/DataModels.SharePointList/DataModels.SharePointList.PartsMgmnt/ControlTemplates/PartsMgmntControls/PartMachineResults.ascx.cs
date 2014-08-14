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
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataModels.SharePointList.PartsMgmnt.ViewModels;
using DataModels.SharePointList.Model;
using Microsoft.SharePoint;
 

namespace DataModels.SharePointList.PartsMgmnt.PartsMgmntControls
{
    public partial class PartMachineResults : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            MachineResultsGridView.RowDeleting += MachineResultsGridView_RowDeleting;
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
     
        }

        public IEnumerable<MachinePart> Data
        {
            set
            {
                var machineResultsViewModels = new List<MachineResultsViewModel>();

                foreach (MachinePart machinepart in value)
                {
                    var machineModel = new MachineResultsViewModel
                    {
                        MachineName = machinepart.Machine.Title,
                        Manufacturer = machinepart.Machine.Manufacturer.Title,
                        Id = machinepart.Id.HasValue ? machinepart.Id.Value : 0,
                        Model = machinepart.Machine.ModelNumber
                    };

                    machineResultsViewModels.Add(machineModel);
                }

                MachineResultsGridView.DataSource = machineResultsViewModels;
                MachineResultsGridView.DataBind();
            }
        }

        public void Update()
        {
            MachineResultsUpdatePanel.Update();
        }

        public void Clear()
        {
            MachineResultsGridView.DataSource = null;
            MachineResultsGridView.DataBind();
            Update();
        }

        protected void MachineResultsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int partMachineId = int.Parse(MachineResultsGridView.DataKeys[e.RowIndex].Value.ToString());
            using (var partManagementRepository = new PartManagementRepository())
            {
                partManagementRepository.DeleteMachinePart(partMachineId);
                Update();
            }
        }
 
    }
}
