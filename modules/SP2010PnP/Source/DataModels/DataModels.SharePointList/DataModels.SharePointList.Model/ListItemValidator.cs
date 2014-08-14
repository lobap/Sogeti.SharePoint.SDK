//===============================================================================
// Microsoft patterns & practices
// Developing Applications for SharePoint 2010
//===============================================================================
// Copyright Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://msdn.microsoft.com/en-us/library/ee663037.aspx)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;

namespace DataModels.SharePointList.Model
{
    using System.Security.Permissions;
    using Microsoft.SharePoint.Security;

    public class ListItemValidator
    {
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private SPQuery BuildQuery(SPList list, Dictionary<string, string> fields)
        {
            SPQuery query = new SPQuery();
            query.Query = BuildWhereClause(list, fields);
            return query;
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private string BuildWhereClause(SPList list, Dictionary<string, string> fields)
        {
            if (fields == null || fields.Count <= 0)
            {
                throw new ArgumentNullException("You must add at least one field in order to validate the Item.");
            }
            
            int numberOfAndClauses = fields.Count - 1;
            StringBuilder sb = new StringBuilder();
            sb.Append("<Where>");
            for (int i = 0; i < numberOfAndClauses; i++)
            {
                sb.Append("<And>");
            }

            int j = 0;
            foreach (KeyValuePair<string, string> kvp in fields)
            {
                SPField field = list.Fields.GetFieldByInternalName(kvp.Key);
                if (j == 0)
                {
                    sb.Append(GetFieldRef(kvp,field.Type));
                }
                else
                {
                    sb.Append(GetFieldRef(kvp, field.Type));
                    sb.Append("</And>");
                }
                j++; 
            }
            sb.Append("</Where>");
            return sb.ToString();
        }

        private string GetFieldRef(KeyValuePair<string, string> field, SPFieldType fieldType)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Eq>");
            sb.Append("<FieldRef Name='");
            sb.Append(field.Key);
            sb.Append(fieldType == SPFieldType.Lookup ? "' LookupId='TRUE' />" : "' />");
            sb.Append(fieldType == SPFieldType.Lookup ? "<Value Type='Lookup'>" : "<Value Type='Text'>"); 
            sb.Append(field.Value);
            sb.Append("</Value>");
            sb.Append("</Eq>");
            return sb.ToString();
        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public bool MatchingFieldsExistInList(SPList list, SPItemEventDataCollection data, IEnumerable<string> compositeKeyFields)
        {
            Dictionary<string, string> fields = GetFieldsAndValues(data, compositeKeyFields);
          
            
            SPListItemCollection listItems = list.GetItems(BuildQuery(list, fields));
            if (listItems != null && listItems.Count > 0)
            {
                return true;
            }
            else return false;

        }

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public Dictionary<string, string> GetFieldsAndValues(SPItemEventDataCollection data, IEnumerable<string> compositeKeyFields)
        {
            Dictionary<string, string> fieldsAndValues = new Dictionary<string, string>();

            foreach (string compositeKeyField in compositeKeyFields)
            {
                SPFieldLookupValue fieldLookupValue = new SPFieldLookupValue(data[compositeKeyField].ToString());
                fieldsAndValues.Add(compositeKeyField, fieldLookupValue.LookupId.ToString());
            }
            return fieldsAndValues;
        }
    }
}
