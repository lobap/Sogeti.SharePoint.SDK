//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System.Windows;
using System.Windows.Controls;

namespace Client.SPWebService.Silverlight
{
    using ViewModels;

    public partial class MainPage : UserControl
    {
        private PartInventoryViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();
            viewModel = new PartInventoryViewModel();
            this.DataContext = viewModel;        
        }

        private void findPartsButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.GetParts();
        }
    }
}
