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
using Contoso.Common;
using Microsoft.Office.Server;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.Practices.SPG.Common.Configuration;
using Microsoft.Practices.SPG.Common.ListRepository;
using Microsoft.Practices.SPG.Common.Logging;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint;

namespace Contoso.PartnerPortal.PartnerDirectory
{
    /// <summary>
    /// The partner site directory, that allows you to retrieve partner site collection URLs, and determine what the currently logged on user is. 
    /// 
    /// The PartnerSiteDirectory class depends upon a MOSS Site Directory site to exist. In addition, the "Sites" list must be modified to include a "PartnerId" field.
    /// </summary>
    public class PartnerSiteDirectory : IPartnerSiteDirectory
    {
        private readonly string partnerDirectoryUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerSiteDirectory"/> class.
        /// </summary>
        public PartnerSiteDirectory()
        {
            partnerDirectoryUrl = GetPartnerSiteDirectoryUrlConfigSetting();
        }

        #region IPartnerSiteDirectory Members

        /// <summary>
        /// Retrieve the ID of the partner that the currently logged on user belongs to.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public string GetCurrentPartnerId()
        {
            if (SPContext.Current == null)
                throw new NoSharePointContextException("PartnerSiteDirectory should run inside a SharePoint context.");

            string accountName = SPContext.Current.Web.CurrentUser.LoginName;
            // If the LoginName is "SharePoint\system", try getting the Current Principal's username from the thread;
            // there is no way to map the "SharePoint\system" username in the user profile.
            if(string.Equals(accountName, "Sharepoint\\system", StringComparison.CurrentCultureIgnoreCase))
            {
                accountName = System.Threading.Thread.CurrentPrincipal.Identity.Name;
            }

            return GetPartnerIdFromUserProfile(accountName);
        }

