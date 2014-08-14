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
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using System.Globalization;
using Microsoft.Practices.SharePoint.Common.Properties;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;

namespace Microsoft.Practices.SharePoint.Common.Logging
{
    /// <summary>
    /// Category collection.
    /// </summary>
    [XmlRoot(ElementName = "DiagnosticsCategories")]
    public class DiagnosticsCategoryCollection : Collection<DiagnosticsCategory>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsCategoryCollection"/> class.
        /// </summary>
        public DiagnosticsCategoryCollection() : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsCategoryCollection"/> class.
        /// </summary>
        /// <param name="categories">The categories to set</param>
        public DiagnosticsCategoryCollection(IList<DiagnosticsCategory> categories) : base(categories)
        { }


        /// <summary>
        /// Inserts a category into the collection.  Duplicate categories are not permitted.
        /// </summary>
        /// <param name="index">the index to insert at</param>
        /// <param name="item">the category to insert</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        protected override void InsertItem(int index, DiagnosticsCategory item)
        {
            Validation.ArgumentNotNull(item, "item");
            Validation.ArgumentNotNull(item.Name, "category.Name");

            if (Find(item.Name) != null)
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.DiagnosticsCategoryExists, item.Name));

            base.InsertItem(index, item);
        }

        /// <summary>
        /// Sets the category at the index.  Duplicate categories are not permitted.
        /// </summary>
        /// <param name="index">The index to set at</param>
        /// <param name="item">The item to set</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        protected override void SetItem(int index, DiagnosticsCategory item)
        {
            Validation.ArgumentNotNull(item, "item");
            Validation.ArgumentNotNull(item.Name, "item.Name");
            DiagnosticsCategory foundCategory = Find(item.Name);

            if (foundCategory != null)
            {
                int indexOfCat = IndexOf(foundCategory);
                if (indexOfCat != index)
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.DiagnosticsCategoryExists, item.Name));
            }

            base.SetItem(index, item);
        }

        /// <summary>
        /// string indexer for diagnostics category collection taking category name.
        /// </summary>
        /// <param name="categoryName">The name of the category in the index</param>
        /// <returns>The category if found, throws if no category found</returns>

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public DiagnosticsCategory this[string categoryName]
        {
            get
            {
                Validation.ArgumentNotNull(categoryName, "categoryName");
                DiagnosticsCategory category = this.Find(categoryName);
                return category;
            }
            set
            {
                Validation.ArgumentNotNull(categoryName, "areaName");
                Validation.ArgumentNotNull(value, "value"); 
                Validation.ArgumentNotNull(value.Name, "value.Name");


                DiagnosticsCategory category = this.Find(categoryName);

                if(category == null)
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.StringIndexOutOfRange, "DiagnosticsAreaCollection", categoryName));

                SetItem(IndexOf(category), value );
            }
        }

        private DiagnosticsCategory Find(string name)
        {
            return this.FirstOrDefault<DiagnosticsCategory>(delegate(DiagnosticsCategory category)
            {
                return name.Equals(category.Name, StringComparison.OrdinalIgnoreCase);
            });
        }
    }
}
