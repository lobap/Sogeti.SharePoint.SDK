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
using System.Data.Services.Client;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Client.REST.Silverlight.Alt.ListDataService;

namespace Client.REST.Silverlight.Alt
{
    public delegate void PartUpdated(object sender, EventArgs e);

    public partial class EditPart : UserControl
    {
        public event PartUpdated CurrentPartUpdated;

        PartsDataContext context = new PartsDataContext(new Uri("http://localhost/sites/sharepointlist/_vti_bin/listdata.svc"));

        public EditPart()
        {
            InitializeComponent();
        }

        void RaisePartUpdated(RoutedEventArgs e)
        {
            if (CurrentPartUpdated != null)
            {
                CurrentPartUpdated(this, e);
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            PartsItem currentItem = (PartsItem)this.DataContext;
            currentItem.Description = descriptionTextBox.Text;
            currentItem.SKU = skuTextBox.Text;
            currentItem.Title = nameTextBox.Text;

            context.MergeOption = MergeOption.OverwriteChanges;

            //See if the entity is being tracked and attach if not
            EntityDescriptor descriptor = context.GetEntityDescriptor(currentItem);
            if (!context.Entities.Contains(descriptor))
            {
                context.AttachTo("Parts", currentItem, "*");
            }
            context.UpdateObject(currentItem);
            context.BeginSaveChanges(SaveChangesOptions.ReplaceOnUpdate, PartUpdatedCallback, context);
        }

        private void PartUpdatedCallback(IAsyncResult result)
        {
            Dispatcher.BeginInvoke(() =>
                                       {
                                           context.EndSaveChanges(result);
                                           RoutedEventArgs e = new RoutedEventArgs();
                                           RaisePartUpdated(e);
                                       });
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
