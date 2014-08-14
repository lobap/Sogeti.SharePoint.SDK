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

namespace Contoso.Common.ExceptionHandling
{
    /// <summary>
    /// Enum that determines what to do with a list item after an exception has occurred
    /// </summary>
    public enum ListItemAction
    {
        /// <summary>
        /// Don't cancel the the action on the item after an exception occurs
        /// </summary>
        Continue, 

        /// <summary>
        /// Cancel the action on the item when an exception occurs. 
        /// </summary>
        Cancel
    }
}