        /// <summary>
        /// Retrieve the URL of the site collection of a particular partner.
        /// </summary>
        /// <param name="partnerId">The Id of the partner</param>
        /// <returns>The URL of the partner site collection. </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"),
         System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
             Justification = "Working with strings is easier here.")]
        public string GetPartnerSiteCollectionUrl(string partnerId)
        {
            // To prevent external code from scanning partner site collection URLs, this code will not run
            // in elevated mode. If the caller does not have access to the Partner Site Directory, then
            // will not be able to retrieve any partner information.
            return GetPartnerSiteCollectionUrl(partnerId, false);
        }      

        /// <summary>
        /// Retrieve the URL of the site collection of the partner that the currently logged on user belongs to.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"),
         System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
             Justification = "Working with strings is easier here.")]
        public string GetPartnerSiteCollectionUrl()
        {
            // The PartnerDirectory should be locked down for all user except for Administrators for Viewing.
            // Users will need to resolve their own site collection URLs in other parts of code that rely on
            // the PartnerDirectory. For this reason, we will elevate the current users privileges to get the
            // the site collection URL for only their current partner id.
            return GetPartnerSiteCollectionUrl(GetCurrentPartnerId(), true);
        }

        /// <summary>
        /// Return a list of all partner sites in the system.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"),
         System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
             Justification = "Working with strings is easier here.")]
        public IEnumerable<PartnerSiteDirectoryEntry> GetAllPartnerSites()
        {
            List<PartnerSiteDirectoryEntry> partnerSiteDirectoryEntries = new List<PartnerSiteDirectoryEntry>();

            // Query for items in the Site Directory where the Partner field contains some value
            CAMLQueryBuilder camlQueryBuilder = new CAMLQueryBuilder();
            camlQueryBuilder.AddNotEqual("PartnerDirectoryPartnerField", string.Empty);
            SPQuery query = camlQueryBuilder.Build();
            ILogger logger = SharePointServiceLocator.Current.GetInstance<ILogger>();
            logger.TraceToDeveloper(string.Format(CultureInfo.CurrentCulture,
                                                  "PartnerSiteDirectory FindPartnerMappingForCurrentPartner CAML: {0}",
                                                  query.Query));

            using (SPSite site = new SPSite(partnerDirectoryUrl))
            {
                using (SPWeb siteDirectory = site.OpenWeb())
                {
                    SPList siteCollectionMappingList = siteDirectory.Lists["Sites"];
                    SPListItemCollection items = siteCollectionMappingList.GetItems(query);

                    foreach (SPListItem item in items)
                    {
                        PartnerSiteDirectoryEntry partnerSiteDirectoryEntry = new PartnerSiteDirectoryEntry();
                        partnerSiteDirectoryEntry.PartnerId = (string) item["PartnerDirectoryPartnerField"];
                        string url = item["URL"].ToString();
                        partnerSiteDirectoryEntry.SiteCollectionUrl = url.Split(",".ToCharArray())[0];
                        partnerSiteDirectoryEntry.Title = (string) item["Title"];

                        partnerSiteDirectoryEntries.Add(partnerSiteDirectoryEntry);
                    }
                }
            }
            return partnerSiteDirectoryEntries;
        }

        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private string GetPartnerSiteCollectionUrl(string partnerId, bool elevatePrivileges)
        {
            string url = string.Empty;

            SPListItem item = null;

            if (elevatePrivileges)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    FindPartnerMapping(partnerId, out item);
                });
            }
            else
            {
                FindPartnerMapping(partnerId, out item);
            }

            if (item != null)
            {
                url = item["URL"].ToString();
                url = url.Split(",".ToCharArray())[0];
            }
            else
            {
                ThrowPartnerInfoNotInSiteDirectoryException(partnerId);
            }

            return url;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private string GetPartnerSiteDirectoryUrlConfigSetting()
        {
            // Read the PartnerSiteDirectoryUrl from the Hierarchical config. This value is stored at the SPFarm level, because
            // there can only be 1 PartnerSiteDirectory in our environment. Therefor it doesn't make sense to override this setting
            // at different levels. 
            IHierarchicalConfig hierarchicalConfig = SharePointServiceLocator.Current.GetInstance<IHierarchicalConfig>();
            return hierarchicalConfig.GetByKey<string>(Constants.PartnerSiteDirectoryUrlConfigKey,
                                                       ConfigLevel.CurrentSPFarm);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private string GetPartnerIdFromUserProfile(string accountName)
        {
            UserProfile userProfile = null;
            ServerContext serverContext = ServerContext.GetContext(SPContext.Current.Site);
            UserProfileManager userProfileManager = new UserProfileManager(serverContext);

            if (!userProfileManager.UserExists(accountName))
            {
                ThrowPartnerIdNotInUserProfileException(null);
            }

            userProfile = userProfileManager.GetUserProfile(accountName);
            if (userProfile["PartnerId"] == null)
            {
                ThrowPartnerIdNotInUserProfileException(null);
            }
            return userProfile["PartnerId"].Value.ToString();
        }

        private static void ThrowPartnerIdNotInUserProfileException(Exception ex)
        {
            string errorMessage = string.Format(CultureInfo.CurrentCulture,
                                                "PartnerId for the current user ({0}) could not be found in user profile.",
                                                System.Threading.Thread.CurrentPrincipal.Identity.Name);

            ILogger logger = SharePointServiceLocator.Current.GetInstance<ILogger>();
            logger.LogToOperations(errorMessage, (int) EventLogEventId.PartnerNotFound);

            throw new PartnerNotFoundException(errorMessage, ex);
        }

        private static void ThrowPartnerInfoNotInSiteDirectoryException(string partnerId)
        {
            string errorMessage = string.Format(CultureInfo.CurrentCulture,
                                                "Partner information for the PartnerId ({0}) could not be found",
                                                partnerId);

            ILogger logger = SharePointServiceLocator.Current.GetInstance<ILogger>();
            logger.LogToOperations(errorMessage, (int) EventLogEventId.PartnerNotFound);

            throw new PartnerNotFoundException(errorMessage);
        }

        private void FindPartnerMapping(string partnerId, out SPListItem partnerDirectoryEntry)
        {
            // The PartnerSiteDirectory class depends upon a MOSS Site Directory site to exist. In addition, the
            // "Sites" list must be modified to include a "PartnerId" field. If these conditions are not meant,
            // then FindPartnerMappingForCurrentPartner method will not successfully retrieve the correct mapping
            // for the partner's site collection.
            using (SPSite site = new SPSite(partnerDirectoryUrl))
            {
                using (SPWeb siteDirectory = site.OpenWeb())
                {
                    SPList siteCollectiongMappingList = siteDirectory.Lists["Sites"];
                    partnerDirectoryEntry = ExecutePartnertSiteDirectoryQuery(siteCollectiongMappingList, partnerId);
                }
            }
        }

        private static SPListItem ExecutePartnertSiteDirectoryQuery(SPList siteCollectiongMappingList, string partnerId)
        {
            SPListItem itemFound = null;
            CAMLQueryBuilder camlQueryBuilder = new CAMLQueryBuilder();
            camlQueryBuilder.AddEqual(FieldIds.PartnerFieldId, partnerId);

            SPQuery query = camlQueryBuilder.Build();
            ILogger logger = SharePointServiceLocator.Current.GetInstance<ILogger>();
            logger.TraceToDeveloper(string.Format(CultureInfo.CurrentCulture,
                                                  "PartnerSiteDirectory FindPartnerMappingForCurrentPartner CAML: {0}",
                                                  query.Query));

            SPListItemCollection items = siteCollectiongMappingList.GetItems(query);
            if (items.Count > 0)
            {
                itemFound = items[0];
            }

            return itemFound;
        }
    }
}