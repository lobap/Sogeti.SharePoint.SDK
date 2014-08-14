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
using Contoso.TrainingManagement.Repository;
using Microsoft.SharePoint;

namespace Contoso.TrainingManagement.Mocks
{
    public class MockListItemRepository : IListItemRepository
    {
        #region Public Properties

        public static SPListItem SPListItemReturnedByGet
        {
            get;
            set;
        }

        public static SPListItemCollection SPListItemCollectionReturnedByGet
        {
            get;
            set;
        }

        public static bool CalledUpdateOrDelete
        {
            get;
            set;
        }
        
        #endregion

        #region Public Methods

        public static void Clear()
        {
            SPListItemReturnedByGet = null;
            SPListItemCollectionReturnedByGet = null;
            CalledUpdateOrDelete = false;
        }

        #endregion

        #region ISPListManager Members

        public SPListItem Add(SPWeb web, string listName, Dictionary<Guid, object> fields)
        {
            return SPListItemReturnedByGet;
        }

        public void Delete(SPWeb web, string listName, int listItemId)
        {
            CalledUpdateOrDelete = true;
        }

        public SPListItem Get(SPWeb web, string listName, SPQuery query)
        {
            return SPListItemReturnedByGet;
        }

        public SPListItemCollection Get(SPWeb web, string listName)
        {
            return SPListItemCollectionReturnedByGet;
        }

        public void Update(SPWeb web, string listName, int listItemId, Dictionary<Guid, object> fields)
        {
            CalledUpdateOrDelete = true;
        }

        #endregion
    }
}
