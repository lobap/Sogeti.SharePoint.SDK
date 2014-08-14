//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


namespace Client.REST.Silverlight.Alt.NoMVVM
{
    using System;
    using System.Collections.ObjectModel;
    using System.Data.Services.Client;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using Client.REST.Silverlight.Alt.NoMVVM.ListDataService;

    public partial class MainPage : UserControl
    {
        private readonly string partsSiteURL = "/sites/sharepointlist/";
        private readonly string listService = "_vti_bin/listdata.svc";
        private PartsItem currentPart;
        private readonly ObservableCollection<PartsItem> parts = new ObservableCollection<PartsItem>();
        private readonly ObservableCollection<SuppliersItem> currentPartSuppliers = new ObservableCollection<SuppliersItem>();
        
        public MainPage()
        {
            InitializeComponent();
            Uri appSource = App.Current.Host.Source;
            string fullPartsSiteUrl = string.Format("{0}://{1}:{2}{3}{4}", appSource.Scheme, appSource.Host, appSource.Port, partsSiteURL, listService);
            this.DataContext = new PartsDataContext(new Uri(fullPartsSiteUrl));

            PartsDataGrid.ItemsSource = parts;
            SuppliersGrid.ItemsSource = currentPartSuppliers;
        }


        private void PartSearchButton_Click(object sender, RoutedEventArgs e)
        {
            GetParts(PartSkuTextBox.Text);
            partLocations1.CurrentLocation = null;
            partLocations1.ResetPart();
        }

        public void GetParts(string Sku)
        {
            parts.Clear();
            var context = (PartsDataContext)this.DataContext;
            //Define Query
            var query = (DataServiceQuery<PartsItem>)context.Parts
                                                                .Where(p => p.SKU.StartsWith(Sku))
                                                                .Select(p => new PartsItem
                                                                {
                                                                    Title = p.Title,
                                                                    SKU = p.SKU,
                                                                    Id = p.Id,
                                                                    Description = p.Description
                                                                });

            //Execute Query
            query.BeginExecute(DisplayParts, query);
        }

        private void DisplayParts(IAsyncResult result)
        {
            Dispatcher.BeginInvoke(() =>
            {
                DataServiceQuery<PartsItem> query = (DataServiceQuery<PartsItem>)result.AsyncState;
                var partResults = query.EndExecute(result);

                foreach (var part in partResults)
                {
                    parts.Add(part);
                }
            });

        }

        public void GetPartSuppliers()
        {
            currentPartSuppliers.Clear();
            var context = (PartsDataContext)this.DataContext;

            if (currentPart != null)
            {
                DataServiceQuery<SuppliersItem> query =
                    (DataServiceQuery<SuppliersItem>)context.PartSuppliers.Where(s => s.PartId == currentPart.Id).Select(s => new SuppliersItem { Title = s.Supplier.Title, DUNS = s.Supplier.DUNS, Rating = s.Supplier.Rating });

                query.BeginExecute(DisplaySuppliers, query);
            }
        }

        private void DisplaySuppliers(IAsyncResult result)
        {
            Dispatcher.BeginInvoke(() =>
            {
                DataServiceQuery<SuppliersItem> query =
                    (DataServiceQuery<SuppliersItem>)result.AsyncState;
                var suppliers = query.EndExecute(result);

                foreach (var supplier in suppliers)
                {
                    currentPartSuppliers.Add(supplier);
                }
            });


        }



        private void PartsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentPart = PartsDataGrid.SelectedItem as PartsItem;
            GetPartSuppliers();
            partLocations1.GetLocations(currentPart.Id);
        }
    }
}
