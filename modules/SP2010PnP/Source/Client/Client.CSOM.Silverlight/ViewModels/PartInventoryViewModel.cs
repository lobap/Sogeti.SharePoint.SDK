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
using System.Linq;
using System.Windows;
using Client.CSOM.Silverlight.Entities;
using Microsoft.SharePoint.Client;


namespace Client.CSOM.Silverlight.ViewModels
{
    public class PartInventoryViewModel : INotifyPropertyChanged, IDisposable
    {
        private ClientContext clientContext;
        private static string partsSiteURL = "/sites/sharepointlist/";
        private string searchSku;
        private ListItemCollection partListItems;
        private ListItemCollection inventoryLocationListItems;
        private ListItemCollection partSuppliersListItems;
        private PartInventory currentItem;

        public PartInventoryViewModel()
        {
            Uri appSource = App.Current.Host.Source;
            string fullPartsSiteUrl = string.Format("{0}://{1}:{2}{3}", appSource.Scheme, appSource.Host, appSource.Port, partsSiteURL);
            clientContext = new ClientContext(new Uri(fullPartsSiteUrl));
            Parts = new ObservableCollection<PartInventory>();
            CurrentPartSuppliers = new ObservableCollection<Supplier>();
        }
        
        public ObservableCollection<PartInventory> Parts { get; private set; }

        public ObservableCollection<Supplier> CurrentPartSuppliers { get; private set; }
        
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

        public void Update()
        {
            clientContext.ExecuteQueryAsync(onUpdatePartLocationSuccess, onQueryFailed);
        }

        public void GetParts()
        {
            Parts.Clear();
            CurrentPartSuppliers.Clear();

            List partsList = clientContext.Web.Lists.GetByTitle("Parts");
            List inventoryLocationsList = clientContext.Web.Lists.GetByTitle("Inventory Locations");

            CamlQuery camlQueryPartsList = new CamlQuery();
            camlQueryPartsList.ViewXml = @"<View>
                                            <Query>
                                                <Where>
                                                    <BeginsWith>
                                                        <FieldRef Name='SKU' />
                                                        <Value Type='Text'>" + SearchSku + @"</Value>
                                                    </BeginsWith>
                                                </Where>
                                            </Query>
                                        </View>";

            CamlQuery camlQueryInvLocationList = new CamlQuery();
            camlQueryInvLocationList.ViewXml = @"<View>
                                                    <Query>
                                                        <Where>
                                                            <BeginsWith>
                                                                <FieldRef Name='PartLookupSKU' />
                                                                <Value Type='Lookup'>" + SearchSku + @"</Value>
                                                            </BeginsWith>
                                                        </Where>
                                                        <OrderBy Override='TRUE'>
                                                            <FieldRef Name='PartLookupSKU' />
                                                        </OrderBy>
                                                    </Query>
                                                    <ViewFields>
                                                        <FieldRef Name='PartLookup' LookupId='TRUE' />
                                                        <FieldRef Name='PartLookupSKU' />
                                                        <FieldRef Name='PartLookupTitle' />
                                                        <FieldRef Name='PartLookupDescription' />
                                                        <FieldRef Name='BinNumber' />
                                                        <FieldRef Name='Quantity' />
                                                    </ViewFields>
                                                    <ProjectedFields>
                                                        <Field Name='PartLookupSKU' Type='Lookup' List='PartLookup' ShowField='SKU' />
                                                        <Field Name='PartLookupTitle' Type='Lookup' List='PartLookup' ShowField='Title' />
                                                        <Field Name='PartLookupDescription' Type='Lookup' List='PartLookup' ShowField='PartsDescription' />
                                                    </ProjectedFields>
                                                    <Joins>
                                                        <Join Type='LEFT' ListAlias='PartLookup'>
                                                            <!--List Name: Parts-->
                                                            <Eq>
                                                                <FieldRef Name='PartLookup' RefType='ID' />
                                                                <FieldRef List='PartLookup' Name='ID' />
                                                            </Eq>
                                                        </Join>
                                                    </Joins>
                                                </View>
";
            partListItems = partsList.GetItems(camlQueryPartsList);
            inventoryLocationListItems = inventoryLocationsList.GetItems(camlQueryInvLocationList);
            clientContext.Load(partListItems);
            clientContext.Load(inventoryLocationListItems);
            clientContext.ExecuteQueryAsync(onQuerySucceeded, onQueryFailed);
        }

