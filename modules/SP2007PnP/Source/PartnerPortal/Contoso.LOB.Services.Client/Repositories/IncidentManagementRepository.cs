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
using System.Net;
using System.Web.Hosting;
using Contoso.Common.Repositories;
using Contoso.LOB.Services.Client.IncidentProxy;
using Contoso.PartnerPortal.PartnerDirectory;
using Microsoft.Practices.SPG.Common.ServiceLocation;

// The types in the proxy have the same name as the types that are used by the app. Change the names of the proxy objects, to 
// avoid having to type the full namespace each time.  
using Incident = Contoso.Common.BusinessEntities.Incident;
using Microsoft.Practices.SPG.SubSiteCreation;
using Microsoft.SharePoint;
using Microsoft.Practices.ServiceLocation;


namespace Contoso.LOB.Services.Client.Repositories
{
    /// <summary>
    /// Repository for interacting with incidents in the system. 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Exposed through the service locator.")]
    internal class IncidentManagementRepository : IIncidentManagementRepository
    {
        private string partnerId;
        IPartnerSiteDirectory partnerSiteDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncidentManagementRepository"/> class.
        /// </summary>
        public IncidentManagementRepository()
        {
            // Retrieve the PartnerID in the constructor. You have to do this here, because, if you want to use this
            // repository in a ListItemEventReceiver, you only have access to the context (spcontext, http context and current user)
            // in the constructor of the splistitemeventreceiver. 
            partnerSiteDirectory = SharePointServiceLocator.Current.GetInstance<IPartnerSiteDirectory>();
            partnerId = partnerSiteDirectory.GetCurrentPartnerId();
        }


        #region IIncidentManagementRepository Members

        /// <summary>
        /// Write incidents to the incident history log.
        /// </summary>
        /// <param name="message">The message to write to the log</param>
        public void WriteToHistory(string message)
        {
            using (DisposableProxy<IIncidentManagement> client = GetClient())
            {
                client.Proxy.WriteToHistory(message);
            }
        }

        /// <summary>
        /// Gets an incident with specified incidentid from the LOB Services.
        /// </summary>
        /// <param name="incidentId">The Id of the incident to retrieve.</param>
        /// <returns>The incident. </returns>
        public Incident GetIncident(string incidentId)
        {
            // Impersonate as the AppPool account: 
            // As part of the trusted facade, we want to ensure that only the SharePoint server can access
            // the WCF LOB services. In order to check that, we have added a PrincipalPermissionAttribute that demands
            // that the calling account is part of a specific group. The app pool account is also part of that group
            // so we need to impersonate as the app pool account. 
            using (HostingEnvironment.Impersonate())
            {
                using (DisposableProxy<IIncidentManagement> client = GetClient())
                {
                    return TransformProxyIncidentToIncident(client.Proxy.GetIncident(incidentId));
                }
            }
        }

        #endregion

        private DisposableProxy<IIncidentManagement> GetClient()
        {
            // Create a proxy that's easy to dispose. The problem with the IPricing interface is, that it doesn't implement 
            // IDisposable. This DisposableProxy helps with that. 
            IncidentManagementClient client = new IncidentManagementClient();

            // Use the PartnerID as the username. We're using the trusted facade pattern. The service trusts this SharePoint app, so 
            // it trusts us to pass in the correct partner information. 
            client.ClientCredentials.UserName.UserName = partnerId;

            // The methods calling GetClient() should have wrapped the call into a HostingEnvironment.Impersonate(). That causes the 
            // DefaultNetworkCredentials to contain the App Pool Account. Note, if you look at the DefaultNetworkCredentials in the debugger
            // the properties will be empty. However, the credentials will still get set. 
            client.ClientCredentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;

            return new DisposableProxy<IIncidentManagement>(client);
        }

        /// <summary>
        /// Disposes the specified disposable.
        /// </summary>
        /// <param name="disposable">The disposable.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected void Dispose(IDisposable disposable)
        {
            if (disposable == null)
                return;

            disposable.Dispose();
        }

        /// <summary>
        /// Transform a incident object that's returned by the proxy to an incident object that's used by the application. 
        /// 
        /// This decouples the implementation of the services from the implementation of the application, so both can evolve seperately. 
        /// </summary>
        /// <param name="incident">The proxy incident to trancform</param>
        /// <returns>The incident. </returns>
        private static Incident TransformProxyIncidentToIncident(IncidentProxy.Incident incident)
        {
            Incident newIncident = new Incident();
            newIncident.Description = incident.Description;
            newIncident.History = incident.History;
            newIncident.Id = incident.Id;
            newIncident.Partner = incident.Partner;
            newIncident.Product = incident.Product;
            newIncident.Status = incident.Status;

            return newIncident;
        }
    }
}