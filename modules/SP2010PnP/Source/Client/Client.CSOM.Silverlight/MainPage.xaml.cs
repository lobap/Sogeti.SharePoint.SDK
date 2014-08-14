//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using Client.CSOM.Silverlight.ViewModels;

namespace Client.CSOM.Silverlight
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public partial class MainPage : UserControl, IDisposable
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

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                viewModel.Dispose();
            }           
        }

        // free native resources
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
