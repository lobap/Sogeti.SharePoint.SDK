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
using Microsoft.SharePoint.Security;

namespace Microsoft.Practices.SPG.Common.Configuration
{
    /// <summary>
    /// A wrapper around the SPWeb property bag. 
    /// </summary>
    [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    public class SPWebPropertyBag : IPropertyBag
    {
        /// <summary>
        /// The SPWeb that's wrapped by this property bag. 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Sp")]
        protected SPWeb SpWeb{ get; set;}

        private IPropertyBag parent;

        /// <summary>
        /// The parent of this property bag. 
        /// </summary>
        public virtual IPropertyBag GetParent()
        {
            if (parent == null)
            {
                parent = new SPSitePropertyBag(this.SpWeb.Site);
            }

            return parent;
        }


        /// <summary>
        /// The config level this PropertyBag represents.
        /// </summary>
        /// <value></value>
        public virtual ConfigLevel Level
        {
            get { return ConfigLevel.CurrentSPWeb; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SPWebPropertyBag"/> class.
        /// </summary>
        /// <param name="spWeb">The SPWeb.</param>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        internal SPWebPropertyBag(SPWeb spWeb)
        {
            if (spWeb == null) throw new ArgumentNullException("spWeb");
            this.SpWeb = spWeb;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SPWebPropertyBag"/> class.
        /// </summary>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public SPWebPropertyBag()
        {
            if (SPContext.Current == null)
                throw new InvalidOperationException("This code should run in a SharePoint context");

            this.SpWeb = SPContext.Current.Web;
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
            return this.SpWeb.Properties.ContainsKey(BuildKey(key))
                // Don't use BuildKey() here, because that's done in the 'this[]' methods. 
                && this[key] != null;
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
                return this.SpWeb.Properties[BuildKey(key)];
            }
            set
            {
                this.SpWeb.Properties[BuildKey(key)] = value;
            }
        }

        /// <summary>
        /// Save any changes made to this PropertyBag.
        /// </summary>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public void Update()
        {
            SpWeb.Properties.Update();
        }

        /// <summary>
        /// Remove a particular config setting from this property bag and all of it's child property bags, using the key.
        /// </summary>
        /// <param name="key">The key to remove</param>
        [SharePointPermissionAttribute(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermissionAttribute(SecurityAction.LinkDemand, ObjectModel = true)]
        public virtual void Remove(string key)
        {
            if (this.Contains(key))
            {
                this[key] = null;
            }
        }

        /// <summary>
        /// Allow derived classes to change the key that's used
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual string BuildKey(string key)
        {
            return key;
        }
        
    }
}