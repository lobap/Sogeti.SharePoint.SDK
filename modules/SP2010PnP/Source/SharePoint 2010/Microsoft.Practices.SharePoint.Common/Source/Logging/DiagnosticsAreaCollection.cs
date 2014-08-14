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
using System.Globalization;
using System.Linq;
using System.Security.Permissions;
using System.Xml.Serialization;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.Properties;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SharePoint.Common.Logging
{
    /// <summary>
    /// Areas collection.
    /// </summary>
    [XmlRoot(ElementName = "DiagnosticsAreas")]
    public class DiagnosticsAreaCollection : Collection<DiagnosticsArea>
    {
        IConfigManager configMgr = null;

        /// <summary>
        /// Constructs an empty diagnostics area collection.  The collection can not be persisted when constructed without the configuration manager provided. 
        /// A default constructor is also required by the deserialization process.
        /// </summary>
        public DiagnosticsAreaCollection()
        {
        }
        
        /// <summary>
        /// Constructs the diagnostics area collection
        /// </summary>
        /// <param name="configMgr">the instance of configuration manager to use when loading areas</param>
        public DiagnosticsAreaCollection(IConfigManager configMgr)
            : base(LoadAreas(configMgr))
        {
            this.configMgr = configMgr;
        }

        /// <summary>
        /// Loads the areas from farm configuration using the configuration manager provided.
        /// </summary>
        /// <param name="configMgr">The configuration manager to use to load the areas</param>
        /// <returns>the list containing areas loaded</returns>
        private static IList<DiagnosticsArea> LoadAreas(IConfigManager configMgr)
        {
            Validation.ArgumentNotNull(configMgr, "configName");
            IPropertyBag bag = configMgr.GetPropertyBag(ConfigLevel.CurrentSPFarm);

            if (configMgr.ContainsKeyInPropertyBag(Constants.AreasConfigKey, bag))
                return configMgr.GetFromPropertyBag<DiagnosticsAreaCollection>(Constants.AreasConfigKey, bag);
             else
                return new DiagnosticsAreaCollection();
         }

        /// <summary>
        /// Inserts a DiagnosticsArea into the collection, throws if a duplicate area already exists.
        /// </summary>
        /// <param name="index">The index of the area</param>
        /// <param name="item">The area to be added</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        protected override void InsertItem(int index, DiagnosticsArea item)
        {
            Validation.ArgumentNotNull(item, "newArea");
            Validation.ArgumentNotNullOrEmpty(item.Name, "newArea.Name");
            ValidateNotDefaultArea(item);

            if (Find(item.Name) != null)
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.DiagnosticsAreaCollectionAreaExists, item.Name));

            base.InsertItem(index, item);
        }

        /// <summary>
        /// Sets a new area in a list
        /// </summary>
        /// <param name="index">The index to set at</param>
        /// <param name="item">The new area to be set at the index</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        protected override void SetItem(int index, DiagnosticsArea item)
        {
            Validation.ArgumentNotNull(item, "item");
            Validation.ArgumentNotNullOrEmpty(item.Name, "item.Name");
            ValidateNotDefaultArea(item);

            DiagnosticsArea area = Find(item.Name);

            if (area != null)
            {
                int indexOfCat = IndexOf(area);
                if (indexOfCat != index)
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.DiagnosticsCategoryExists, item.Name));
            }

            base.SetItem(index, item);
        }

        private void ValidateNotDefaultArea(DiagnosticsArea area)
        {
            if (area.Name == Constants.DefaultAreaName)
            {
                throw new InvalidOperationException(Resources.AddedDefaultDiagnosticsAreaToCollection);
            }
        }
        

        /// <summary>
        /// string indexer for collection, looks up based on area name.  Throws if area doesn't exist.
        /// </summary>
        /// <param name="areaName">The area name to index</param>
        /// <returns>the DiagnosticsArea at that index</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public DiagnosticsArea this[string areaName]
        {
            get
            {
                Validation.ArgumentNotNullOrEmpty(areaName, "areaName");
                DiagnosticsArea area = this.Find(areaName);
                return area;
            }
            set
            {
                Validation.ArgumentNotNull(areaName, "areaName");
                Validation.ArgumentNotNull(value, "value"); 
                Validation.ArgumentNotNull(value.Name, "value.Name"); 

               DiagnosticsArea area = this.Find(areaName);

                if(area == null)
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.StringIndexOutOfRange, "DiagnosticsAreaCollection", areaName));

                SetItem(IndexOf(area), value);
            }
        }

         private DiagnosticsArea Find(string name)
        {
            Validation.ArgumentNotNullOrEmpty(name, "name");

            return this.FirstOrDefault<DiagnosticsArea>(delegate(DiagnosticsArea area)
            {
                return name.Equals(area.Name, StringComparison.OrdinalIgnoreCase);
            });
        }


        /// <summary>
        /// Persists the changes to the configuration store.
        /// </summary>
        public void SaveConfiguration()
        {
            if (this.configMgr == null)
                throw new InvalidOperationException(Resources.DiagnosticsAreaCollectionSaveWithNoConfig);

            IPropertyBag bag = this.configMgr.GetPropertyBag(ConfigLevel.CurrentSPFarm);
            this.configMgr.SetInPropertyBag(Constants.AreasConfigKey, this, bag);
            DiagnosticsService.Unregister();
            var svc = DiagnosticsService.Local; // force reregister.
        }
    }
}
