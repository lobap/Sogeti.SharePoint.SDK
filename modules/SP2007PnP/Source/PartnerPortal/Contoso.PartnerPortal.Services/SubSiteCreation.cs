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
using System.Security.Permissions;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.Practices.SPG.SubSiteCreation;
using Contoso.Common;
using Contoso.PartnerPortal.PartnerDirectory;
using Microsoft.SharePoint.Security;
using Contoso.PartnerPortal.Services.Security;

namespace Contoso.PartnerPortal.Services
{
    public class SubSiteCreation : ISubSiteCreation
    {
        public SubSiteCreation()
        {
            this.SecurityHelper = new SecurityHelper();
        }

        #region ISubSiteCreation Members

        /// <summary>
        /// Creates a subsite for the business event type
        /// </summary>
        /// <param name="businessEvent"></param>
        /// <param name="eventIdentifier"></param>
        /// <param name="entityId"></param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void CreateSubSite(string businessEvent, string eventIdentifier, string entityId)
        {
            this.SecurityHelper.DemandAuthorizedPermissions();

            SubSiteCreationRequest request = new SubSiteCreationRequest();
            request.BusinessEvent = businessEvent;
            request.EventId = eventIdentifier;
            IPartnerSiteDirectory partnerSiteDirectory = SharePointServiceLocator.Current.GetInstance<IPartnerSiteDirectory>();
            request.SiteCollectionUrl = partnerSiteDirectory.GetPartnerSiteCollectionUrl(entityId);

            ISubSiteCreationRequestRepository repository = SharePointServiceLocator.Current.GetInstance<ISubSiteCreationRequestRepository>();
            repository.AddSubSiteCreationRequest(request);
        }

        #endregion

        protected ISecurityHelper SecurityHelper { get; set; }
    }
}
