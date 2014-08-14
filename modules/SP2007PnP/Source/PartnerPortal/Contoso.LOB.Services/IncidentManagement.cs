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
using System.Globalization;
using System.ServiceModel.Activation;
using System.Threading;
using System.Web.Configuration;
using Contoso.LOB.Services.Contracts;
using Contoso.LOB.Services.Data;
using Contoso.LOB.Services.BusinessEntities;
using Contoso.LOB.Services.Security;

namespace Contoso.LOB.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class IncidentManagement : IIncidentManagement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public IncidentManagement()
        {
            this.SecurityHelper = new SecurityHelper();
        }

        #region IIncidentManagement Members

        public Incident GetIncident(string incidentId)
        {
            this.SecurityHelper.DemandAuthorizedPermissions();

            Incident incident = null;

            //Placing sleep here to simulate latency in service; most service do not have data source in memory
            Thread.Sleep(Convert.ToInt32(WebConfigurationManager.AppSettings.Get("sleepAmount"), CultureInfo.CurrentCulture));

            IncidentDataSet.IncidentRow[] rows = IncidentDataSet.Instance.Incident.Select(string.Format(CultureInfo.CurrentCulture, "Id = {0}", incidentId)) as IncidentDataSet.IncidentRow[];
            if (rows.Length > 0)
            {
                incident = new Incident();
                incident.Description = rows[0].Description;
                incident.Id = rows[0].Id.ToString(CultureInfo.CurrentCulture);
                incident.Partner = rows[0].Partner;
                incident.Status = rows[0].Status;
                incident.Product = rows[0].Product;
                incident.History = new List<string>();
                IncidentDataSet.IncidentHistoryRow[] historyRows = IncidentDataSet.Instance.IncidentHistory.Select(string.Format(CultureInfo.CurrentCulture, "IncidentId = {0}", incidentId)) as IncidentDataSet.IncidentHistoryRow[];
                if (historyRows.Length > 0)
                {
                    foreach (IncidentDataSet.IncidentHistoryRow historyRow in historyRows)
                    {
                        incident.History.Add(historyRow.Note);
                    }
                }
            }

            #region LINQ Example:
            // The logic above can be implemented using LINQ and .NET 3.5 language capabilities

            //var match = from incidentRow in IncidentDataSet.Instance.Incident
            //            join incidentHistoryRow in IncidentDataSet.Instance.IncidentHistory
            //            on incidentRow.Id equals incidentHistoryRow.IncidentId into incidentHistory
            //            where incidentRow.Id.ToString() == incidentId
            //            select new Incident
            //            {
            //                Description = incidentRow.Description,
            //                Id = incidentRow.Id.ToString(),
            //                Partner = incidentRow.Partner,
            //                Product = incidentRow.Product,
            //                Status = incidentRow.Status,
            //                History = incidentHistory.Select(p => p.Note).ToList()
            //            };

            //if (match.Count() > 0)
            //{
            //    incident = match.First();
            //}

            #endregion

            return incident;
        }

        public void WriteToHistory(string message)
        {
            SecurityHelper.DemandAuthorizedPermissions();
            // DO NOTHING
        }

        public IEnumerable<Incident> GetAllIncidents()
        {
            SecurityHelper.DemandAuthorizedPermissions();

            List<Incident> incidents = new List<Incident>();

            foreach (IncidentDataSet.IncidentRow row in IncidentDataSet.Instance.Incident)
            {
                Incident incident = new Incident();
                incident.Description = row.Description;
                incident.Id = row.Id.ToString(CultureInfo.CurrentCulture);
                incident.Partner = row.Partner;
                incident.Status = row.Status;
                incident.Product = row.Product;

                incidents.Add(incident);
            }

            return incidents;
        }

        #endregion

        protected ISecurityHelper SecurityHelper { get; set; }

    }
}