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
namespace Contoso.TrainingManagement.Repository.BusinessEntities
{
    /// <summary>
    /// BaseEntity is the base class for all custom entity class
    /// wrappers for SharePoint list items. It is mainly used to
    /// abstract the SharePoint classes used in custom code.
    /// </summary>
    public abstract class BaseEntity
    {
        #region Public Properties

        /// <summary>
        /// Represents the SharePoint list item's Id field
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the SharePoint list item's Title field.
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        #endregion
    }
}
