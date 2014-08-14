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
using System.ComponentModel;
using System.Data.Services.Client;
using System.Linq;
using System.Windows;
using Client.REST.Silverlight.ListDataService;


namespace Client.REST.Silverlight.ViewModels
{
    public class PartInventoryViewModel : INotifyPropertyChanged
    {
        private PartsDataContext context = null;
        private List<PartsItem> InventoryPartResults = new List<PartsItem>();
        private readonly string partsSiteURL = "/sites/sharepointlist/";
        private readonly string listService = "_vti_bin/listdata.svc";
        private string searchSku;

        public PartInventoryViewModel()
        {
            Uri appSource = App.Current.Host.Source;
            string fullPartsSiteUrl = string.Format("{0}://{1}:{2}{3}{4}", appSource.Scheme, appSource.Host, appSource.Port, partsSiteURL, listService);
            context = new PartsDataContext(new Uri(fullPartsSiteUrl));
        }

        
        private ObservableCollection<PartInventory> parts = new ObservableCollection<PartInventory>();
        public ObservableCollection<PartInventory> Parts
        {
            get { return parts; }
        }
        private PartInventory currentItem = new PartInventory();
        public PartInventory CurrentItem
        {
            get { return currentItem; }
            set 
            {
                if (value == currentItem) return;
                currentItem = value; 
                GetPartSuppliers(); 
                OnPropertyChanged("CurrentItem"); 
            }
        }

        private ObservableCollection<SuppliersItem> currentPartSuppliers = new ObservableCollection<SuppliersItem>();
        public ObservableCollection<SuppliersItem> CurrentPartSuppliers
        {
            get
            {
                return currentPartSuppliers;
            } 
        }

        private void OnSaveChanges(IAsyncResult result)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                       {
                                           context.EndSaveChanges(result);
                                           MessageBox.Show("Inventory Changes Saved Successfully");
                                       });
        }

        public string SearchSku
        {
            get { return searchSku; }
            set
            {
                if (value == searchSku) return;
                searchSku = value;
                OnPropertyChanged("SearchSku");
            }
        }

        public void GetParts()
        {
            CurrentPartSuppliers.Clear();
            Parts.Clear();

            //Define Query against Inventory Locations to return Parts that match with Inventory Locations
            //Using AddQueryOption here to enforce the projection across entities. We chose to maintain the REST entity types and not create a  custom type (to support anonymous)this also helps to facilitate the databinding to between the View and ViewModel and simplifies updating.
            var inventoryLocationsQuery = (DataServiceQuery<InventoryLocationsItem>)context.InventoryLocations.Expand("Part").AddQueryOption("$select", "BinNumber,Quantity,Title,Id,PartId, Part/SKU,Part/Title,Part/Id").Where(p => p.Part.SKU.StartsWith(SearchSku)).OrderBy(p => p.Part.SKU);

            //Define Query against Parts to return Parts that match regardless of Inventory Locations
            var partsQuery = (DataServiceQuery<PartsItem>)context.Parts.Where(p => p.SKU.StartsWith(SearchSku)).Select(p => new PartsItem { Title = p.Title, SKU = p.SKU, Id = p.Id, Description = p.Description });


            //Execute Query as a Batch
            context.BeginExecuteBatch(DisplayParts, context, inventoryLocationsQuery, partsQuery);
        }

        private void DisplayParts(IAsyncResult result)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                       {

                                           List<PartsItem> AllPartResults = new List<PartsItem>();
                                           List<PartsItem> NoInventoryPartResults = new List<PartsItem>();

                                           //Get the Batch Response
                                           DataServiceResponse Response = context.EndExecuteBatch(result);

                                           //Loop through each operation
                                           foreach (QueryOperationResponse Operation in Response)
                                           {
                                               if (Operation.Error != null)
                                               {
                                                   throw Operation.Error;
                                               }
                                               if (Operation is QueryOperationResponse<InventoryLocationsItem>)
                                               {
                                                   //Process Results
                                                   foreach (InventoryLocationsItem location in Operation as QueryOperationResponse<InventoryLocationsItem>)
                                                   {
                                                       PartInventory partInventory = new PartInventory();
                                                       partInventory.Part = location.Part;
                                                       partInventory.InventoryItem = location;
                                                       Parts.Add(partInventory);
                                                       InventoryPartResults.Add(location.Part);
                                                   }

                                               }

                                               if (Operation is QueryOperationResponse<PartsItem>)
                                               {
                                                   //Process Results
                                                   foreach (PartsItem part in Operation as QueryOperationResponse<PartsItem>)
                                                   {
                                                       AllPartResults.Add(part);
                                                   }
                                               }
                                           }

                                           foreach (
    var allPartResult in
        AllPartResults.Where(
            allPartResult =>
            !InventoryPartResults.Contains(allPartResult)))
                                           {
                                               NoInventoryPartResults.Add(allPartResult);
                                           }

                                           foreach (var part in NoInventoryPartResults)
                                           {
                                               PartInventory partInventory = new PartInventory();
                                               partInventory.Part = part;
                                               partInventory.InventoryItem = null;

                                               Parts.Add(partInventory);
                                           }
                                       });
        }

        private void GetPartSuppliers()
        {
            if (currentItem != null)
            {
                DataServiceQuery<SuppliersItem> query =
                    (DataServiceQuery<SuppliersItem>)context.PartSuppliers.Where(s => s.PartId == currentItem.Part.Id).Select(s => new SuppliersItem { Title = s.Supplier.Title, DUNS = s.Supplier.DUNS, Rating = s.Supplier.Rating });
                
                query.BeginExecute(DisplaySuppliers, query);
            }
        }

        private void DisplaySuppliers(IAsyncResult result)
        {

            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                       {
                                           CurrentPartSuppliers.Clear();

                                           DataServiceQuery<SuppliersItem> query =
                                               (DataServiceQuery<SuppliersItem>)result.AsyncState;
                                           var partSuppliers = query.EndExecute(result);

                                           foreach (var partSupplier in partSuppliers)
                                           {
                                               CurrentPartSuppliers.Add(partSupplier);
                                           }
                                       });
        }

        public void UpdateInventoryLocation()
        {
            context.UpdateObject(CurrentItem.InventoryItem);
        }

        public void Update()
        {
            context.BeginSaveChanges(SaveChangesOptions.Batch, OnSaveChanges, null);
        }
        
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }


}
