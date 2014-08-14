//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
namespace Client.ExtService.Silverlight.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;
    using Entities;
    using Microsoft.SharePoint.Client;
    using VendorService;

    public class SupplierViewModel : INotifyPropertyChanged, IDisposable
    {
        private ClientContext clientContext;
        private readonly string partsSiteURL = "/sites/sharepointlist/";
        private ListItemCollection supplierListItems;
        private string numberOfSuppliersFound;
        private string errorText;
        private string accountsPayableValue;
        private Supplier currentSupplier;
        private Cursor cursor;

        public SupplierViewModel()
        {
            Uri appSource = App.Current.Host.Source;
            string fullPartsSiteUrl = string.Format("{0}://{1}:{2}{3}", appSource.Scheme, appSource.Host, appSource.Port, partsSiteURL);
            clientContext = new ClientContext(new Uri(fullPartsSiteUrl));
            Suppliers = new ObservableCollection<Supplier>();
        }

        public ObservableCollection<Supplier> Suppliers { get; private set; }

        public string NumberOfSuppliersFound
        {
            get { return numberOfSuppliersFound; }
            set
            {
                if (value == numberOfSuppliersFound) return;
                numberOfSuppliersFound = value;
                OnPropertyChanged("NumberOfSuppliersFound");
            }
        }

        public string AccountsPayableValue
        {
            get { return accountsPayableValue; }
            set
            {
                if (value == accountsPayableValue) return;
                accountsPayableValue = value;
                OnPropertyChanged("AccountsPayableValue");
            }
        }

        public string ErrorText
        {
            get { return errorText; }
            set
            {
                if (value == errorText) return;
                errorText = value;
                OnPropertyChanged("ErrorText");
            }
        }

        public Supplier CurrentSupplier
        {
            get { return currentSupplier; }
            set
            {
                if (value == currentSupplier) return;
                currentSupplier = value;
                RetrieveVendorData();
            }
        }

        public Cursor Cursor
        {
            get { return cursor; }
            set
            {
                if (value == cursor) return;
                cursor = value;
                OnPropertyChanged("Cursor");
            }
        }

        private void RetrieveVendorData()
        {
            this.Cursor = Cursors.Wait;
            try
            {
                var externalServiceProxy = new VendorService.VendorServicesClient();
                string hostName = App.Current.Host.Source.Host;
                string endPointUrl = externalServiceProxy.Endpoint.Address.ToString().Replace("localhost", hostName);
                externalServiceProxy.Endpoint.Address = new System.ServiceModel.EndpointAddress(endPointUrl);

                externalServiceProxy.GetAccountsPayableCompleted += externalServiceProxy_GetAccountsPayableCompleted;
                externalServiceProxy.GetAccountsPayableAsync(currentSupplier.DUNS);

                externalServiceProxy.WhoAmICompleted += externalServiceProxy_WhoAmICompleted;
                externalServiceProxy.WhoAmIAsync();
            }
            catch (System.Exception ex)
            {
                ErrorText = ex.Message;
            }
        }

        private void externalServiceProxy_WhoAmICompleted(object sender, WhoAmICompletedEventArgs e)
        {
            ErrorText = e.Result;
        }

        private void externalServiceProxy_GetAccountsPayableCompleted(object sender, GetAccountsPayableCompletedEventArgs e)
        {
            try
            {
                AccountsPayableValue = String.Format("{0:C}", e.Result);
            }
            catch (System.Exception ex)
            {
                ErrorText = ex.ToString();
            }

            this.Cursor = Cursors.Arrow;
        }

        public void RetrieveSuppliers()
        {
            Suppliers.Clear();
            List supplierList = clientContext.Web.Lists.GetByTitle("Suppliers");

            var camlQuerySupplierList = new CamlQuery();
            camlQuerySupplierList.ViewXml = @"<View>
                                            <Query>
                                                <Where>
                                                    <IsNotNull>
                                                        <FieldRef Name='DUNS' />
                                                    </IsNotNull>
                                                </Where>
                                            </Query>
                                        </View>";

            supplierListItems = supplierList.GetItems(camlQuerySupplierList);
            clientContext.Load(supplierListItems);
            clientContext.ExecuteQueryAsync(onSupplierQuerySucceeded, onQueryFailed);
            this.Cursor = Cursors.Wait;
        }

        private void onSupplierQuerySucceeded(object sender, ClientRequestSucceededEventArgs args)
        {
            Deployment.Current.Dispatcher.BeginInvoke(ShowSuppliers);
        }

        private void ShowSuppliers()
        {
            foreach (ListItem supplierListItem in supplierListItems)
            {
                var supplier = new Supplier
                {
                    Name = supplierListItem["Title"].ToString(),
                    DUNS = supplierListItem["DUNS"].ToString(),
                    Rating = Convert.ToDouble(supplierListItem["Rating"]),
                };
                Suppliers.Add(supplier);
            }

            NumberOfSuppliersFound = Suppliers.Count.ToString();
            this.Cursor = Cursors.Arrow;
        }

        private void onQueryFailed(object sender, ClientRequestFailedEventArgs args)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Execution Failed:" + args.Exception.InnerException.Message));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
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
