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

    public partial class PartLocations : UserControl
    {
        private readonly ObservableCollection<InventoryLocationsItem> inventoryLocations = new ObservableCollection<InventoryLocationsItem>();
        private int currentPartId = -1;

        public PartLocations()
        {
            InitializeComponent();
            locationsDataGrid.ItemsSource = inventoryLocations;
            locationsDataGrid.SelectionChanged += locationsDataGrid_SelectionChanged;

        }

        void locationsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentLocation = locationsDataGrid.SelectedItem as InventoryLocationsItem;
        }

        private InventoryLocationsItem currentLocation;
        public InventoryLocationsItem CurrentLocation
        {
            get { return currentLocation; }
            set
            {
                currentLocation = value;
                if (currentLocation != null)
                {
                    quantityTextBox.Text = currentLocation.Quantity.ToString();
                    binTextBox.Text = currentLocation.BinNumber;
                }
            }
        }

        private bool CheckForError()
        {
            string error = null;
            int quantityVal = -1;

            if (string.IsNullOrEmpty(quantityTextBox.Text) || string.IsNullOrEmpty(binTextBox.Text))
            {
                error = "Bin number and quantity must be specified";
            }
            else if (int.TryParse(quantityTextBox.Text, out quantityVal) == false)
            {
                error = "Quantity must be a valid number";
            }
            else if (this.currentPartId == -1)
            {
                error = "No Part Selected";
            }

            if (error != null)
            {
                MessageBox.Show(error);
                return true;
            }
            return false;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            var context = (PartsDataContext)this.DataContext;
            if (CheckForError() == false)
            {
                //if null then its a new item, otherwise update existing
                if (CurrentLocation != null)
                {
                    CurrentLocation.Quantity = int.Parse(quantityTextBox.Text);
                    CurrentLocation.BinNumber = binTextBox.Text;
                    context.UpdateObject(CurrentLocation);
                }
                else
                {
                    InventoryLocationsItem locationsItem = new InventoryLocationsItem();
                    locationsItem.Quantity = int.Parse(quantityTextBox.Text);
                    locationsItem.BinNumber = binTextBox.Text;
                    locationsItem.PartId = this.currentPartId;
                    context.AddToInventoryLocations(locationsItem);
                    inventoryLocations.Add(locationsItem);
                    this.CurrentLocation = locationsItem;
                }
                context.BeginSaveChanges(SaveChangesOptions.Batch, OnSaveChanges, null);
            }
        }


        private void addNew_Click(object sender, RoutedEventArgs e)
        {
            locationsDataGrid.SelectedItem = null;
            binTextBox.Text = string.Empty;
            quantityTextBox.Text = string.Empty;
        }

        private void OnSaveChanges(IAsyncResult result)
        {
            var context = (PartsDataContext)this.DataContext;
            Dispatcher.BeginInvoke(() =>
            {
                context.EndSaveChanges(result);
                MessageBox.Show("Inventory Changes Saved Successfully");
            });
        }

        public void ResetPart()
        {
            currentPartId = -1;
        }

        public void GetLocations(int partId)
        {
            var context = (PartsDataContext)this.DataContext;
            inventoryLocations.Clear();

            this.currentPartId = partId;
            var query =
                (DataServiceQuery<InventoryLocationsItem>)
                context.InventoryLocations.Where(
                    p => p.PartId == partId).Select(
                        p =>
                        new InventoryLocationsItem 
                            {   BinNumber = p.BinNumber, 
                                Id = p.Id, 
                                Quantity = p.Quantity, 
                                Title = p.Title, 
                                PartId = p.PartId 
                            });

            //Execute Query
            query.BeginExecute(DisplayLocations, query);
        }

        private void DisplayLocations(IAsyncResult asyncResult)
        {
            Dispatcher.BeginInvoke(
                () =>
                {
                    DataServiceQuery<InventoryLocationsItem> query =
                        (DataServiceQuery<InventoryLocationsItem>)asyncResult.AsyncState;

                    //this delegate will execute when the results of the async query have returned
                    var partLocations = query.EndExecute(asyncResult);

                    foreach (var location in partLocations)
                    {
                        inventoryLocations.Add(location);
                    }
                });
        }
    }
}
