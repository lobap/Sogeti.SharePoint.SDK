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

using Microsoft.SharePoint;

namespace Contoso.TrainingManagement.Repository
{
    /// <summary>
    /// The SPListManager class provides functinality to interact with SharePoint Lists
    /// </summary>
    public class ListItemRepository : IListItemRepository
    {
        #region Methods

        /// <summary>
        /// The Add method will add a new SPListItem to the specified list with a set of values.
        /// </summary>
        /// <param name="web">The SPWeb of the SPList</param>
        /// <param name="listName">The name of the SPList</param>
        /// <param name="fields">Dictionarty containing unique SPField ID's and values</param>
        /// <returns>The new SPListItem added to the SPList</returns>
        public SPListItem Add(SPWeb web, string listName, Dictionary<Guid, object> fields)
        {
            SPListItem newItem = null;

            newItem = web.Lists[listName].Items.Add();

            foreach ( Guid key in fields.Keys )
            {
                newItem[key] = fields[key];
            }

            newItem.Update();

            return newItem;
        }

        /// <summary>
        /// The Get method will get an SPListItem from the specified list based on a specified query.
        /// </summary>
        /// <param name="web">The SPWeb of the SPList</param>
        /// <param name="listName">The name of the SPList</param>
        /// <param name="query">The SPQuery to find the item</param>
        /// <returns>The SPListItem in the SPList</returns>
        public SPListItem Get(SPWeb web, string listName, SPQuery query)
        {
            SPListItem item = null;
            SPListItemCollection collection = null;

            collection = web.Lists[listName].GetItems(query);

            if ( collection != null && collection.Count > 0 )
            {
                item = collection[0];
            }

            return item;
        }

        /// <summary>
        /// The Get method will get a SPListItemCollection from the specified list of all items.
        /// </summary>
        /// <param name="web">The SPWeb of the SPList</param>
        /// <param name="listName">The name of the SPList</param>
        /// <returns>The SPListItemCollection in the SPList</returns>
        public SPListItemCollection Get(SPWeb web, string listName)
        {
            SPListItemCollection collection = null;

            collection = web.Lists[listName].Items;

            return collection;
        }

        /// <summary>
        /// The Update method will update a SPListItem in the specified list with a set of values.
        /// </summary>
        /// <param name="web">The SPWeb of the SPList</param>
        /// <param name="listName">The name of the SPList</param>
        /// <param name="listItemId">The Id of the list item to update</param>
        /// <param name="fields">Dictionarty containing unique SPField ID's and values</param>
        public void Update(SPWeb web, string listName, int listItemId, Dictionary<Guid, object> fields)
        {
            SPListItem item = null;
            SPListItemCollection collection = null;

            collection = web.Lists[listName].GetItems(BuildQuery(listItemId));

            if ( collection != null && collection.Count > 0 )
            {
                item = collection[0];

                foreach ( Guid key in fields.Keys )
                {
                    item[key] = fields[key];
                }

                item.Update();
            } 
        }

        /// <summary>
        /// The Delete method will delete a SPListItem in the specified list.
        /// </summary>
        /// <param name="web">The SPWeb of the SPList</param>
        /// <param name="listName">The name of the SPList</param>
        /// <param name="listItemId">The Id of the list item to update</param>
        public void Delete(SPWeb web, string listName, int listItemId)
        {
            SPListItem item = null;
            SPListItemCollection collection = null;

            collection = web.Lists[listName].GetItems(BuildQuery(listItemId));

            if ( collection != null && collection.Count > 0 )
            {
                item = collection[0];

                item.Delete();
            }
        } 

        private static SPQuery BuildQuery(int listItemId)
        {
            SPQuery query = new SPQuery();
            query.Query = string.Format(CultureInfo.InvariantCulture, "<Where><Eq><FieldRef Name='ID'/><Value Type='Integer'>{0}</Value></Eq></Where>", listItemId);

            return query;
        }

        #endregion
    }
}
