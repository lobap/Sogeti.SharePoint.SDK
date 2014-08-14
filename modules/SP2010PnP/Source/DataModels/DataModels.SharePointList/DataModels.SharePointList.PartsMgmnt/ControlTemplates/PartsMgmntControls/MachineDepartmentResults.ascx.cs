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
using Constants = DataModels.SharePointList.PartsMgmnt.Constants;


namespace DataModels.SharePointList.PartsMgmnt.PartsMgmntControls
{
    public partial class MachineDepartmentResults : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            MachineResultsGridView.RowDeleting += MachineResultsGridView_RowDeleting;
        }

        protected void MachineResultsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var selectedMachineDepartmentId = int.Parse(MachineResultsGridView.DataKeys[e.RowIndex].Value.ToString());
            using (var partManagementRepository = new PartManagementRepository())
            {
                partManagementRepository.DeleteMachineDepartment(selectedMachineDepartmentId);
            }
            Update();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IEnumerable<MachineDepartment> Data
        {
            set
            {
                var machineDepartmentResultsViewModels = new List<MachineDepartmentResultsViewModel>();

                foreach (MachineDepartment machineDepartment in value)
                {
                    var machineDepartmentViewModel = new MachineDepartmentResultsViewModel
                    {
                        Id = machineDepartment.Id.HasValue ? machineDepartment.Id.Value : 0,
                        MachineName = machineDepartment.Machine.Title,
                        LocationDescription = machineDepartment.Description,
                        Model = machineDepartment.Machine.ModelNumber
                    };

                    machineDepartmentResultsViewModels.Add(machineDepartmentViewModel);
                }

                FormatGrid();

                MachineResultsGridView.DataSource = machineDepartmentResultsViewModels;
                MachineResultsGridView.DataBind();


            }
        }

        protected void FormatGrid()
        {
            foreach (DataControlField column in MachineResultsGridView.Columns)
            {
                if (column.HeaderText == Constants.PartColumnName)
                {
                    column.Visible = false;
                }
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

    }
}
