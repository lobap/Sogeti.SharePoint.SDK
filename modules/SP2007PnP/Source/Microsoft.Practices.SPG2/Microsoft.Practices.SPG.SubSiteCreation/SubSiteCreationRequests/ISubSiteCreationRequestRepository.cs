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

namespace Microsoft.Practices.SPG.SubSiteCreation
{
    /// <summary>
    /// Repository that facilitates in adding SubSiteCreationRequests to the SubSiteCreationRequest SPList. 
    /// 
    /// Adding a request to the list will start the SubSite Creation workflow. 
    /// </summary>
    public interface ISubSiteCreationRequestRepository
    {
        /// <summary>
        /// Add a subsite creation request to the SPList. 
        /// </summary>
        /// <param name="request">The subsite creation request to add.</param>
        void AddSubSiteCreationRequest(SubSiteCreationRequest request);
    }
}