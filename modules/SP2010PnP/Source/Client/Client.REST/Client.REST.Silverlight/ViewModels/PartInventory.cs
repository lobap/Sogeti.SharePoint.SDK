//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Client.REST.Silverlight.ListDataService;


namespace Client.REST.Silverlight.ViewModels
{
    public class PartInventory :INotifyPropertyChanged
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
                if (value == part) return;
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
