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
    /// Repository that helps in creating business event type configurations. 
    /// </summary>
    public interface IBusinessEventTypeConfigurationRepository
    {
        /// <summary>
        /// Return the configuration for a particular business event type. 
        /// </summary>
        /// <param name="businessEvent">The key of the business event type.</param>
        /// <returns>The configuration for the business event type. </returns>
        BusinessEventTypeConfiguration GetBusinessEventTypeConfiguration(string businessEvent);
    }
}