//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace DataModels.SharePointList.PartsMgmnt.ControlTemplates.PartsMgmntControls
{
    public partial class AddPartLocation : UserControl
    {
        public event EventHandler<GenericEventArgs<string[]>> OnInventoryLocationAdded;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SaveButton.Click += SaveButton_Click;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (OnInventoryLocationAdded != null)
            {
                InventoryLocationAdded(new GenericEventArgs<string[]>
                {
                    PayLoad = new string[]{BinTextBox.Text, QuantityTextBox.Text}
                });
                
                ClearControls();
            }
        }

 
        public void ClearControls()
        {
            BinTextBox.Text = string.Empty;
            QuantityTextBox.Text = string.Empty;

            UpdatePanel1.Update();
        }

        protected virtual void InventoryLocationAdded(GenericEventArgs<string[]> e)
        {
            OnInventoryLocationAdded(this, e);
        }
    }
}
