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
using System.Security.Permissions;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;

namespace Microsoft.Practices.SPG.Common.ListRepository
{
    /// <summary>
    /// Class that helps in building CAML Queries. 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "CAML")]
    public class CAMLQueryBuilder
    {
        private List<CAMLFilter> filters = new List<CAMLFilter>();

        /// <summary>
        /// Build a CAML Query, based on the filter expressions that have been added to it. 
        /// </summary>
        /// <returns></returns>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public SPQuery Build()
        {
            SPQuery spQuery = new SPQuery();

            StringBuilder queryBuilder = new StringBuilder("<Where>");

            if (filters.Count > 1)
            {
                queryBuilder.Append("<And>");
            }

            foreach (CAMLFilter filter in this.filters)
            {
                queryBuilder.Append(filter.FilterExpression);
            }

            if (filters.Count > 1)
            {
                queryBuilder.Append("</And>");
            }

            queryBuilder.Append("</Where>");

            spQuery.Query = queryBuilder.ToString();
            return spQuery;
        }

        /// <summary>
        /// Add a filter expression to the CAML Query builder
        /// </summary>
        /// <param name="filter">The filter expression to add. </param>
        public void AddFilter(CAMLFilter filter)
        {
            this.filters.Add(filter);
        }

        /// <summary>
        /// Add an 'EQ' (equals) expression to the CAML Query builder where you want to filter the field 
        /// based on the value. You can specify the CAML type to determine what the type is you are filtering on.
        /// </summary>
        /// <param name="name">The name of the field to filter. </param>
        /// <param name="value">The value to filter the field by.</param>
        /// <param name="camlType">The type of the expression in CAML</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "caml")]
        public void AddEqual(string name, object value, string camlType)
        {
            string filterExpression = string.Format(CultureInfo.InvariantCulture,
                                        "<Eq><FieldRef Name='{0}'/><Value Type='{1}'>{2}</Value></Eq>"
                                        , name, camlType, value);

            this.AddFilter(new CAMLFilter { FilterExpression = filterExpression });

        }

        /// <summary>
        /// Add an 'EQ' (equals) expression to the CAML Query builder where you want to filter the field 
        /// based on the value. The type of the filter expression is 'Integer'.
        /// </summary>
        /// <param name="name">The fieldname to filter by. </param>
        /// <param name="value">The value to filter on. </param>
        public void AddEqual(string name, int value)
        {
            AddEqual(name, value, "Integer");
        }

        /// <summary>
        /// Add an 'EQ' (equals) expression to the CAML Query builder where you want to filter the field 
        /// based on the value. The type of the filter expression is 'DateTime' and the value will be formatted
        /// using <see cref="SPUtility.CreateDateTimeFromISO8601DateTimeString"/>. 
        /// </summary>
        /// <param name="name">The fieldname to filter by. </param>
        /// <param name="value">The value to filter on. </param>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public void AddEqual(string name, DateTime value)
        {
            AddEqual(name, SPUtility.CreateISO8601DateTimeFromSystemDateTime(value), "DateTime");
        }

        /// <summary>
        /// Add an 'EQ' (equals) expression to the CAML Query builder where you want to filter the field 
        /// based on the value. The type of the filter expression is 'Text'.
        /// </summary>
        /// <param name="name">The fieldname to filter by. </param>
        /// <param name="value">The value to filter on. </param>
        public void AddEqual(string name, string value)
        {
            AddEqual(name, value, "Text");
        }

        /// <summary>
        /// Add a filter expression to filter by content type. 
        /// </summary>
        /// <param name="contentTypeName">The contenttype to filter by.</param>
        public void FilterByContentType(string contentTypeName)
        {
            string filterExpression = string.Format(CultureInfo.InvariantCulture,
                           "<Eq><FieldRef Name='ContentType'/><Value Type='Text'>{0}</Value></Eq>",
                           contentTypeName);

            this.AddFilter(new CAMLFilter { FilterExpression = filterExpression });
        }

