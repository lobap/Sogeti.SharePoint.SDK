//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SharePoint;
using System.Text;
namespace DataModels.SharePointList.Model.ListEventReceivers
{
    using System.Security.Permissions;
    using Microsoft.SharePoint.Security;

    /// <summary>
    /// List Item Events
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    public class UniqueListItemEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// An item is being added.
        /// </summary>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void ItemAdding(SPItemEventProperties properties)
        {
            try
            {
                string[] keyFields = properties.ReceiverData.Split(';');
                ValidateUniqueCompositeKey(properties, keyFields);
            }
            catch (Exception ex)
            {
                properties.ErrorMessage = ex.Message;
                properties.Cancel = true;
            }
        }

        /// <summary>
        /// An item is being updated.
        /// </summary>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            try
            {
                string[] keyFields = properties.ReceiverData.Split(';');
                if (CompositeKeyValuesHaveChanged(properties, keyFields))
                {
                    ValidateUniqueCompositeKey(properties, keyFields);
                }
            }
            catch (Exception ex)
            {
                properties.ErrorMessage = ex.Message;
                properties.Cancel = true;
            }

        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private bool CompositeKeyValuesHaveChanged(SPItemEventProperties properties, IEnumerable<string> compositeKeyFields)
        {
            foreach (string compositeKeyField in compositeKeyFields)
            {
                //Need to get original value from properties.ListItem as BeforeProperties is not populated for list items.
                int originalValue = GetOriginalLookupValue(properties.ListItem, compositeKeyField);
                int newValue = GetNewLookupValue(properties.AfterProperties, compositeKeyField);

                if (originalValue != newValue)
                {
                    return true;
                }
            }
            return false;
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private int GetOriginalLookupValue(SPListItem listItem, string compositeKeyField)
        {
            string[] fieldValues = listItem[compositeKeyField].ToString().Split(';');
            return int.Parse(fieldValues[0]);
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private int GetNewLookupValue(SPItemEventDataCollection dataCollection, string compositeKeyField)
        {
            SPFieldLookupValue fieldLookupValue = new SPFieldLookupValue(dataCollection[compositeKeyField].ToString());
            return fieldLookupValue.LookupId;
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private void ValidateUniqueCompositeKey(SPItemEventProperties properties, string[] compositeKeyFields)
        {
            // If the receiver isn't created with the data we need exit quickly (for performance reasons)
            if (string.IsNullOrEmpty(properties.ReceiverData)) return;


            SPItemEventDataCollection aftProps = properties.AfterProperties;
            ListItemValidator validator = new ListItemValidator();

            bool result = validator.MatchingFieldsExistInList(properties.List, aftProps, compositeKeyFields);
            if (result)
            {
                StringBuilder errorMessage = new StringBuilder();
                errorMessage.Append("Duplicate entry.  The same composite key exists with the Field=Value: ");
                for (int looper = 0; looper < compositeKeyFields.Length; looper++)
                {
                    string key = compositeKeyFields[looper];
                    errorMessage.Append(key).Append('=').Append(aftProps[key]);
                    if (looper + 1 != compositeKeyFields.Length) errorMessage.Append(", ");
                }
                properties.ErrorMessage = errorMessage.ToString();
                properties.Cancel = true;
            }

        }
    }
}
