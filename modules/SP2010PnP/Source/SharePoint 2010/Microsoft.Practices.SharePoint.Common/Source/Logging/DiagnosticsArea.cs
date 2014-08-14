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
using System.Security.Permissions;
using Microsoft.Practices.SharePoint.Common.Properties;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;
using Microsoft.Practices.SharePoint.Common.Configuration;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Microsoft.Practices.SharePoint.Common.Logging
{
    /// <summary>
    /// Class representing an area for diagnostic purpose.
    /// </summary>
    public class DiagnosticsArea
    {
        private string name;
        private DiagnosticsCategoryCollection diagnosticsCategories = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsArea"/> class.
        /// </summary>
        public DiagnosticsArea()
        {
            this.diagnosticsCategories = new DiagnosticsCategoryCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsArea"/> class.
        /// </summary>
        /// <param name="name">The name of the diagnostic area.</param>
        public DiagnosticsArea(string name) : this()
        {
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsArea"/> class.
        /// </summary>
        /// <param name="name">The name of the diagnostic area.</param>
        /// <param name="diagnosticsCategories">A collection of <see cref="DiagnosticsCategory"/> that will be part of the area.</param>
        public DiagnosticsArea(string name, DiagnosticsCategoryCollection diagnosticsCategories)
        {
            this.Name = name;

            if (diagnosticsCategories == null)
                this.diagnosticsCategories = new DiagnosticsCategoryCollection();
            else
                this.diagnosticsCategories = new DiagnosticsCategoryCollection(diagnosticsCategories);
        }

        /// <summary>
        /// The name of the diagnostic area
        /// </summary>
        public string Name
        {
            get { return name; }

             set
            {
                Validation.ArgumentNotNullOrEmpty(value, "Name");
                name = value;
            }
        }
        
        /// <summary>
        /// Gets the collection of <see cref="DiagnosticsCategory"/>.
        /// </summary>
        public DiagnosticsCategoryCollection DiagnosticsCategories
        {
            get
            {
                if (diagnosticsCategories == null)
                    diagnosticsCategories = new DiagnosticsCategoryCollection();

                return diagnosticsCategories;
            }
        }

        /// <summary>
        /// Converts from <see cref="DiagnosticsArea"/> to <see cref="SPDiagnosticsArea"/> instance.
        /// </summary>
        /// <returns>the SPDiagnosticsArea created from this diagnostics area</returns>
        public SPDiagnosticsArea ToSPDiagnosticsArea()
        {
            var categories = new List<SPDiagnosticsCategory>();
            foreach (DiagnosticsCategory category in this.DiagnosticsCategories)
                categories.Add(category.ToSPDiagnosticsCategory());

            return new SPDiagnosticsArea(this.Name, categories);
        }

        /// <summary>
        /// Gets the default <see cref="SPDiagnosticsArea"/> instance.
        /// </summary>
        public static SPDiagnosticsArea DefaultSPDiagnosticsArea
        {
            get
            {
                var spDefaultCategories = new List<SPDiagnosticsCategory>();
                spDefaultCategories.Add(DiagnosticsCategory.DefaultSPDiagnosticsCategory);
                return new SPDiagnosticsArea(Constants.DefaultAreaName, spDefaultCategories);
            }
        }
     }
 }
