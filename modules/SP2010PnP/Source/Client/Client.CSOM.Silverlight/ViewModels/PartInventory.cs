//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System.ComponentModel;
using Client.CSOM.Silverlight.Entities;


namespace Client.CSOM.Silverlight.ViewModels
{
    public class PartInventory : INotifyPropertyChanged
    {
        private PartsItem part = new PartsItem();
        public PartsItem Part
        {
            get
            {
                return part;
            }
            set
            {
                if (part == value) return;
                part = value;
                OnPropertyChanged("Part");
            }
        }

        private InventoryLocationsItem inventoryItem = new InventoryLocationsItem();
        public InventoryLocationsItem InventoryItem
        {
            get
            {
                return inventoryItem;
            }
            set
            {
                if (value == inventoryItem) return;
                inventoryItem = value;
                OnPropertyChanged("InventoryItem");
            }
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
