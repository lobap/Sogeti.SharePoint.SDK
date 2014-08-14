//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


namespace Client.SPWebService.Silverlight.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Xml.Linq;
    using SPListsWebService;

    public class PartInventoryViewModel : INotifyPropertyChanged
    {
        private string numberOfPartsFound;
        private string numberOfSuppliersFound;
        private Part currentPart;
        private string searchSku;

        public PartInventoryViewModel()
        {
            Parts = new ObservableCollection<Part>();
            Suppliers = new ObservableCollection<string>();
        }

        public ObservableCollection<Part> Parts { get; private set;}

        public ObservableCollection<string> Suppliers { get; private set;}

        public string NumberOfPartsFound
        {
            get { return numberOfPartsFound; }
            set
            {
                if (value == numberOfPartsFound) return;
                numberOfPartsFound = value;
                OnPropertyChanged("NumberOfPartsFound");
            }
        }

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

        public Part CurrentPart
        {
            get { return currentPart;}
            set
            {
                if (value == currentPart) return;
                currentPart = value;
                GetPartSuppliers();
                OnPropertyChanged("CurrentPart");
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

        public void GetParts()
        {
            Parts.Clear();

            ListsSoapClient proxy = GetListServiceProxy();
            proxy.GetListItemsCompleted += proxy_GetPartsListItemsCompleted;
            var query = XElement.Parse(@"<Query>
                                            <Where>
                                                <BeginsWith>
                                                    <FieldRef Name='SKU' />
                                                    <Value Type='Text'>" + SearchSku + @"</Value>
                                                </BeginsWith>
                                            </Where>
                                          </Query>");
            var queryOptions = XElement.Parse(@"<QueryOptions></QueryOptions>");
            var viewFields = XElement.Parse(@"<ViewFields></ViewFields>");
            proxy.GetListItemsAsync("Parts", null, query, viewFields, null, queryOptions, null);
        }

        private ListsSoapClient GetListServiceProxy()
        {
            ListsSoapClient proxy = new ListsSoapClient();
            string hostName = App.Current.Host.Source.Host;
            string endPointUrl = proxy.Endpoint.Address.ToString().Replace("localhost", hostName);
            proxy.Endpoint.Address = new System.ServiceModel.EndpointAddress(endPointUrl);
            return proxy;
        }

        void proxy_GetPartsListItemsCompleted(object sender, GetListItemsCompletedEventArgs e)
        {
            XNamespace rowsetNamespace = "#RowsetSchema";
            var query = from x in e.Result.Descendants()
                        where x.Name == rowsetNamespace + "row"
                        select x.Attribute("ows_Title").Value;

            foreach (string partTitle in query)
            {
                Parts.Add(new Part { Title = partTitle });
            }

            XElement rsData = e.Result.Descendants().First<XElement>(f => f.Name.LocalName == "data");

            NumberOfPartsFound = rsData.Attributes("ItemCount").First().Value;
        }

        public void GetPartSuppliers()
        {
            Suppliers.Clear();

            ListsSoapClient proxy = GetListServiceProxy();
            proxy.GetListItemsCompleted += proxy_GetSuppliersListItemsCompleted;
            var query = XElement.Parse(@"<Query>
                                            <Where>
                                                <Eq>
                                                    <FieldRef Name='PartLookup' />
                                                    <Value Type='Lookup'>" + CurrentPart.Title + @"</Value>
                                                </Eq>
                                            </Where>
                                          </Query>");
            var queryOptions = XElement.Parse(@"<QueryOptions></QueryOptions>");
            var viewFields = XElement.Parse(@"<ViewFields></ViewFields>");
            proxy.GetListItemsAsync("Part Suppliers", null, query, viewFields, null, queryOptions, null);
        }

        void proxy_GetSuppliersListItemsCompleted(object sender, GetListItemsCompletedEventArgs e)
        {
            XNamespace rowsetNamespace = "#RowsetSchema";
            var query = from x in e.Result.Descendants()
                        where x.Name == rowsetNamespace + "row"
                        select x.Attribute("ows_SupplierLookup").Value;

            foreach (var supplierLookupValue in query)
            {
                Suppliers.Add(supplierLookupValue);
            }

            XElement rsData = e.Result.Descendants().First<XElement>(f => f.Name.LocalName == "data");

            NumberOfSuppliersFound = rsData.Attributes("ItemCount").First().Value;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
