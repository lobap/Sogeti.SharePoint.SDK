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

namespace Contoso.Common
{
    /// <summary>
    /// Enumeration for common EventLog event id's that should be used. 
    /// </summary>
    public enum EventLogEventId
    {
        /// <summary>
        /// Empty EventLog id.
        /// </summary>
        None = 0, 

        /// <summary>
        /// Indicatese that a partner could not be found.
        /// </summary>
        PartnerNotFound = 1,

        /// <summary>
        /// Indicates that a particular sku could not be found. 
        /// </summary>
        SkuNotFound = 2
    }
}
