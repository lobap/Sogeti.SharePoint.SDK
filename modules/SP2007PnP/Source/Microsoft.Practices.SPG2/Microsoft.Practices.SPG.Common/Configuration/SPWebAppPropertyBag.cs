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
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SPG.Common.Configuration
{
    /// <summary>
    /// Wrapper class around the SPWebApplication property bag. 
    /// </summary>
    [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    public class SPWebAppPropertyBag : IPropertyBag
    {
        private readonly SPWebApplication webApplication;
        private IPropertyBag parent;

        /// <summary>
        /// Get the parent proeprtybag. This will return the SPFarmPropertyBag. 
        /// </summary>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public IPropertyBag GetParent()
        {
            if (parent == null)
            {
                parent = new SPFarmPropertyBag(webApplication.Farm);
            }

            return parent;
        }

        /// <summary>
        /// The config level this property bag operates on. Returns <see cref="ConfigLevel.CurrentSPWebApplication"/>.
        /// </summary>

        public ConfigLevel Level
        {
            get { return ConfigLevel.CurrentSPWebApplication; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SPWebAppPropertyBag"/> class that uses the
        /// <see cref="SPContext.Current"/>.Site.WebApplication property find the Web Application
        /// </summary>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public SPWebAppPropertyBag()
        {
            this.webApplication = SPContext.Current.Site.WebApplication;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SPWebAppPropertyBag"/> class.
        /// </summary>
        /// <param name="webApplication">The Web Application.</param>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        internal SPWebAppPropertyBag(SPWebApplication webApplication)
        {
            if (webApplication == null) throw new ArgumentNullException("webApplication");
            this.webApplication = webApplication;
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
            return this.webApplication.Properties.ContainsKey(key);
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
                return (string)this.webApplication.Properties[key];
            }
            set
            {
                this.webApplication.Properties[key] = value;
            }
        }

        /// <summary>
        /// Save any changes made to this PropertyBag.
        /// </summary>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public void Update()
        {
            this.webApplication.Update(true);
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
                this.webApplication.Properties.Remove(key);
            }
        }
    }
}