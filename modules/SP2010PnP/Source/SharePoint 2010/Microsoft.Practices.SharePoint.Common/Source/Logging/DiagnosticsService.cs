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
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.Properties;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SharePoint.Common.Logging
{
    /// <summary>
    /// Provides a diagnostic logging for Windows SharePoint Services.
    /// </summary>
    [Guid("1BA36583-1EDB-4AB6-92C5-ACF18FB742AA")] 
    public class DiagnosticsService : SPDiagnosticsServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the DiagnosticService class.
        /// </summary>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public DiagnosticsService()
        {
        }

        /// <summary>
        /// /// Initializes a new instance of the DiagnosticService class.
        /// </summary>
        /// <param name="name">Gets or Sets the name that identifies the particular instance of the object.</param>
        /// <param name="parent">Gets the ID of the parent class that declares de object</param>

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public DiagnosticsService(string name, SPFarm parent)
            : base(name, parent)
        {
        }

        /// <summary>
        /// Gets the local instance of the class and registers it.
        /// </summary>
     
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
     
        public static DiagnosticsService Local
        {
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                return SPDiagnosticsServiceBase.GetLocal<DiagnosticsService>();
            }
        }

        /// <summary>
        /// Registers the class.  
        /// </summary>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
         public static void Register()
        {
            DiagnosticsService diagnositicService = new DiagnosticsService(String.Empty, SPFarm.Local);
            diagnositicService.Update(true);
        }

        /// <summary>
        /// Unregisters the class. 
        /// </summary>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public static void Unregister()
        {
            DiagnosticsService diagnosticsService = DiagnosticsService.Local;
            if (diagnosticsService != null)
            {
                diagnosticsService.Delete();
            }
        }

        /// <summary>
        /// Finds the desired SharePoint diagnostic category based on the name passed in.
        /// </summary>
        /// <param name="categoryPath">>The name of the diagnostic category.</param>
        /// <returns>The sharepoint diagnostics category found.</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public virtual SPDiagnosticsCategory FindCategory(string categoryPath)
        {
            Validation.ArgumentNotNullOrEmpty(categoryPath, "categoryPath");

            string[] categoryPathElements = ParseCategoryPath(categoryPath);
            string areaName = categoryPathElements[0];
            string categoryName = categoryPathElements[1];

            SPDiagnosticsCollection<SPDiagnosticsArea> areas = this.Areas;

            if (areas == null)
                return DefaultCategory;

            foreach (SPDiagnosticsArea area in areas)
            {
                if (areaName.Trim().Equals(area.Name.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    SPDiagnosticsCategory foundCategory = area.Categories.FirstOrDefault<SPDiagnosticsCategory>(delegate(SPDiagnosticsCategory category)
                    {
                        return categoryName.Equals(category.Name, StringComparison.OrdinalIgnoreCase);
                    });

                    return foundCategory;
                }
            }

            return null;
        }

        /// <summary>
        /// Provides the collection of diagnostic areas getting them from the configuration.
        /// </summary>
        /// <returns>An Enumerable collection of diagnostic areas.</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        protected override IEnumerable<SPDiagnosticsArea> ProvideAreas()
        {
            IConfigManager config = SharePointServiceLocator.GetCurrent().GetInstance<IConfigManager>();

            IEnumerable<DiagnosticsArea> diagnosticsAreas = new DiagnosticsAreaCollection(config);

            List<SPDiagnosticsArea> spAreas = new List<SPDiagnosticsArea>();

            foreach (DiagnosticsArea area in diagnosticsAreas)
            {
                List<SPDiagnosticsCategory> spCategories = new List<SPDiagnosticsCategory>();
                foreach (DiagnosticsCategory category in area.DiagnosticsCategories)
                {
                    spCategories.Add(category.ToSPDiagnosticsCategory());
                }
                SPDiagnosticsArea spArea = new SPDiagnosticsArea(area.Name, spCategories);
                spAreas.Add(spArea);
            }

            spAreas.Add(DiagnosticsArea.DefaultSPDiagnosticsArea);

            return spAreas;
        }

        /// <summary>
        /// Returns the default diagnostic category.
        /// </summary>
        /// <returns>A default instance of SPDiagnosticsCategory.</returns>
    
        public virtual SPDiagnosticsCategory DefaultCategory
        {
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                return this.Areas[Constants.DefaultAreaName].Categories[Constants.DefaultCategoryName];
            }
        }

        /// <summary>
        /// Logs an event to the event log using default severity for category.
        /// </summary>
        /// <param name="message">the message to log</param>
        /// <param name="eventId">the id of the event</param>
        /// <param name="category">the cateogry for the event</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public virtual void LogEvent(string message, int eventId, string category)
        {
            Validation.ArgumentNotNullOrEmpty(message, "message");
            SPDiagnosticsCategory spCategory = GetCategory(category);
            LogEvent(message, eventId, spCategory.DefaultEventSeverity, spCategory);
        }

        /// <summary>
        /// Logs an event to the event log.
        /// </summary>
        /// <param name="message">the message to log</param>
        /// <param name="eventId">the id of the event</param>
        /// <param name="severity">the severity of the event</param>
        /// <param name="categoryName">the cateogry for the event</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public virtual void LogEvent(string message, int eventId, EventSeverity severity, string categoryName)
        {
            Validation.ArgumentNotNullOrEmpty(message, "message");

            SPDiagnosticsCategory category = GetCategory(categoryName);
            LogEvent(message, eventId, severity, category);
        }

     
        private void LogEvent(string message, int eventId, EventSeverity severity, SPDiagnosticsCategory spCategory)
        {
            string formattedMessage = string.Format(CultureInfo.CurrentCulture, "Category: {0}: {1}", spCategory.Name, message);
            base.WriteEvent((ushort)eventId, spCategory, severity, formattedMessage, null);
        }


        /// <summary>
        /// Logs an trace to the ULS log.
        /// </summary>
        /// <param name="message">the message to log</param>
        /// <param name="eventId">the id of the trace</param>
        /// <param name="severity">the severity of the trace</param>
        /// <param name="categoryName">the category for the trace</param>       
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public virtual void LogTrace(string message, int eventId, TraceSeverity severity, string categoryName)
        {
            Validation.ArgumentNotNullOrEmpty(message, "message");

            SPDiagnosticsCategory category = GetCategory(categoryName);
            base.WriteTrace((uint)eventId, category, severity, message, null);
        }

        /// <summary>
        /// Logs an trace to the ULS log.
        /// </summary>
        /// <param name="message">the message to log</param>
        /// <param name="eventId">the id of the trace</param>
        /// <param name="category">the category for the trace</param>       
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public virtual void LogTrace(string message, int eventId, string category)
        {
            SPDiagnosticsCategory spCategory = GetCategory(category);
            base.WriteTrace((uint)eventId, spCategory, spCategory.DefaultTraceSeverity, message, null);
        }

        #region Private Members

        /// <summary>
        /// Gets the category for the provided category name.  Throws a logging exception if the 
        /// category is not found.  Returns default category for null or empty category name.
        /// </summary>
        /// <param name="categoryName">The category to find</param>
        /// <returns>the SPCategory for the category name provided</returns>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        private SPDiagnosticsCategory GetCategory(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
                return DefaultCategory;
            else
            {
                SPDiagnosticsCategory foundCategory = FindCategory(categoryName);
                if (foundCategory == null)
                    throw new LoggingException(string.Format(CultureInfo.CurrentCulture, Resources.CategoryNotFoundExceptionMessage, categoryName));
                return foundCategory;
            }
        }

        private static string[] ParseCategoryPath(string categoryPath)
        {
            if (categoryPath.IndexOf(Constants.CategoryPathSeparator) <= 0)
            {
                throw new LoggingException(Resources.CategoryNotFoundExceptionMessage);
            }

            string[] categoryPathElements = categoryPath.Split(new char[] { Constants.CategoryPathSeparator }, StringSplitOptions.RemoveEmptyEntries);

            if (categoryPathElements.Length != 2)
            {
                throw new LoggingException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidCategoryFormat, categoryPath));
            }

            return categoryPathElements;
        }

        #endregion
    }
}