        /// <summary>
        /// Add an 'EQ' (equals) expression to the CAML Query builder where you want to filter the field 
        /// based on the value. You can specify the CAML type to determine what the type is you are filtering on.
        /// </summary>
        /// <param name="fieldId">The fieldId to filter by. </param>
        /// <param name="value">The value to filter on. </param>
        /// <param name="camlType">The type of the column to filter on. </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "caml")]
        public void AddEqual(Guid fieldId, object value, string camlType)
        {
            string filterExpression = string.Format(CultureInfo.InvariantCulture,
                                        "<Eq><FieldRef ID='{0}'/><Value Type='{1}'>{2}</Value></Eq>"
                                        , fieldId.ToString(), camlType, value);

            this.AddFilter(new CAMLFilter { FilterExpression = filterExpression });
        }

        /// <summary>
        /// Add an 'EQ' (equals) expression to the CAML Query builder where you want to filter the field 
        /// based on the value. The type of the filter expression is 'Text'.
        /// </summary>
        /// <param name="fieldId">The fieldId to filter by. </param>
        /// <param name="value">The value to filter on. </param>
        public void AddEqual(Guid fieldId, string value)
        {
            this.AddEqual(fieldId, value, "Text");
        }

        /// <summary>
        /// Add an 'EQ' (equals) expression to the CAML Query builder where you want to filter the field 
        /// based on the value. The type of the filter expression is 'Integer'.
        /// </summary>
        /// <param name="fieldId">The fieldId to filter by. </param>
        /// <param name="value">The value to filter on. </param>
        public void AddEqual(Guid fieldId, int value)
        {
            this.AddEqual(fieldId, value, "Integer");
        }

        /// <summary>
        /// Add an 'EQ' (equals) expression to the CAML Query builder where you want to filter the field 
        /// based on the value. The type of the filter expression is 'DateTime' and the value will be formatted
        /// using <see cref="SPUtility.CreateDateTimeFromISO8601DateTimeString"/>. 
        /// </summary>
        /// <param name="fieldId">The fieldId to filter by. </param>
        /// <param name="value">The value to filter on. </param>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void AddEqual(Guid fieldId, DateTime value)
        {
            this.AddEqual(fieldId, SPUtility.CreateISO8601DateTimeFromSystemDateTime(value), "DateTime");
        }

        /// <summary>
        /// Add an 'Neq' (not equals) expression to the CAML Query builder where you want to filter the field 
        /// based on the value. You can specify the CAML type to determine what the type is you are filtering on.
        /// </summary>
        /// <param name="name">The name of the field to filter. </param>
        /// <param name="value">The value to filter the field by.</param>
        /// <param name="camlType">The type of the expression in CAML</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "caml")]
        public void AddNotEqual(string name, object value, string camlType)
        {
            string filterExpression = string.Format(CultureInfo.InvariantCulture,
                "<Neq><FieldRef Name='{0}'/><Value Type='{1}'>{2}</Value></Neq>"
                                        , name, camlType, value);

            this.AddFilter(new CAMLFilter { FilterExpression = filterExpression });
        }

        /// <summary>
        /// Add an 'Neq' (not equals) expression to the CAML Query builder where you want to filter the field 
        /// based on the value. This overload will use the CAML type "Text" to create a filter.
        /// </summary>
        /// <param name="name">The name of the field to filter. </param>
        /// <param name="value">The value to filter the field by.</param>
        public void AddNotEqual(string name, string value)
        {
            this.AddNotEqual(name, value, "Text");
        }

        /// <summary>
        /// Add an 'Neq' (not equals) expression to the CAML Query builder where you want to filter the field 
        /// based on the value. This overload will use the CAML type "Integer" to create a filter.
        /// </summary>
        /// <param name="name">The name of the field to filter. </param>
        /// <param name="value">The value to filter the field by.</param>
        public void AddNotEqual(string name, int value)
        {
            this.AddNotEqual(name, value, "Integer");
        }

        /// <summary>
        /// Add an 'Neq' (not equals) expression to the CAML Query builder where you want to filter the field 
        /// based on the value. This overload will use the CAML type "DateTime" to create a filter.
        /// </summary>
        /// <param name="name">The name of the field to filter. </param>
        /// <param name="value">The value to filter the field by.</param>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public void AddNotEqual(string name, DateTime value)
        {
            this.AddNotEqual(name, SPUtility.CreateISO8601DateTimeFromSystemDateTime(value), "DateTime");
        }
    }
}
