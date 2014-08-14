//===============================================================================
// Microsoft patterns & practices
// SharePoint Guidance version 2.0
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Contoso.Common.BusinessEntities;
using Contoso.Common.Repositories;
using Microsoft.Office.Server.Search.Query;
using Microsoft.Office.Server;
using Microsoft.SharePoint.Administration;
using Microsoft.Practices.SPG.Common.ServiceLocation;

using Contoso.PartnerPortal.PartnerCentral.Properties;
using System.Globalization;
using Microsoft.Practices.SPG.Common.Logging;
using Contoso.PartnerPortal.PartnerDirectory;

namespace Contoso.PartnerPortal.PartnerCentral.PartnerRollup
{
    /// <summary>
    /// This class holds the testable logic for the PartnerRollup webpart. 
    /// 
    /// This class is the Presenter in the Model View Presenter pattern. 
    /// </summary>
    public class PartnerRollupPresenter
    {
        IPartnerRollupView view;

        public PartnerRollupPresenter(IPartnerRollupView view)
        {
            this.view = view;
        }

        public void ReturnSearchResults()
        {
            // First query the Partner Site Directory to get entries for each partner
            IPartnerSiteDirectory partnerSiteDirectory = SharePointServiceLocator.Current.GetInstance<IPartnerSiteDirectory>();
            var partnerSites = partnerSiteDirectory.GetAllPartnerSites();

            // Then execute the search query. This logic is performed by the IncidentTaskRepository. 
            IIncidentTaskRepository incidentTaskRepository =
                SharePointServiceLocator.Current.GetInstance<IIncidentTaskRepository>();
            var incidentTasks = incidentTaskRepository.GetAllOpenIncidentTasks();

            // Merge the results from both here to get an aggregated view
            var partnerRollupResults = new List<PartnerRollupSearchResult>();
            foreach (PartnerSiteDirectoryEntry entry in partnerSites)
            {
                PartnerRollupSearchResult aResult = new PartnerRollupSearchResult();
                aResult.Partner = entry;

                foreach (IncidentTask incidentTask in incidentTasks)
                {
                    if (incidentTask.Path.StartsWith(entry.SiteCollectionUrl, false, CultureInfo.CurrentCulture))
                    {
                        aResult.IncidentTasks.Add(incidentTask);
                    }
                }

                partnerRollupResults.Add(aResult);
            }

            // Forward the data to the View. The view is then responsible for rendering the list. 
            this.view.SetData(partnerRollupResults);
        }


    }
}
