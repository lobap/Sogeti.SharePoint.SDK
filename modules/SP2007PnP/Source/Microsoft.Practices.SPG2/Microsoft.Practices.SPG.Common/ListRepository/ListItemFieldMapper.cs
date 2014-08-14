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
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using System.Collections.ObjectModel;

namespace Microsoft.Practices.SPG.Common.ListRepository
{
    /// <summary>
    /// Class that maps Fields from a <see cref="SPListItem"/> to properties on a strong typed business entity
    /// </summary>
    /// <typeparam name="TEntity">The type of businessentity to map. </typeparam>
    [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
    public class ListItemFieldMapper<TEntity> where TEntity : new()
    {
        Collection<FieldToEntityPropertyMapping> fieldMappings = new Collection<FieldToEntityPropertyMapping>();

        /// <summary>
        /// Create an entity, and fill the mapped properties from the specified <see cref="SPListItem"/>.
        /// </summary>
        /// <param name="item">The listitem to use to fill the entities properties. </param>
        /// <returns>The created and populated entity. </returns>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public TEntity CreateEntity(SPListItem item)
        {
            TEntity entity = new TEntity();
            Type entityType = typeof (TEntity);

            foreach (FieldToEntityPropertyMapping fieldmapping in fieldMappings)
            {
                PropertyInfo propertyInfo = GetPropertyInfo(item, entityType, fieldmapping);
                EnsureListFieldID(item, entityType, fieldmapping);
                propertyInfo.SetValue(entity, item[fieldmapping.ListFieldId], null);
            }
            return entity;

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "ensuredField"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void EnsureListFieldID(SPListItem item, Type entityType, FieldToEntityPropertyMapping fieldMapping)
        {
            try
            {
                var ensuredField = item.Fields[fieldMapping.ListFieldId];
            }
            catch (ArgumentException argumentException)
            {
                string errorMessage = string.Format(CultureInfo.CurrentCulture
                                        , "SPListItem '{0}' does not have a field with Id '{1}' which was mapped to property: '{2}' for entity '{3}'."
                                        , item.Name
                                        , fieldMapping.ListFieldId
                                        , fieldMapping.EntityPropertyName
                                        , entityType.FullName);

                throw new ListItemFieldMappingException(errorMessage, argumentException);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private PropertyInfo GetPropertyInfo(SPListItem item, Type entityType, FieldToEntityPropertyMapping fieldMapping)
        {
            PropertyInfo propertyInfo = entityType.GetProperty(fieldMapping.EntityPropertyName);
            if (propertyInfo == null)
            {
                string errorMessage = string.Format(CultureInfo.CurrentCulture, "Type '{0}' does not have a property '{1}' which was mapped to FieldID: '{2}' for SPListItem '{3}'."
                                                    , entityType.FullName
                                                    , fieldMapping.EntityPropertyName
                                                    , fieldMapping.ListFieldId
                                                    , item.Name);
                throw new ListItemFieldMappingException(errorMessage);
            }
            return propertyInfo;
        }

        /// <summary>
        /// The list of field mappings that are used by the ListItemFieldMapper class. 
        /// </summary>
        public Collection<FieldToEntityPropertyMapping> FieldMappings
        {
            get { return fieldMappings; }
        }

        /// <summary>
        /// Fill a SPListItem's properties based on the values in an entity.  
        /// </summary>
        /// <param name="spListItem"></param>
        /// <param name="entity"></param>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public void FillSPListItemFromEntity(SPListItem spListItem, TEntity entity)
        {
            Type entityType = typeof(TEntity);
            foreach (FieldToEntityPropertyMapping fieldmapping in fieldMappings)
            {
                PropertyInfo propertyInfo = GetPropertyInfo(spListItem, entityType, fieldmapping);
                EnsureListFieldID(spListItem, entityType, fieldmapping);
                spListItem[fieldmapping.ListFieldId] = propertyInfo.GetValue(entity, null);
            }
        }

        /// <summary>
        /// Add a mapping between a field in an SPListItem and a property in the entity. 
        /// </summary>
        /// <param name="fieldId"></param>
        /// <param name="entityPropertyName"></param>
        public void AddMapping(Guid fieldId, string entityPropertyName)
        {
            fieldMappings.Add(new FieldToEntityPropertyMapping { EntityPropertyName = entityPropertyName, ListFieldId = fieldId });
        }
    }
}
