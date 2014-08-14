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
using Microsoft.SharePoint;

namespace Contoso.TrainingManagement.Repository
{
    public interface IListItemRepository
    {
        /// <summary>
        /// Designed to add a new SPListItem to a list based on the specified collection of field values
        /// </summary>        
        SPListItem Add(SPWeb web, string listName, Dictionary<Guid, object> fields);

        /// <summary>
        /// Designed to get a SPListItem based on the specified query
        /// </summary>
        SPListItem Get(SPWeb web, string listName, SPQuery query);

        /// <summary>
        /// Designed to get a SPListItemCollection of all items in a list
        /// </summary>
        SPListItemCollection Get(SPWeb web, string listName);

        /// <summary>
        /// Designed to update a SPListItem in a list based on the specified collection of field values
        /// </summary>
        void Update(SPWeb web, string listName, int listItemId, Dictionary<Guid, object> fields);

        /// <summary>
        /// Designed to delete a SPListItem from a list based on the specified Id
        /// </summary>
        void Delete(SPWeb web, string listName, int listItemId);                
    }
}
