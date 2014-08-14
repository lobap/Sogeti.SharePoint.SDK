//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Services.Client;
using System.Linq;
using System.Windows;
using Client.REST.Silverlight.Alt.ListDataService;


namespace Client.REST.Silverlight.Alt.ViewModels
{
    public class PartInventoryViewModel : INotifyPropertyChanged
    {
        private PartsDataContext context;
        private readonly string partsSiteURL = "/sites/sharepointlist/";
        private readonly string listService = "_vti_bin/listdata.svc";
        private string searchSku;

        public PartInventoryViewModel()
        {
            Uri appSource = App.Current.Host.Source;
            string fullPartsSiteUrl = string.Format("{0}://{1}:{2}{3}{4}", appSource.Scheme, appSource.Host, appSource.Port, partsSiteURL, listService);
            context = new PartsDataContext(new Uri(fullPartsSiteUrl));
        }

        private ObservableCollection<PartsItem> parts = new ObservableCollection<PartsItem>();
        public ObservableCollection<PartsItem> Parts
        {
            get { return parts; }
        }

        private PartsItem currentPart = null;
        public PartsItem CurrentPart
        {
            get { return currentPart; }
            set
            {
                if (value == currentPart) return;
                currentPart = value;
                GetPartSuppliers();
                GetLocations();
                OnPropertyChanged("CurrentPart");
            }
        }

        private InventoryLocationsItem currentLocation= new InventoryLocationsItem();
        public InventoryLocationsItem CurrentLocation
        {
            get { return currentLocation; }
            set
            {
                if (value == currentLocation) return;
                currentLocation = value;
                OnPropertyChanged("CurrentLocation");
            }
        }

        private InventoryLocationsItem newLocation;

        public void SetNewLocation()
        {
            if (newLocation == null)
            {
                newLocation = new InventoryLocationsItem();
            }
            else
            {
                newLocation.BinNumber = string.Empty;
                newLocation.Quantity = null;
            }

            CurrentLocation = newLocation;
        }

        private ObservableCollection<SuppliersItem> currentPartSuppliers = new ObservableCollection<SuppliersItem>();
        public ObservableCollection<SuppliersItem> CurrentPartSuppliers
        {
            get
            {
                return currentPartSuppliers;
            }
        }

        private ObservableCollection<InventoryLocationsItem> currentInventoryLocations = new ObservableCollection<InventoryLocationsItem>();
        public ObservableCollection<InventoryLocationsItem> CurrentInventoryLocations
        {
            get
            {
                return currentInventoryLocations;
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
            Parts.Clear();
            CurrentPart = null;

            //Define Query
            var query = (DataServiceQuery<PartsItem>)
                 context.Parts
                        .Where(p => p.SKU.StartsWith(SearchSku))
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
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                DataServiceQuery<PartsItem> query = 
                    (DataServiceQuery<PartsItem>)result.AsyncState;
                var parts = query.EndExecute(result);

                foreach (var part in parts)
                {
                    Parts.Add(part);
                }
            });
        }

        public void GetLocations()
        {
            currentInventoryLocations.Clear();

            if (currentPart != null)
            {
                var query =
                    (DataServiceQuery<InventoryLocationsItem>)
                    context.InventoryLocations.Where(
                        p => p.PartId == currentPart.Id).Select(
                            p =>
                            new InventoryLocationsItem { BinNumber = p.BinNumber, Id = p.Id, Quantity = p.Quantity, Title = p.Title, PartId = p.PartId });

                //Execute Query
                query.BeginExecute(DisplayLocations, query);
            }
        }

        private void DisplayLocations(IAsyncResult asyncResult)
        {
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                    {
                        DataServiceQuery<InventoryLocationsItem> query =
                            (DataServiceQuery<InventoryLocationsItem>) asyncResult.AsyncState;

                        //this delegate will execute when the results of the async query have returned
                        var partLocations = query.EndExecute(asyncResult);

                        foreach (var location in partLocations)
                        {
                            CurrentInventoryLocations.Add(location);
                        }
                    });
        }


        public void GetPartSuppliers()
        {
            CurrentPartSuppliers.Clear();

            if (currentPart != null)
            {
                DataServiceQuery<SuppliersItem> query =
                    (DataServiceQuery<SuppliersItem>)context.PartSuppliers.Where(s => s.PartId == currentPart.Id).Select(s => new SuppliersItem { Title = s.Supplier.Title, DUNS = s.Supplier.DUNS, Rating = s.Supplier.Rating });

                query.BeginExecute(DisplaySuppliers, query);
            }
        }

        private void DisplaySuppliers(IAsyncResult result)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                       {
                                           DataServiceQuery<SuppliersItem> query =
                                               (DataServiceQuery<SuppliersItem>) result.AsyncState;
                                           var partSuppliers = query.EndExecute(result);

                                           foreach (var partSupplier in partSuppliers)
                                           {
                                               CurrentPartSuppliers.Add(partSupplier);
                                           }
                                       });


        }


        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }

        private string ValidateSaveInputs(string bin, double? quantity)
        {
            string error = null;

            if (quantity==null || string.IsNullOrEmpty(bin))
            {
                error = "Bin number and quantity must be specified";
            }
            else if (CurrentPart == null)
            {
                error = "no Part Selected";
            }
            return error;
        }

        public string InventoryLocationSaved()
        {
            string error =  ValidateSaveInputs(currentLocation.BinNumber, currentLocation.Quantity);

            if (error == null)
            {
                //if null then its a new item, otherwise update existing
                if (CurrentLocation != newLocation)
                {
                    //CurrentLocation represents an object that is already in the data context and observable
                    //collection.  We just need to update the values.
                    context.UpdateObject(CurrentLocation);
                }
                else
                {
                    //This is a new item, so we need to add a new item to the context.
                    newLocation.PartId = CurrentPart.Id;

                    //Add to the data context
                    context.AddToInventoryLocations(newLocation);

                    //Add to the observable collection
                    this.currentInventoryLocations.Add(newLocation);

                    this.CurrentLocation = newLocation;
                    newLocation = null;
                }

                context.BeginSaveChanges(SaveChangesOptions.Batch, OnSaveChanges, null);
            }

            return error;
        }
    }


}
