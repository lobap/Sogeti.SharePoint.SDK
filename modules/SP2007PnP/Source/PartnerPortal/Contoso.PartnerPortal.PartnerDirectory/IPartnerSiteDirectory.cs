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
using System.Linq;
using System.Text;

namespace Contoso.PartnerPortal.PartnerDirectory
{
    /// <summary>
    /// Interface for the partner site directory. This directory contains the URLs to the site collections of the partners. It also allows you to
    /// retrieve information about the currently logged on user. 
    /// </summary>
    public interface IPartnerSiteDirectory
    {

        /// <summary>
        /// Retrieve the ID of the partner that the currently logged on user belongs to. 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        string GetCurrentPartnerId();

        /// <summary>
        /// Retrieve the URL of the site collection of the partner that the currently logged on user belongs to. 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")]
        string GetPartnerSiteCollectionUrl();

        /// <summary>
        /// Retrieve the URL of the site collection of a particular patner. 
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")]
        string GetPartnerSiteCollectionUrl(string partnerId);

        /// <summary>
        /// Return a list of all partner sites in the system. 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IEnumerable<PartnerSiteDirectoryEntry> GetAllPartnerSites();
    }
}
