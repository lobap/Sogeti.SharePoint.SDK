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
using System.Web.UI;
using DataModels.SharePointList.Model;
using Microsoft.SharePoint;

namespace DataModels.SharePointList.PartsMgmnt.PartsMgmntControls
{
    public partial class PartsBySku : UserControl
    {
        public event EventHandler<GenericEventArgs<IEnumerable<Part>>> OnPartsFound;
        public int CurrentPage { get; set; }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            GoButton.Click += GoButton_Click;
            AddNewHyperlink.NavigateUrl = string.Concat("javascript: ShowModalDialog('",
                                                        SPContext.Current.Site.RootWeb.Url,
                                                        "/", SharePointList.Model.Constants.ListUrls.Parts, "/NewForm.aspx?IsDlg=1');");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void GoButton_Click(object sender, EventArgs eventArgs)
        {
            GetData();
        }

        public void GetData()
        {
            using (var partManagementRepository = new PartManagementRepository())
            {
                IEnumerable<Part> parts;
                if (CurrentPage > 0)
                {
                    parts = partManagementRepository.GetPartsByPartialSkuPaged(PartSearchTextBox.Text, CurrentPage, Constants.DeafultPageSize);
                }
                else
                {
                    parts = partManagementRepository.GetPartsByPartialSku(PartSearchTextBox.Text);   
                }

                var partsEventArgs = new GenericEventArgs<IEnumerable<Part>>();
                partsEventArgs.PayLoad = parts;

                if (OnPartsFound != null)
                {
                    PartsFound(partsEventArgs);
                }
            }
        }

        protected virtual void PartsFound(GenericEventArgs<IEnumerable<Part>> e)
        {
            OnPartsFound(this, e);
        }
    }
}
