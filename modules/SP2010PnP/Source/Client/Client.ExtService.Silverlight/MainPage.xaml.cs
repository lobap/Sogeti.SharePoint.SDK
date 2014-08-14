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

namespace Client.ExtService.Silverlight
{
    using ViewModels;

    public partial class MainPage : UserControl, IDisposable
    {
        private SupplierViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();
            viewModel = new SupplierViewModel();

            this.DataContext = viewModel;
        }

        private void getSuppliersButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RetrieveSuppliers();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                viewModel.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }

    }
}
