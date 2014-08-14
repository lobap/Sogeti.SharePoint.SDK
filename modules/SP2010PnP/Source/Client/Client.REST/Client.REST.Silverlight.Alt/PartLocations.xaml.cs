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
using System.Collections.ObjectModel;
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
using Client.REST.Silverlight.Alt.ListDataService;
using Client.REST.Silverlight.Alt.ViewModels;

namespace Client.REST.Silverlight.Alt
{
    public partial class PartLocations : UserControl
    {
        public PartLocations()
        {
            InitializeComponent();

        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            PartInventoryViewModel viewModel = (PartInventoryViewModel)this.DataContext;

            string error = viewModel.InventoryLocationSaved();

            if(error != null)
                MessageBox.Show(error);
        }

        
        private void addNew_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (PartInventoryViewModel) this.DataContext;

            viewModel.SetNewLocation();
        }

    }
}
