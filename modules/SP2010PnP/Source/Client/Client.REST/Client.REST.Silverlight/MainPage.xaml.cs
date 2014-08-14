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
using System.Windows;
using System.Windows.Controls;
using Client.REST.Silverlight.ListDataService;
using Client.REST.Silverlight.ViewModels;

namespace Client.REST.Silverlight
{
    public partial class MainPage : UserControl
    {
        private PartInventoryViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();
            partLocations1.Visibility = Visibility.Collapsed;
            viewModel = new PartInventoryViewModel();
            this.DataContext = viewModel;
        }

        private void PartSearchButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.GetParts();
        }
        

        private void locationsButton_Click(object sender, RoutedEventArgs e)
        {
            partLocations1.Visibility = Visibility.Visible;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Update();
        }

    }
}
