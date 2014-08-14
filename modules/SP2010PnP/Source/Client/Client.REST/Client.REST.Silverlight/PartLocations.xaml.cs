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
using System.Data.Services.Client;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Client.REST.Silverlight.ListDataService;
using Client.REST.Silverlight.ViewModels;

namespace Client.REST.Silverlight
{
    public delegate void LocationChanged(object sender, EventArgs e);
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
            PartInventoryViewModel model = this.DataContext as PartInventoryViewModel;
            if (model != null)
            {
                //edit existing location
                if (model.CurrentItem.InventoryItem != null && !string.IsNullOrEmpty(quantityTextBox.Text))
                {
                    model.CurrentItem.InventoryItem.Quantity = int.Parse(quantityTextBox.Text);
                    model.CurrentItem.InventoryItem.BinNumber = binTextBox.Text;
                    model.UpdateInventoryLocation();
                }
               
            }
            this.Visibility = Visibility.Collapsed;   
        }

  }
}