        private void DisplayParts()
        {
            List<int> inventoryPartResults = new List<int>();

            //Populate BindingViewsModels with Parts with InventoryLocations
            foreach (ListItem inventoryLocationListItem in inventoryLocationListItems)
            {
                PartInventory view = new PartInventory();
                view.InventoryItem.Id = int.Parse(inventoryLocationListItem["ID"].ToString());
                view.InventoryItem.Quantity = int.Parse(inventoryLocationListItem["Quantity"].ToString());
                view.InventoryItem.BinNumber = inventoryLocationListItem["BinNumber"].ToString();
                view.Part.SKU = ((FieldLookupValue)inventoryLocationListItem["PartLookupSKU"]).LookupValue;
                view.Part.Title = ((FieldLookupValue)inventoryLocationListItem["PartLookupTitle"]).LookupValue;
                view.Part.Id = ((FieldLookupValue)inventoryLocationListItem["PartLookup"]).LookupId;
                view.Part.Description = ((FieldLookupValue)inventoryLocationListItem["PartLookupDescription"]).LookupValue;

                Parts.Add(view);
                inventoryPartResults.Add(view.Part.Id);
            }

            Collection<PartsItem> allPartResults = new Collection<PartsItem>();
            foreach (ListItem partListItem in partListItems)
            {
                PartsItem part = new PartsItem
                {
                    Id = int.Parse(partListItem["ID"].ToString()),
                    SKU = partListItem["SKU"].ToString(),
                    Title = partListItem["Title"].ToString()
                };
                allPartResults.Add(part);
            }

            Collection<PartsItem> noInventoryPartResults = new Collection<PartsItem>();
            foreach (var partResult in allPartResults.Where(part => !inventoryPartResults.Contains(part.Id)))
            {
                noInventoryPartResults.Add(partResult);
            }

            foreach (var part in noInventoryPartResults)
            {
                PartInventory view = new PartInventory();
                view.InventoryItem = null;

                view.Part.SKU = part.SKU;
                view.Part.Title = part.Title;
                view.Part.Id = part.Id;
                view.Part.Description = part.Description;

                Parts.Add(view);
            }
        }

        private void GetPartSuppliers()
        {
            if (currentItem != null)
            {
                List partSuppliersList = clientContext.Web.Lists.GetByTitle("Part Suppliers");
                CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml = @"<View>
                                          <Query>
                                            <Where>
                                              <Eq>
                                                <FieldRef Name='PartLookup' LookupId='TRUE' />
                                                <Value Type='Lookup'>" + currentItem.Part.Id + @"</Value>
                                              </Eq>
                                            </Where>
                                          </Query>
                                          <ViewFields>
                                            <FieldRef Name='SupplierLookupTitle' />
                                            <FieldRef Name='SupplierLookupDUNS' />
                                            <FieldRef Name='SupplierLookupRating' />
                                          </ViewFields>
                                          <ProjectedFields>
                                            <Field Name='SupplierLookupTitle' Type='Lookup' List='SupplierLookup' ShowField='Title' />
                                            <Field Name='SupplierLookupDUNS' Type='Lookup' List='SupplierLookup' ShowField='DUNS' />
                                            <Field Name='SupplierLookupRating' Type='Lookup' List='SupplierLookup' ShowField='Rating' />
                                          </ProjectedFields>
                                          <Joins>
                                            <Join Type='LEFT' ListAlias='SupplierLookup'>
                                              <!--List Name: Suppliers-->
                                              <Eq>
                                                <FieldRef Name='SupplierLookup' RefType='ID' />
                                                <FieldRef List='SupplierLookup' Name='ID' />
                                              </Eq>
                                            </Join>
                                          </Joins>
                                        </View>
                                        ";
                partSuppliersListItems = partSuppliersList.GetItems(camlQuery);
                clientContext.Load(partSuppliersListItems);

                //Get Supplier Data
                clientContext.ExecuteQueryAsync(onPartSupplierQuerySucceeded, onQueryFailed);
            }
         
        }

        private void DisplaySuppliers()
        {
            CurrentPartSuppliers.Clear();

            foreach (ListItem partSuppliersListItem in partSuppliersListItems)
            {
                Supplier supplier = new Supplier();
                supplier.Name = ((FieldLookupValue)partSuppliersListItem["SupplierLookupTitle"]).LookupValue;
                supplier.DUNS = ((FieldLookupValue)partSuppliersListItem["SupplierLookupDUNS"]).LookupValue;
                supplier.Rating = ((FieldLookupValue)partSuppliersListItem["SupplierLookupRating"]).LookupValue;

                CurrentPartSuppliers.Add(supplier);
            }
        }


        public void UpdateInventoryLocation()
        {
            List inventoryLocationsList = clientContext.Web.Lists.GetByTitle("Inventory Locations");
            ListItem inventoryLocation = null;

            inventoryLocation = inventoryLocationsList.GetItemById(currentItem.InventoryItem.Id);
            inventoryLocation["BinNumber"] = currentItem.InventoryItem.BinNumber;
            inventoryLocation["Quantity"] = currentItem.InventoryItem.Quantity;
            inventoryLocation.Update();
        }

        private void onUpdatePartLocationSuccess(object sender, ClientRequestSucceededEventArgs args)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                            {
                                                MessageBox.Show("Inventory Location Saved Successfully !");
                                            });

        }

        private void onPartSupplierQuerySucceeded(object sender, ClientRequestSucceededEventArgs args)
        {
            Deployment.Current.Dispatcher.BeginInvoke(DisplaySuppliers);
        }

        private void onQuerySucceeded(object sender, ClientRequestSucceededEventArgs args)
        {
            Deployment.Current.Dispatcher.BeginInvoke(DisplayParts);
        }

        private void onQueryFailed(object sender, ClientRequestFailedEventArgs args)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Execution Failed:" + args.Exception.InnerException.Message));
        }

        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                clientContext.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }
    }


}
