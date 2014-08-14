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

using Contoso.TrainingManagement.Common.Constants;
using Contoso.TrainingManagement.Repository.BusinessEntities;

namespace Contoso.TrainingManagement.Repository
{
    /// <summary>
    /// The BaseEntityRepositroy is the base class for all repository classes
    /// used for accessing lists in SharePoint. The base class is used to enforce
    /// the repository pattern for accessing lists in custom code. It abstracts
    /// the SharePoint calls from custom code.
    /// </summary>
    /// <typeparam name="T">A business entity type that is inherited from BaseEntity</typeparam>
    public abstract class BaseEntityRepository<T> where T : BaseEntity, new()
    {
        #region Private Fields

        private readonly IListItemRepository listItemRepository;

        #endregion
	
        #region Public Properties

        /// <summary>
        /// Derived classes must implement this property to
        /// indicate which list the repository is targeting.
        /// </summary>
        protected abstract string ListName
        {
            get;
        }

        #endregion

        #region Constructor

        protected BaseEntityRepository()
        {
            this.listItemRepository = ServiceLocator.GetInstance().Get<IListItemRepository>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts entity T to an SPListItem and adds it to the specified list
        /// </summary>
        /// <param name="entity">Business entity inherited from BaseEntity to add to the list</param>
        /// <param name="web">The SPWeb where the list is located</param>
        /// <returns>ID of the new SPListItem in the list</returns>
        protected int AddListItem(T entity, SPWeb web)
        {
            Dictionary<Guid, object> fields = GatherParameters(entity, web);

            SPListItem item = null;

            item = listItemRepository.Add(web, this.ListName, fields);

            return (int)item[new Guid(Fields.Id)];
        }

        /// <summary>
        /// Returns an entity T given an CAML query
        /// </summary>
        /// <param name="caml">CAML used to return a single entity</param>
        /// <param name="web">The SPWeb where the list is located</param>
        /// <returns>Business entity inherited from BaseEntity from the list</returns>
        protected T GetListItem(string caml, SPWeb web)
        {
            T entity = null;

            SPQuery query = new SPQuery();
            query.Query = caml;

            SPListItem item = null;            

            item = listItemRepository.Get(web, this.ListName, query);            

            if (item != null)
            {
                entity = PopulateEntity(item);
            }

            return entity;
        }

        /// <summary>
        /// Returns a list of all entities T in a list
        /// </summary>
        /// <param name="web">The SPWeb where the list is located</param>
        /// <returns>List of entities T</returns>
        protected IList<T> GetListItemList(SPWeb web)
        {
            IList<T> entities = new List<T>();

            SPListItemCollection listItems = null;

            listItems = listItemRepository.Get(web, this.ListName);
                
            for ( int i = 0; i < listItems.Count; i++ )
            {
                T entity = PopulateEntity(listItems[i]);
                entities.Add(entity);
            }

            return entities;
        }

        /// <summary>
        /// Updates an entity T to a list based on the Id of the specified entity
        /// </summary>
        /// <param name="entity">Entity T to update in the list</param>
        /// <param name="web">The SPWeb where the list is located</param>
        protected void UpdateListItem(T entity, SPWeb web)
        {
            Dictionary<Guid, object> fields = GatherParameters(entity, web);

            listItemRepository.Update(web, this.ListName, entity.Id, fields);
        }

        /// <summary>
        /// Deletes an entity T from a list based on the specified Id
        /// </summary>
        /// <param name="id">The Id of the SPListItem to delete</param>
        /// <param name="web">The SPWeb where the list is located</param>
        protected void DeleteListItem(int id, SPWeb web)
        {
            listItemRepository.Delete(web, this.ListName, id);            
        }

        /// <summary>
        /// Gets the internal name of the field, given the field key, from the list.
        /// </summary>
        /// <param name="key">The field Id</param>
        /// <param name="web">The SPWeb where the list is located</param>
        /// <returns>Internal field name</returns>
        public string GetFieldName(Guid key, SPWeb web)
        {
            return web.Lists[this.ListName].Fields[key].InternalName;
        }

        #region Abstract

        /// <summary>
        /// Designed to convert an entity T into a generic dictionary of the fieldIds and field values
        /// </summary>
        protected abstract Dictionary<Guid, object> GatherParameters(T entity, SPWeb web);

        /// <summary>
        /// Designed to convert an SPListItem into an entity T
        /// </summary>
        protected abstract T PopulateEntity(SPListItem item);

        #endregion

        #endregion
    }
}
