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

namespace DataModels.ExternalData.PartsManagement.ManageSuppliers
{
    [ToolboxItemAttribute(false)]
    public class ManageSuppliers : WebPart
    {
        private Label ResponseLabel = new Label();
        TextBox searchSupplierNameTextBox = new TextBox();
        GridView SupplierGridView = new GridView();
        GridView partGridView = new GridView();
        Label validationLabel = new Label { Text = "", ID = "ValidationLabel", ForeColor = System.Drawing.Color.Red };

        protected override void CreateChildControls()
        {
            //Configure Search Button
            var searchSupplierNameButton = new Button { Text = "Go", CausesValidation=true };
            searchSupplierNameButton.Click += new EventHandler(SearchSupplierNameButton_Click);

            //Configure Button Field
            ButtonField selectButton = new ButtonField();
            selectButton.Text = "View Details";
            selectButton.CommandName = "Select";

            //Configure Supplier GridView
            SupplierGridView.Columns.Add(selectButton);
            SupplierGridView.DataKeyNames = new string[] { "SupplierID" };
            SupplierGridView.SelectedIndexChanged += SupplierGridView_SelectedIndexChanged;
            SupplierGridView.RowStyle.Wrap = false;

            //Configure Parts GridView
            partGridView.RowStyle.Wrap = false;

            //Add Controls
            Controls.Add(new Label { Text = "Search by Supplier Name: "});
            searchSupplierNameTextBox.ID="SupplierNameTextBox";
            Controls.Add(searchSupplierNameTextBox);
            Controls.Add(validationLabel);
            Controls.Add(searchSupplierNameButton);
            Controls.Add(ResponseLabel);
            Controls.Add(new Literal() { Text = "<br/><br/>" });
            Controls.Add(SupplierGridView);
            Controls.Add(new Literal() { Text = "<br/><br/>" });
            Controls.Add(partGridView);
        }


        protected void SupplierGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedSupplierID = int.Parse(SupplierGridView.SelectedRow.Cells[1].Text.ToString());
            this.Page.Response.Redirect("SupplierDetails.aspx?Identifier1=" + selectedSupplierID);
        }

        void SearchSupplierNameButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(searchSupplierNameTextBox.Text) && searchSupplierNameTextBox.Text.Length >= 3)
            {
                validationLabel.Text = string.Empty;
                validationLabel.Visible = false;

                var partManagementRepository = new PartManagementRepository();
                SupplierGridView.DataSource = partManagementRepository.GetSuppliersByName(searchSupplierNameTextBox.Text);
                SupplierGridView.DataBind();
            }
            else
            {
                validationLabel.Text = "Supplier Name must contain at least three (3) alpha-numeric characters !";
                validationLabel.Visible = true;
            }

            
        }
    }
}
