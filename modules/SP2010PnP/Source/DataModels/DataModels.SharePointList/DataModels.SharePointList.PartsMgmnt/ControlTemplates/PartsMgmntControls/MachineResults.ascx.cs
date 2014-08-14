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
    public partial class MachineResults : UserControl
    {
        public event EventHandler<GenericEventArgs<IEnumerable<Part>>> OnRelatedPartsFound;
        public event EventHandler<GenericEventArgs<Machine>> OnMachineSelected;

        public bool AllowParts { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowSelect { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            MachineResultsGridView.RowCommand += machineResultsGridView_RowCommand;
            MachineResultsGridView.RowDataBound += MachineResultsGridView_RowDataBound;
            MachineResultsGridView.SelectedIndexChanged += MachineResultsGridView_SelectedIndexChanged;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
     
        }

        public MachineResults()
        {
            OnMachineSelected += delegate { };
        }
        public IEnumerable<Machine> Data
        {
            set
            {
                var machineResultsViewModels = new List<MachineResultsViewModel>();

                foreach (Machine machine in value)
                {
                    var machineModel = new MachineResultsViewModel
                    {
                        MachineName = machine.Title,
                        Manufacturer = machine.Manufacturer.Title,
                        Id = machine.Id.HasValue ? machine.Id.Value : 0,
                        Model = machine.ModelNumber
                    };

                    machineResultsViewModels.Add(machineModel);
                }

                FormatGrid();

                MachineResultsGridView.DataSource = machineResultsViewModels;
                MachineResultsGridView.DataBind();

                MachineResultsUpdatePanel.Update();


            }
        }

        protected void FormatGrid()
        {
            if (!AllowParts)
            {
                MachineResultsGridView.Columns[6].Visible = false;
            }
            if (!AllowEdit)
            {
                MachineResultsGridView.Columns[1].Visible = false;
            }
            if (!AllowSelect)
            {
                MachineResultsGridView.Columns[2].Visible = false;
            }
        }

        public void Update()
        {
           
        }

        public void Clear()
        {
            MachineResultsGridView.DataSource = null;
            MachineResultsGridView.DataBind();
            DataBind();
        }

        private void machineResultsGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var selectedMachineId =
                int.Parse(MachineResultsGridView.DataKeys[int.Parse(e.CommandArgument.ToString())].Value.ToString());
            using (var partManagementRepository = new PartManagementRepository())
            {

                switch (e.CommandName)
                {
                    case "ViewParts":
                        if (OnRelatedPartsFound != null)
                        {
                            RelatedPartsFound(new GenericEventArgs<IEnumerable<Part>>
                                                  {
                                                      PayLoad = partManagementRepository.GetPartsByMachineId(selectedMachineId)
                                                  });
                        }

                        break;
                }
            }
        }


        protected virtual void RelatedPartsFound(GenericEventArgs<IEnumerable<Part>> e)
        {
            OnRelatedPartsFound(this, e);
        }

        protected virtual void MachineSelected(GenericEventArgs<Machine> e)
        {
            OnMachineSelected(this, e);
        }

        protected void MachineResultsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedMachineId =
                int.Parse(MachineResultsGridView.DataKeys[MachineResultsGridView.SelectedIndex].Value.ToString());
            using (var partManagementRepository = new PartManagementRepository())
            {
                MachineSelected(new GenericEventArgs<Machine>
                                    {
                                        PayLoad = partManagementRepository.GetMachine(selectedMachineId)
                                    });
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
    }
}
