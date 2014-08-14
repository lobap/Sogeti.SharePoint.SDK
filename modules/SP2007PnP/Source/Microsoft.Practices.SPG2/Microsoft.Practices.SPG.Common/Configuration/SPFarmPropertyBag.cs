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
using System.Security.Permissions;
using Microsoft.Practices.SPG.Common.ServiceLocation;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SPG.Common.Configuration
{
    /// <summary>
    /// Class that gives access to the PropertyBag in the SPFarm.
    /// </summary>
    [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
    public class SPFarmPropertyBag : IPropertyBag
    {
        private readonly SPFarm farm;

        /// <summary>
        /// Initializes a new instance of the <see cref="SPFarmPropertyBag"/> class.
        /// </summary>
        /// <param name="farm">The farm.</param>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        internal SPFarmPropertyBag(SPFarm farm)
        {
            if (farm == null) throw new ArgumentNullException("farm");
            this.farm = farm;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SPFarmPropertyBag"/> class.
        /// </summary>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public SPFarmPropertyBag()
        {
            if (SPFarm.Local == null)
            {
                throw new NoSharePointContextException("SPFarm was not found.");
            }
            this.farm = SPFarm.Local;
        }

        /// <summary>
        /// Does a specific key exist in the PropertyBag.
        /// </summary>
        /// <param name="key">the key to check.</param>
        /// <returns>true if the key exists, else false.</returns>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public bool Contains(string key)
        {
            return farm.Properties.ContainsKey(key);
        }

        /// <summary>
        /// Gets or sets a value based on the key. If the value is not defined in this PropertyBag, it will look in it's
        /// parent property bag.
        /// </summary>
        /// <value></value>
        /// <returns>The config value defined in the property bag. </returns>
        public string this[string key]
        {
            get
            {
                if (!this.Contains(key))
                    return null;

                return (string)this.farm.Properties[key];
            }
            set
            {
                this.farm.Properties[key] = value;
            }
        }

        /// <summary>
        /// Save any changes made to this PropertyBag.
        /// </summary>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public void Update()
        {
            this.farm.Update(true);
        }

        /// <summary>
        /// The config level this PropertyBag represents: <see cref="ConfigLevel.CurrentSPFarm"/>.
        /// </summary>
        /// <value></value>
        public ConfigLevel Level
        {
            get { return ConfigLevel.CurrentSPFarm; }
        }


        /// <summary>
        /// Remove a particular config setting from this property bag and all of it's child property bags, using the key.
        /// </summary>
        /// <param name="key">The key to remove</param>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public void Remove(string key)
        {
            if (this.Contains(key))
            {
                this.farm.Properties.Remove(key);
            }
        }

        /// <summary>
        /// The SPFarm doesn't have a parent PropertyBag, so it returns null. 
        /// </summary>
        /// <returns>null</returns>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public IPropertyBag GetParent()
        {
            return null;
        }
    }
}