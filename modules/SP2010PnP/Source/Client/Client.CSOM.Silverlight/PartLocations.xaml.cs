//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Windows;
using System.Windows.Controls;
using Client.CSOM.Silverlight.Entities;
using Client.CSOM.Silverlight.ViewModels;
using Microsoft.SharePoint.Client;


namespace Client.CSOM.Silverlight
{
    public partial class PartLocations : UserControl
    {
        public PartLocations()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            // we only allow update, not add, in the RI.
            PartInventoryViewModel model = DataContext as PartInventoryViewModel;
            if (model != null && model.CurrentItem != null && model.CurrentItem.InventoryItem != null)
            {
                model.CurrentItem.InventoryItem.BinNumber = binTextBox.Text;
                if (!string.IsNullOrEmpty(quantityTextBox.Text))
                {
                    model.CurrentItem.InventoryItem.Quantity = int.Parse(quantityTextBox.Text);
                }
                else model.CurrentItem.InventoryItem.Quantity = 0;
                model.UpdateInventoryLocation();

                this.Visibility = Visibility.Collapsed;
            }
        }
    }
}
