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

namespace Client.CSOM.Silverlight.Entities
{
    public class InventoryLocationsItem: INotifyPropertyChanged
    {
        private int id = 0;
        public int Id
        {
            get
            {
                return id;
            } 
            set
            {
                id = value;
                OnPropertyChanged("ID");
            }
        }

        private int quantity = 0;
        public int Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        private string binNumber = string.Empty;
        public string BinNumber
        {
            get
            {
                return binNumber;
            }
            set
            {
                binNumber = value;
                OnPropertyChanged("BinNumber");
            }
        }

        private int partId = 0;
        public int PartId 
         {
            get
            {
                return partId;
            } 
            set
            {
                partId = value;
                OnPropertyChanged("PartID");
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
