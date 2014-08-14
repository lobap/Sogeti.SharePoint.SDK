//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


namespace DataModels.ExternalData.PartsManagement.ManageMachines
{
    [ToolboxItemAttribute(false)]
    public class ManageMachines : WebPart
    {
        private Label ResponseLabel = new Label();
        TextBox searchMachineModelTextBox = new TextBox();
        GridView machineGridView = new GridView();
        GridView partGridView = new GridView();
        Label validationLabel = new Label { Text = "", ID="ValidationLabel", ForeColor=System.Drawing.Color.Red  };
        protected override void CreateChildControls()
        {
            //Configure Search Button
            var searchMachineModelButton = new Button { Text = "Go", CausesValidation=true };
            searchMachineModelButton.Click += new EventHandler(SearchMachineModelButton_Click);

            //Configure Button Field
            ButtonField selectButton = new ButtonField();
            selectButton.Text = "select";
            selectButton.CommandName = "Select";
            selectButton.CausesValidation = false;

            //Configure Machine GridView
            machineGridView.Columns.Add(selectButton);
            machineGridView.DataKeyNames = new string[] { "ID" };
            machineGridView.SelectedIndexChanged += machineGridView_SelectedIndexChanged;
            machineGridView.RowStyle.Wrap = false;

            //Configure Parts GridView
            partGridView.RowStyle.Wrap = false;

            //Add Controls
            Controls.Add(new Label { Text = "Search by Machine Model: " });
            searchMachineModelTextBox.ID = "SearchMachineTextBox";
            Controls.Add(searchMachineModelTextBox);
            Controls.Add(validationLabel);
            Controls.Add(searchMachineModelButton);
            Controls.Add(ResponseLabel);
            Controls.Add(new Literal() { Text = "<br/><br/>" });
            Controls.Add(machineGridView);
            Controls.Add(new Literal() { Text = "<br/><br/>" });
            Controls.Add(partGridView);
        }



        protected void machineGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedMachineID = int.Parse(machineGridView.SelectedRow.Cells[1].Text.ToString());

            var partManagementRepository = new PartManagementRepository();
            partGridView.DataSource = partManagementRepository.GetPartsByMachineId(selectedMachineID);
            partGridView.DataBind();
        }

        void SearchMachineModelButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(searchMachineModelTextBox.Text) && searchMachineModelTextBox.Text.Length >= 3)
            {
                validationLabel.Text = string.Empty;
                validationLabel.Visible = false;

                var partManagementRepository = new PartManagementRepository();
                machineGridView.DataSource = partManagementRepository.GetMachinesByModelNumber(searchMachineModelTextBox.Text);
                machineGridView.DataBind();
            }
            else
            {
                validationLabel.Text = "Machine Model must contain at least three (3) alpha-numeric characters !";
                validationLabel.Visible = true;
            }

        }

    }

}


